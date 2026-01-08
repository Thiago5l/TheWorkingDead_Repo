using UnityEngine;
using UnityEngine.AI;//librería necesaria para referenciar clases de NavMesh
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine.UI;


public enum PatrollMode
{
    Random,// modo de patrulla de destinos aleatorios
    Waypoints// modo de patrulla por puntos a sequir en orden
}

public class EnemyAiBase : MonoBehaviour
{

    #region General Variables


    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent;//ref al "cerebro" del sistema de ia
    [SerializeField] Transform target;// Ref al objetivo a perseguir
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask groundLayer;


    [Header("Parolling Stats")]
    [SerializeField] private PatrollMode patrollMode = PatrollMode.Random;
    //Variables de estado que son comunes a ambos modos de patrulla
    Vector3 walkPoint;//destino actual a perseguir
    bool walkPointSet;// ¿Hay un punto a perseguir fijado o debemos determinarlo?


    [Header("Parolling-Random")]
    [SerializeField] float walkPointRange;//Define el radio de detección de puntos a seguir alrrededor del agente


    [Header("Parolling-Waypoints")]
    [SerializeField] private List<Transform> waypoints;
    int currentWaypointIndex = 0;

    [Header("Attack configuration")]
    public float timeBetweenAttacks; // cadencia de disparo del enemigo
    bool alreadyattacked; //seguridad de ataques infinitos
    [SerializeField] GameObject proyectile;// ref prefab proyectil
    [SerializeField] Transform shootPoint;// ref al punto desde el que se disdpara
    [SerializeField] float shootSpeedZ;
    [SerializeField] float shootSpeedY;

    [SerializeField] float maxValSliderSospecha;
    [SerializeField] Slider sliderSospecha;
    float detectandoZombie;
    [SerializeField] float fillSpeed;
    bool isFillingBar;
    [SerializeField] GameObject looseCanvas;


    [Header("States and detections")]
    [SerializeField] float sightRange;//rango al partir del cual persigue a player
    [SerializeField] float attackRange;//rango al partir del cual ataca a player
    [SerializeField] float valorSumaZombiedad;
    [SerializeField] bool targetInSightRange;
    [SerializeField] bool targetInAttackRange;
    [SerializeField] GameObject playerObject;
    [SerializeField] public Light detectionlight;
    public Color colorInsideRange = Color.red;   // Color si está dentro del rango
    public Color colorOutsideRange = Color.green; // Color si está fuera del rango

    [Header("Optimization")]
    [SerializeField] float aiUpdateFrequency = 0.5f;

    [Header("Always Chase Settings")]
    [SerializeField] bool alwaysChaseAtStart = true; // persigue siempre al inicio
    [SerializeField] float chaseDelay = 2f;          // tiempo hasta empezar a respetar sightRange
    private bool useSightRange = false;               // controla si se respeta sightRange

