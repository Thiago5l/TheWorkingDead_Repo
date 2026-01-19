 using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TaskOrdenar : TaskBase
{
    [SerializeField] UIPuzzleManager puzzleManager;

    // Update is called once per frame
    void Update()
    {
        if(puzzleManager.win == true)
        {
            puzzleManager.tiempoStart = false;
            Win();
        }
        if(puzzleManager.loose == true)
        {
            puzzleManager.tiempoStart = false;
            Loose();
        }
    }
    protected override void IniciarTarea()
    {
        puzzleManager.tiempoStart = true;
        Debug.Log("TareaIniciada");
        //BarraMear.value = BarraMear.maxValue;
    }
    protected override void CancelarTarea()
    { puzzleManager.tiempoStart = false; }
}
