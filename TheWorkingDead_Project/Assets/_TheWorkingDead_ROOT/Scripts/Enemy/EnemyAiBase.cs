using UnityEngine;
using UnityEngine.AI; // libreria necesaria para referenciar clases de NavMesh
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PatrollMode
{
    Random,     // modo de patrulla de destinos aleatorios
    Waypoints   // modo de patrulla por puntos a seguir en orden
}

public class EnemyAiBase : MonoBehaviour
{
    #region General Variables

    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; // ref al "cerebro" del sistema de ia
    [SerializeField] Transform target;   // ref al objetivo a perseguir
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask groundLayer;

    [Header("Patrolling Stats")]
    [SerializeField] private PatrollMode patrollMode = PatrollMode.Random;
    Vector3 walkPoint;       // destino actual a perseguir
    bool walkPointSet;       // hay un punto a perseguir fijado o debemos determinarlo?

    [Header("Patrolling Random")]
    [SerializeField] float walkPointRange; // define el radio de deteccion de puntos alrededor del agente

    [Header("Patrolling Waypoints")]
    [SerializeField] private List<Transform> waypoints;
    int currentWaypointIndex = 0;

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; // cadencia de disparo del enemigo
    bool alreadyattacked;           // seguridad de ataques infinitos
    [SerializeField] GameObject proyectile; // ref prefab proyectil
    [SerializeField] Transform shootPoint;  // ref al punto desde el que se dispara
    [SerializeField] float shootSpeedZ;
    [SerializeField] float shootSpeedY;

    [SerializeField] float maxValSliderSospecha;
    [SerializeField] Slider sliderSospecha;
    float detectandoZombie;
    [SerializeField] float fillSpeed;
    public bool isFillingBar;
    [SerializeField] GameObject looseCanvas;

    [Header("States and Detections")]
    [SerializeField] public float sightRange;   // rango al partir del cual persigue al player
    [SerializeField] public float attackRange;  // rango al partir del cual ataca al player
    [SerializeField] float valorSumaZombiedad;
    [SerializeField] public bool targetInSightRange;
    [SerializeField] public bool targetInAttackRange;
    [SerializeField] GameObject playerObject;
    [SerializeField] public Light detectionlight;
    public Color colorInsideRange = Color.red;   // color si esta dentro del rango
    public Color colorOutsideRange = Color.green; // color si esta fuera del rango
    public CanvasGroup exclamation;
    bool wasChasing = false;
    Coroutine exclamationRoutine;
    bool wasFillingBar = false;

    [Header("Optimization")]
    [SerializeField] float aiUpdateFrequency = 0.5f;

    [Header("Always Chase Settings")]
    [SerializeField] bool alwaysChaseAtStart = true; // persigue siempre al inicio
    [SerializeField] float chaseDelay = 2f;          // tiempo hasta empezar a respetar sightRange
    private bool useSightRange = false;               // controla si se respeta sightRange

    [Header("Field of View")]
    [SerializeField, Range(0, 360)] private float viewAngle = 120f; // angulo de vision del enemigo
    [SerializeField] private float viewDistance = 10f;             // distancia maxima de vision
    [SerializeField] private LayerMask obstacleMask;               // para raycast de obstaculos
    [Header("Vision Cone")]
    public VisionCone visionCone;


    #endregion

