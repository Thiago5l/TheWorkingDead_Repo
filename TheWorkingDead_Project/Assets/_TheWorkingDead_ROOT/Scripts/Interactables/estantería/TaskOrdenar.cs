using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskOrdenar : TaskBase
{
    [SerializeField] UIPuzzleManager uiPuzzleManager;
    [SerializeField] GameObject UIgameObject;

    private bool resultadoEnviado = false;

    void Update()
    {
        if (resultadoEnviado) return;

        if (uiPuzzleManager.win)
        {
            resultadoEnviado = true;

            uiPuzzleManager.tiempoStart = false;
            UIgameObject.SetActive(false);

            Win();
        }
        else if (uiPuzzleManager.loose)
        {
            resultadoEnviado = true;

            uiPuzzleManager.tiempoStart = false;
            UIgameObject.SetActive(false);

            Loose();
        }
    }

    protected override void IniciarTarea()
    {
        resultadoEnviado = false;

        uiPuzzleManager.win = false;
        uiPuzzleManager.loose = false;

        uiPuzzleManager.time = uiPuzzleManager.maxTime;
        uiPuzzleManager.tiempoStart = true;


        Debug.Log("TareaIniciada");
    }

    protected override void CancelarTarea()
    {
        resultadoEnviado = false;

        uiPuzzleManager.tiempoStart = false;
        UIgameObject.SetActive(false);
    }
}
