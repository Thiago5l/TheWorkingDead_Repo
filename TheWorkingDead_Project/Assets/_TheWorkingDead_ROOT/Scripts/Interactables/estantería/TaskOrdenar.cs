 using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TaskOrdenar : TaskBase
{
    [SerializeField] UIPuzzleManager uiPuzzleManager;
    [SerializeField] GameObject gameObject;

    // Update is called once per frame
    void Update()
    {
        if(uiPuzzleManager.win == true)
        {
            uiPuzzleManager.tiempoStart = false;
            gameObject.gameObject.SetActive(false);
            Win();
        }
        if(uiPuzzleManager.loose == true)
        {
            uiPuzzleManager.tiempoStart = false;
            uiPuzzleManager.tiempoStart = false;
            Loose();
        }
    }
    protected override void IniciarTarea()
    {
        uiPuzzleManager.tiempoStart = true;
        Debug.Log("TareaIniciada");
        //BarraMear.value = BarraMear.maxValue;
    }
    protected override void CancelarTarea()
    { 
        uiPuzzleManager.tiempoStart = false;
        uiPuzzleManager.tiempoStart = false;

    }
}
