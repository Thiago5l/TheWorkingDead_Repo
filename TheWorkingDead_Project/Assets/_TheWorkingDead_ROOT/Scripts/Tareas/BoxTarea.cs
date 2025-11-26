using TMPro;
using UnityEngine;

public class BoxTarea : MonoBehaviour
{
    

    private Animator animator;
    [SerializeField] TMP_Text textoTarea;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void SetText(string text)
    {
        textoTarea.text = text;
    }


}
