using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TareaMear : TaskBase
{
    public Slider BarraMear;

    private bool resultadoEnviado = false;

    void Update()
    {
        // Si ya se envio el resultado, no volver a ejecutar
        if (resultadoEnviado) return;

        if (BarraMear.value >= BarraMear.maxValue)
        {
            resultadoEnviado = true;
            tareaAcabada = true;
            Win();
        }
        else if (BarraMear.value <= BarraMear.minValue)
        {
            resultadoEnviado = true;
            Loose();
        }

        if (tareaAcabada)
        {
            player.GetComponent<PlayerController>().playerOcupado = false;
        }
    }

    protected override void IniciarTarea()
    {
        Debug.Log("TareaIniciada");

        resultadoEnviado = false;
        tareaAcabada = false;

        BarraMear.value = BarraMear.maxValue / 4;
    }

    protected override void CancelarTarea()
    {
    }

    IEnumerator VaciarBarra()
    {
        yield return new WaitForSeconds(2f);
        BarraMear.value -= BarraMear.maxValue * 0.02f;
    }
}