    #endregion



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }

            else
            {
                Debug.LogError("no se pudo encontrar objeto con tag Player; revise los tags");
                this.enabled = false;// Deshabilita script de IA si no encuentra al player
            }
        }
        if (detectionlight == null)
        {
            detectionlight = GetComponentInChildren<Light>(); // Busca la luz en el mismo GameObject si no se asigna
        }

    }


    private void Start()
    {
        sliderSospecha.maxValue = maxValSliderSospecha;
        sliderSospecha.value = 0;
        sliderSospecha.gameObject.SetActive(false);

        // Inicia la IA
        StartCoroutine(AIUpdateROutine());

        // Control del comportamiento inicial de persecución
        if (alwaysChaseAtStart)
            StartCoroutine(EnableSightRangeAfterDelay());
        else
            useSightRange = true; // usa sightRange desde el inicio
    }


    private void Update()
    {
        if (isFillingBar)
        {
            sliderSospecha.value += fillSpeed * Time.deltaTime;

            if (sliderSospecha.value >= sliderSospecha.maxValue)
            {
                sliderSospecha.value = sliderSospecha.maxValue;
                looseCanvas.SetActive(true);
                playerObject.GetComponent<OviedadZombie>().Zombiedad = 1f;
            }
        }

        if (targetInSightRange)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        UpdateLightColor();

        //if (targetInSightRange) transform.LookAt(target);

        //if (targetInSightRange && !agent.isStopped)
        //{
        //    Vector3 dir = (target.position - transform.position).normalized;
        //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //}
    }
    private IEnumerator EnableSightRangeAfterDelay()
    {
        yield return new WaitForSeconds(chaseDelay);
        useSightRange = true;
    }

    void UpdateLightColor()
    {
        if (targetInAttackRange == true)
        {
            detectionlight.color = colorInsideRange;
            detectionlight.enabled = true;
        }
        else
        {
            detectionlight.color = colorOutsideRange;
            detectionlight.enabled = false;
        }
    }
    void Patrolling()
    {

        sliderSospecha.gameObject.SetActive(false);
        sliderSospecha.value = 0;
        isFillingBar = false;
        //playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
        alreadyattacked = false;
        //deolvemos la capacidad de moverse al agente 
        if (agent.isStopped) agent.isStopped = false;
        //se comprueba si el agente ha llegado al punto
        //Para ello, se usa un margen de distancia pequeña + stoppingDistance para asegurarnos
        if (walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            walkPointSet = false;//ya no tenemos punto a patrullar y se generará uno nuevo
        }


        //se comprueba si no tenemos punto de destino por lo que buscará uno nuevo segun el método de patrulla (random/puntos)
        if (!walkPointSet)
        {
            switch (patrollMode)
            {

                case PatrollMode.Random:
                    SearchWalkPoint_Random();
                    break;
                case PatrollMode.Waypoints:
                    SearchWalkPoint_Waypoints();
                    break;
            }
        }


        //isFillingBar = false;
        //sliderSospecha.value = 0;
        //sliderSospecha.gameObject.SetActive(false);

        //playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
        //alreadyattacked = false;

        //if (agent.isStopped) agent.isStopped = false;

        //if (walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        //    walkPointSet = false;

        //if (!walkPointSet)
        //{
        //    switch (patrollMode)
        //    {
        //        case PatrollMode.Random:
        //            SearchWalkPoint_Random();
        //            break;
        //        case PatrollMode.Waypoints:
        //            SearchWalkPoint_Waypoints();
        //            break;
        //    }
        //}

    }



    void SearchWalkPoint_Random()
    {
        //GENERAR UN PUNTO DE PATRULLA (DESTINATION RANDOM)
        //Paso 1: generar posición aleatoria
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //Paso 2: generar el punto con la posición determinada en formato navmesh
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        NavMeshHit hit;//almacen de unformación de impacto de rayo SOLO en bakeo de navmesh
        if (NavMesh.SamplePosition(randomPoint, out hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;//DEFINIR EL PUNTO A PERSEGUIR 
            agent.SetDestination(walkPoint);
            walkPointSet = true;
        }
    }


    void SearchWalkPoint_Waypoints()
    {
        //DETECTAR LOS PUNTOS DE PATRUYA FIJOS EN UNA LISTA Y HACER BUCLE ENTRE ELLOS
        //paso 0: ¿Tenemos Waypoints asignados?
        if (walkPoint == null || waypoints.Count == 0)
        {
            Debug.LogWarning("El agente funciona en modo Waypoints pero la lista de Waypoints es nula o tiene cero espacios" + "CAmbiando a modo Random");
            patrollMode = PatrollMode.Random;
            return;
        }
        //Paso 1: asignar el siguiente waypoint como destino
        walkPoint = waypoints[currentWaypointIndex].position;
        agent.SetDestination(walkPoint);

        walkPointSet = true;

        //Paso2: Cambiar el número de espacio de la lista a perseguir
        //... y en caso de llegar al fina, pasarlo a 0 de forma PRO
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

    }

    void ChaseTarget()
    {
        sliderSospecha.gameObject.SetActive(false);
        isFillingBar = false;
        sliderSospecha.value = 0;

        //playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
        alreadyattacked = false;

        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(target.position);

        //sliderSospecha.gameObject.SetActive(false);
        //playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
        //alreadyattacked = false;
        //if (agent.isStopped) agent.isStopped = false;//si el agente está parado dejará de estarlo 
        //agent.SetDestination(target.position);//cambia el destino del agente a la position del target

    }


    void AttackTarget()
    {

        agent.isStopped = true;
        sliderSospecha.gameObject.SetActive(true);
        isFillingBar = true;

        if (!alreadyattacked)
        {
            //playerObject.GetComponent<OviedadZombie>().sumValue *= 2f;
            alreadyattacked = true;
        }


        //agent.isStopped = true;//el agente se para y empieza a atacar
        //if (!alreadyattacked)
        //{
        //    sliderSospecha.gameObject.SetActive(true);
        //    playerObject.GetComponent<OviedadZombie>().sumValue *= 2f;

        //    //Rigidbody rb = Instantiate(proyectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        //    //rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
        //    //EL siquiente addforce solo se aplica si queremos catapulta
        //    //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse);
        //}
        //alreadyattacked = true;
        ////StartCoroutine(ResetAttackRoutine());

    }



    //IEnumerator ResetAttackRoutine()
    //{
    //    yield return new WaitForSeconds(30);
    //    yield return new WaitForSeconds(timeBetweenAttacks);
    //    alreadyattacked = false; // permitir que el ataque se ejecute de nuevo
    //}




    //corrutine de funcionamiento de la IA (Cerebro de la IA)
   private IEnumerator AIUpdateROutine()
{
    while (true)
    {
        yield return new WaitForSeconds(aiUpdateFrequency);

        // Detección del target usando sightRange solo si está habilitado
        if (useSightRange)
            targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        else
            targetInSightRange = true; // persigue siempre al inicio

        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        // Cambio de estados
        if (targetInSightRange && targetInAttackRange)
        {
            AttackTarget();
        }
        else if (targetInSightRange && !targetInAttackRange)
        {
            ChaseTarget();
        }
        else
        {
            Patrolling(); // patrulla cuando no ve al target
        }
    }
}








    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

}




