    #region Unity Methods
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                target = playerObject.transform;
            else
            {
                Debug.LogError("No se pudo encontrar objeto con tag Player; revise los tags");
                this.enabled = false; // deshabilita IA si no encuentra al player
            }
        }

        if (detectionlight == null)
            detectionlight = GetComponentInChildren<Light>(); // busca luz en hijos
    }

    private void Start()
    {
        exclamation.alpha = 0;
        sliderSospecha.maxValue = maxValSliderSospecha;
        sliderSospecha.value = 0;
        sliderSospecha.gameObject.SetActive(false);

        StartCoroutine(AIUpdateROutine());

        if (alwaysChaseAtStart)
            StartCoroutine(EnableSightRangeAfterDelay());
        else
            useSightRange = true;
    }

    private void Update()
    {
        if (isFillingBar)
        {
            RotateTowardsTarget();
            sliderSospecha.value += fillSpeed * Time.deltaTime;

            if (sliderSospecha.value >= sliderSospecha.maxValue)
            {
                sliderSospecha.value = sliderSospecha.maxValue;
                looseCanvas.SetActive(true);
                playerObject.GetComponent<OviedadZombie>().Zombiedad = 1f;
            }
        }

        if (targetInSightRange && !targetInAttackRange)
        {
            RotateTowardsTarget();
            if (!wasChasing && !wasFillingBar)
            {
                if (exclamationRoutine != null)
                    StopCoroutine(exclamationRoutine);

                exclamationRoutine = StartCoroutine(EnableExclamation());
            }

            wasChasing = true;
        }
        else
        {
            wasChasing = false;
        }

        wasFillingBar = isFillingBar;
        UpdateLightColor();
    }
    #endregion

    #region Coroutines
    private IEnumerator EnableSightRangeAfterDelay()
    {
        yield return new WaitForSeconds(chaseDelay);
        useSightRange = true;
    }

    private IEnumerator EnableExclamation(float duration = 1f)
    {
        exclamation.alpha = 1;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            exclamation.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }
        exclamation.alpha = 0;
    }

    private IEnumerator AIUpdateROutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(aiUpdateFrequency);

            targetInSightRange = useSightRange && IsTargetInSight();
            targetInAttackRange = Vector3.Distance(transform.position, target.position) <= attackRange;

            if (targetInSightRange && targetInAttackRange)
                AttackTarget();
            else if (targetInSightRange && !targetInAttackRange)
                ChaseTarget();
            else
                Patrolling();
        }
    }

    private bool IsTargetInSight()
    {
        if (target == null || visionCone == null) return false;

        // Origen del raycast a la altura del ojo del enemigo
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        // Apuntar al centro del jugador
        Vector3 targetCenter = target.position + Vector3.up * 0.5f;
        Vector3 directionToTarget = (targetCenter - rayOrigin).normalized;
        float distanceToTarget = Vector3.Distance(rayOrigin, targetCenter);

        // Usar la distancia del cono de visión
        float maxDistance = visionCone.viewDistance;
        if (distanceToTarget > maxDistance) return false;

        // Usar el ángulo del cono de visión
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget > visionCone.viewAngle / 2f) return false;

        // Raycast para verificar obstáculos
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, directionToTarget, out hit, maxDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0 || hit.collider.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }





    #endregion

    #region AI Behaviors
    private void UpdateLightColor()
    {
        if (targetInAttackRange)
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

    private void Patrolling()
    {
        sliderSospecha.gameObject.SetActive(false);
        sliderSospecha.value = 0;
        isFillingBar = false;
        alreadyattacked = false;

        if (agent.isStopped) agent.isStopped = false;

        if (walkPointSet && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
            walkPointSet = false;

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
    }

    private void SearchWalkPoint_Random()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            agent.SetDestination(walkPoint);
            walkPointSet = true;
        }
    }

    private void SearchWalkPoint_Waypoints()
    {
        if (walkPoint == null || waypoints.Count == 0)
        {
            Debug.LogWarning("Lista de Waypoints vacia, cambiando a Random");
            patrollMode = PatrollMode.Random;
            return;
        }

        walkPoint = waypoints[currentWaypointIndex].position;
        agent.SetDestination(walkPoint);
        walkPointSet = true;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    private void ChaseTarget()
    {
        sliderSospecha.gameObject.SetActive(false);
        isFillingBar = false;
        sliderSospecha.value = 0;
        alreadyattacked = false;

        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        agent.isStopped = true;
        sliderSospecha.gameObject.SetActive(true);
        isFillingBar = true;

        if (!alreadyattacked)
        {
            alreadyattacked = true;
        }
    }

    [SerializeField] private float rotationSpeed = 5f; // velocidad de giro suave

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // mantener solo rotacion horizontal
        if (direction.magnitude == 0) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Cono de visión
        Gizmos.color = Color.cyan;
        Vector3 forward = transform.forward * viewDistance;
        Quaternion leftRayRotation = Quaternion.Euler(0, -viewAngle / 2f, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, viewAngle / 2f, 0);
        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(transform.position + Vector3.up * 1.0f, leftRayDirection);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.0f, rightRayDirection);

        // Línea directa al jugador
        if (target != null)
            Gizmos.DrawLine(transform.position + Vector3.up * 1.0f, target.position + Vector3.up * 1.0f);
    }


    #endregion
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
