using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskDibujarManager : TaskBase
{
    [SerializeField] private GameObject taskDibujar;
    [SerializeField] private GameObject uIGameObject;
    [SerializeField] private Texture2D rotulador;
    [SerializeField] private CursorSprite cursorSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uIGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void IniciarTarea()
    {
        
        uIGameObject.SetActive(true);
        cursorSprite.CambiarCursor(rotulador);
        Debug.Log("TareaIniciada");
    }

    protected override void CancelarTarea()
    {
        uIGameObject.SetActive(false);
    }
}