//using UnityEngine;
//using UnityEngine.AI;//librería necesaria para referenciar clases de NavMesh
//using System.Collections;
//using System.Collections.Generic;
//using System.Transactions;
//using UnityEngine.UI;


//public enum PatrollMode
//{
//    Random,// modo de patrulla de destinos aleatorios
//    Waypoints// modo de patrulla por puntos a sequir en orden
//}

//public class EnemyAiBase : MonoBehaviour
//{

//    #region General Variables

//    Coroutine waitCoroutine;



//    [Header("AI Configuration")]
//    [SerializeField] NavMeshAgent agent;//ref al "cerebro" del sistema de ia
//    [SerializeField] Transform target;// Ref al objetivo a perseguir
//    [SerializeField] LayerMask targetLayer;
//    [SerializeField] LayerMask groundLayer;


//    [Header("Parolling Stats")]
//    [SerializeField] private PatrollMode patrollMode = PatrollMode.Random;
//    //Variables de estado que son comunes a ambos modos de patrulla
//    Vector3 walkPoint;//destino actual a perseguir
//    bool walkPointSet;// ¿Hay un punto a perseguir fijado o debemos determinarlo?


//    [Header("Parolling-Random")]
//    [SerializeField] float walkPointRange;//Define el radio de detección de puntos a seguir alrrededor del agente


//    [Header("Parolling-Waypoints")]
//    [SerializeField] private List<Transform> waypoints;
//    int currentWaypointIndex = 0;

//    [Header("Attack configuration")]
//    public float timeBetweenAttacks; // cadencia de disparo del enemigo
//    bool alreadyattacked; //seguridad de ataques infinitos
//    [SerializeField] float detectandoPlayer;
//    [SerializeField] float maxValSliderSospecha;
//    [SerializeField] Slider sliderSospecha;
//    [SerializeField] bool atacando;


