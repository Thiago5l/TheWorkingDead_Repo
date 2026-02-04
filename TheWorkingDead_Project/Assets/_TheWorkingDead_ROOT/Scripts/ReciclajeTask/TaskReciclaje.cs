using UnityEngine;

public class TaskReciclaje : TaskBase
{
    [SerializeField] private SpawnerReciclajeUI spawnerReciclajeUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerReciclajeUI.correcto == true)
        {
            Win();
            spawnerReciclajeUI.correcto = false;
            spawnerReciclajeUI.rondaActual = 0;
        }
        if (spawnerReciclajeUI.fallo == true)
        {
            spawnerReciclajeUI.fallo = false;
            Loose();
        }
    }

    protected override void IniciarTarea()
    {
        Debug.Log("Iniciar Tarea Reciclaje");
        //spawnerReciclajeUI.GenerarObjetos();
        spawnerReciclajeUI.rondaActual = 0;
    }

    protected override void CancelarTarea()
    {
    }

}
