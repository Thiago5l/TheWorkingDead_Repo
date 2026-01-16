using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TareaMear : TaskBase
{
    public Slider BarraMear;
    public GameObject canvasTarea;
    public FadeCanvas FadeCanvas;
    void Update()
    {
        

        if (BarraMear.value >= BarraMear.maxValue)
        {
            Win();
            tareaAcabada = true;
        }
        if (BarraMear.value <= BarraMear.minValue)
        {
            Loose();
        }

        if (tareaAcabada)
        {
            tareaAcabada = true;
            canvasTarea.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = false;
        }
    }
    protected override void IniciarTarea()
    {
        Debug.Log("TareaIniciada");
        //BarraMear.value = BarraMear.maxValue;
    }
    protected override void CancelarTarea()
    { }
    IEnumerator VaciarBarra()
    {
        yield return new WaitForSeconds(2f);
        BarraMear.value -= BarraMear.maxValue * 0.02f;
    }
}