//    [Header("States and detections")]
//    [SerializeField] float sightRange;//rango al partir del cual persigue a player
//    [SerializeField] float attackRange;//rango al partir del cual ataca a player
//    [SerializeField] float valorSumaZombiedad;
//    [SerializeField] bool targetInSightRange;
//    [SerializeField] bool targetInAttackRange;
//    [SerializeField] GameObject playerObject;
//    [SerializeField]public Light detectionlight;
//    public Color colorInsideRange = Color.red;   // Color si está dentro del rango
//    public Color colorOutsideRange = Color.green; // Color si está fuera del rango

//    [Header("Optimization")]
//    [SerializeField] float aiUpdateFrequency = 0.5f;

//    #endregion



//    private void Awake()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        if(target == null)
//        {
//            playerObject = GameObject.FindGameObjectWithTag("Player");
//            if(playerObject != null)
//            {
//                target = playerObject.transform;
//            }

//            else
//            {
//                this.enabled = false;// Deshabilita script de IA si no encuentra al player
//            }
//        }
//        if (detectionlight == null)
//        {
//            detectionlight = GetComponentInChildren<Light>(); // Busca la luz en el mismo GameObject si no se asigna
//        }

//    }


//    private void Start()
//    {
//        valorSumaZombiedad = playerObject.GetComponent<OviedadZombie>().sumValue;
//        //Arranque de la corrutina de proceso de la IA que sustituya al Update
//        StartCoroutine(AIUpdateROutine());
//        sliderSospecha.maxValue = maxValSliderSospecha;

//        if (playerObject != null)
//        {
//            valorSumaZombiedad = playerObject.GetComponent<OviedadZombie>().sumValue;
//        }

//    }


//    private void Update()
//    {


//        if (targetInSightRange)
//        {
//            Vector3 direction = target.position - transform.position;
//            direction.y = 0;
//            if (direction != Vector3.zero)
//            {
//                Quaternion lookRotation = Quaternion.LookRotation(direction);
//                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
//            }
//        }
//        UpdateLightColor();

//        //if (targetInSightRange) transform.LookAt(target);

//        //if (targetInSightRange && !agent.isStopped)
//        //{
//        //    Vector3 dir = (target.position - transform.position).normalized;
//        //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
//        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
//        //}
//    }

//    void UpdateLightColor()
//    {
//        if (targetInAttackRange==true)
//        { detectionlight.color=colorInsideRange;
//            detectionlight.enabled=true;
//        }
//        else
//        { detectionlight.color=colorOutsideRange;
//            detectionlight.enabled = false;
//        }
//    }
//    void Patrolling()
//    {
//        atacando = false;
//        playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
//        alreadyattacked = false;
//        //deolvemos la capacidad e moverse al agene 
//        if (agent.isStopped) agent.isStopped = false;
//        //se comprueba si el agente ha llegado al punto
//        //Para ello, se usa un margen de distancia pequeña + stoppingDistance para asegurarnos
//        if(walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
//        {
//            walkPointSet = false;//ya no tenemos punto a patrullar y se generará uno nuevo
//        }


//        //se comprueba si no tenemos punto de destino por lo que buscará uno nuevo segun el método de patrulla (random/puntos)
//        if (!walkPointSet)
//        {
//            switch(patrollMode)
//            {

//                case PatrollMode.Random:
//                    SearchWalkPoint_Random();
//                    break;
//                case PatrollMode.Waypoints:
//                    SearchWalkPoint_Waypoints();
//                    break;
//            }
//        }

//    }



