using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TareaMear : TaskBase
{
    public Slider BarraMear;
    public GameObject canvasTarea;
    void Update()
    {
        if(canvasTarea.GetComponent<MearUI>().meandoDentro)
        {
            StartCoroutine(VaciarBarra());
        }
        if (!canvasTarea.GetComponent<MearUI>().meandoDentro || tareaAcabada)
        {
            StopCoroutine(VaciarBarra());
        }

        if (BarraMear.value <= 0 && !tareaAcabada)
        {
            tareaAcabada = true;
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
        BarraMear.value = BarraMear.maxValue;
    }
    protected override void CancelarTarea()
    { }
    IEnumerator VaciarBarra()
    {
        yield return new WaitForSeconds(2f);
        BarraMear.value -= BarraMear.maxValue*0.02f;
    }
}
