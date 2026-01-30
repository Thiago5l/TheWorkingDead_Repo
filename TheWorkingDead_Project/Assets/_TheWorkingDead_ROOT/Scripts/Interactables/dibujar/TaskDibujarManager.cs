using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TaskDibujarManager : TaskBase
{
    [SerializeField] private TareaDibujar TareaDibujar;
    [SerializeField] private GameObject uIGameObject;
    [SerializeField] private Texture2D rotulador;
    [SerializeField] private CursorSprite cursorSprite;
    [SerializeField] private DibujoUI dibujoUI;
    [SerializeField] private bool isTaskActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uIGameObject.SetActive(false);
        isTaskActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTaskActive)
        {
            if(dibujoUI.tareaCompletada)
            {
                Win();
                dibujoUI.tareaCompletada = false;
            }
            if(TareaDibujar.perderTarea)
            {
                Loose();
                TareaDibujar.perderTarea = false;
                
            }
        }
    }

    protected override void IniciarTarea()
    {
        TareaDibujar.contandoTiempo = false;
        TareaDibujar.tiempoActual = TareaDibujar.tiempoMaximo;
        uIGameObject.SetActive(true);
        cursorSprite.CambiarCursor(rotulador);
        Debug.Log("TareaIniciada");
        isTaskActive = true;
    }

    protected override void CancelarTarea()
    {
        uIGameObject.SetActive(false);
    }
}