//    void SearchWalkPoint_Random()
//    {
//        //GENERAR UN PUNTO DE PATRULLA (DESTINATION RANDOM)
//        //Paso 1: generar posición aleatoria
//        float randomZ =  Random.Range(-walkPointRange, walkPointRange);
//        float randomX =  Random.Range(-walkPointRange, walkPointRange);
//        //Paso 2: generar el punto con la posición determinada en formato navmesh
//        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
//        NavMeshHit hit;//almacen de unformación de impacto de rayo SOLO en bakeo de navmesh
//        if(NavMesh.SamplePosition(randomPoint, out hit, walkPointRange, NavMesh.AllAreas))
//        {
//            walkPoint = hit.position;//DEFINIR EL PUNTO A PERSEGUIR 
//            agent.SetDestination(walkPoint);    
//            walkPointSet = true;
//        }

//    }


//    void SearchWalkPoint_Waypoints()
//    {
//        //DETECTAR LOS PUNTOS DE PATRUYA FIJOS EN UNA LISTA Y HACER BUCLE ENTRE ELLOS
//        //paso 0: ¿Tenemos Waypoints asignados?
//        if(walkPoint == null || waypoints.Count == 0)
//        {
//            Debug.LogWarning("El agente funciona en modo Waypoints pero la lista de Waypoints es nula o tiene cero espacios" + "CAmbiando a modo Random");
//            patrollMode = PatrollMode.Random;
//            return;
//        }
//        //Paso 1: asignar el siguiente waypoint como destino
//        walkPoint = waypoints[currentWaypointIndex].position;
//        agent.SetDestination(walkPoint);

//        walkPointSet = true;

//        //Paso2: Cambiar el número de espacio de la lista a perseguir
//        //... y en caso de llegar al fina, pasarlo a 0 de forma PRO
//        currentWaypointIndex = (currentWaypointIndex+1) % waypoints.Count;

//    }

//    void ChaseTarget()
//    {
//        atacando = false;
//        playerObject.GetComponent<OviedadZombie>().sumValue = valorSumaZombiedad;
//        alreadyattacked = false;
//        if(agent.isStopped) agent.isStopped = false;//si el agente está parado dejará de estarlo 
//        agent.SetDestination(target.position);//cambia el destino del agente a la position del target

//        if (waitCoroutine != null)
//        {
//            StopCoroutine(waitCoroutine);
//            waitCoroutine = null;
//        }
//    }



//    void AttackTarget()
//    {
//        atacando = true;




//        agent.isStopped = true;//el agente se para y empieza a atacar
//        if (!alreadyattacked)
//        {
//            if (waitCoroutine == null)
//            {
//                waitCoroutine = StartCoroutine(WaitToLoose());
//            }
//            playerObject.GetComponent<OviedadZombie>().sumValue *= 2f;
//            alreadyattacked = true;

//            //Rigidbody rb = Instantiate(proyectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
//            //rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
//            //EL siquiente addforce solo se aplica si queremos catapulta
//            //rb.AddForce(transform.up * shootSpeedY, ForceMode.Impulse);
//        }
//        //StartCoroutine(ResetAttackRoutine());

//    }



//    //IEnumerator ResetAttackRoutine()
//    //{
//    //    yield return new WaitForSeconds(30);
//    //    yield return new WaitForSeconds(timeBetweenAttacks);
//    //    alreadyattacked = false; // permitir que el ataque se ejecute de nuevo
//    //}




//    //corrutine de funcionamiento de la IA (Cerebro de la IA)
//    private IEnumerator AIUpdateROutine()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(aiUpdateFrequency);

//            //Chequear detección de target

//            targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
//            targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

//            //Paso 2: detección y cambio entre estados

//            if(targetInSightRange &&  targetInAttackRange)
//            {
//                //atacar
//                AttackTarget();
//            }
//            else if(targetInSightRange && !targetInAttackRange)
//            {
//                //persecución
//                ChaseTarget();
//            }

//            else
//            {
//                //patrullar
//                Patrolling(); 
//            }
//            Debug.Log("Sight: " + targetInSightRange + " | Attack: " + targetInAttackRange);

//        }
//    }


//    IEnumerator WaitToLoose()
//    {
//        yield return new WaitForSeconds(detectandoPlayer);
//        sliderSospecha.value += 1;
//    }




//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, attackRange);

//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, sightRange);

//    }

//}
