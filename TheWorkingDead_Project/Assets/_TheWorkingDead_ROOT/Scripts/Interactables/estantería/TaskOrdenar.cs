using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TaskOrdenar : TaskBase
{
    [SerializeField] PuzzleManager puzzleManager;

    // Update is called once per frame
    void Update()
    {
        if(puzzleManager.win == true)
        {
            Win();
        }
        if(puzzleManager.loose == true)
        {
            Loose();
        }
    }
    protected override void IniciarTarea()
    {
        Debug.Log("TareaIniciada");
        //BarraMear.value = BarraMear.maxValue;
    }
    protected override void CancelarTarea()
    { }
}
