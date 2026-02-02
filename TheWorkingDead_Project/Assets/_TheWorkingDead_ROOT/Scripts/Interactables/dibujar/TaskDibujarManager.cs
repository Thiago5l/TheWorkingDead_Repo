using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TaskDibujarManager : TaskBase
{
    [SerializeField] private TareaDibujar TareaDibujar;
    [SerializeField] private GameObject uIGameObject;
    //[SerializeField] private Texture2D rotulador;
    //[SerializeField] private CursorSprite cursorSprite;
    [SerializeField] private NewCursor newCursor;
    [SerializeField] private Sprite cursorRotu;
    [SerializeField] private Image cursorDef;
    [SerializeField] private RectTransform saverLineas;

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
                newCursor.CursorToNormalState();
                Win();
                dibujoUI.tareaCompletada = false;
            }
            if(TareaDibujar.perderTarea)
            {
                newCursor.CursorToNormalState();
                foreach(RectTransform child in saverLineas)
                {
                    Destroy(child.gameObject);
                }
                dibujoUI.LimpiarDibujo();
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
        newCursor.CambiarCursor(cursorRotu);
        Debug.Log("TareaIniciada");
        isTaskActive = true;
    }

    protected override void CancelarTarea()
    {
        uIGameObject.SetActive(false);
    }
}
