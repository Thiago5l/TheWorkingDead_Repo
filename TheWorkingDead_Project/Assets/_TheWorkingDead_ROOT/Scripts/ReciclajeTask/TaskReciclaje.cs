using UnityEngine;
using UnityEngine.UI;

public class TaskReciclaje : TaskBase
{
    [SerializeField] private SpawnerReciclajeUI spawnerReciclajeUI;
    [Header("Tiempo")]
    public Slider barraTiempo;
    public float tiempoMaximo = 10f;
    public float tiempoActual;
    public bool contandoTiempo;
    //private bool perderTarea = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        tiempoActual = tiempoMaximo;
        barraTiempo.value = tiempoMaximo;
        contandoTiempo = false;
        barraTiempo.maxValue = tiempoMaximo;
        //perderTarea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (contandoTiempo)
        {
            tiempoActual -= Time.deltaTime;
            barraTiempo.value = tiempoActual /*/ tiempoMaximo*/;

            if (tiempoActual <= 0f)
            {
                TiempoAgotado();
            }
        }
        if (spawnerReciclajeUI.correcto == true)
        {
            Win();
            spawnerReciclajeUI.correcto = false;
            spawnerReciclajeUI.rondaActual = 0;
        }
        if (spawnerReciclajeUI.fallo == true)
        {
            spawnerReciclajeUI.fallo = false;
            //perderTarea = false;
            Loose();
        }

    }

    protected override void IniciarTarea()
    {
        Debug.Log("Iniciar Tarea Reciclaje");
        //spawnerReciclajeUI.GenerarObjetos();
        spawnerReciclajeUI.rondaActual = 0;
        contandoTiempo = true;
        tiempoActual = tiempoMaximo;
        barraTiempo.value = 1f;

    }

    protected override void CancelarTarea()
    {
    }
    void TiempoAgotado()
    {
        tiempoActual = 0f;
        barraTiempo.value = 0f;
        contandoTiempo = false;
        //perderTarea = true;

        Debug.Log(" Tiempo agotado");
        Loose();

        // Aquí puedes:
        // - cancelar la tarea
        // - borrar líneas
        // - cerrar la UI
        // - avisar al gestor de misiones
    }
}
