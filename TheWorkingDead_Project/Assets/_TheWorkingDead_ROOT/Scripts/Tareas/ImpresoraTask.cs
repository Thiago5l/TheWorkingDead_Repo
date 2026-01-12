using UnityEngine;
using System.Collections;
public class ImpresoraTask : TaskBase
{
    [Header("Barra")]
    [SerializeField] float value;
    [SerializeField] float suma;
    [SerializeField] float resta;
    [SerializeField] float tiempo;
    [SerializeField] GameObject taskBar;

    private bool interactuando;

    protected override void IniciarTarea()
    {
        interactuando = true;
        taskBar.SetActive(true);
        player.GetComponent<PlayerController>().playerOcupado = true;
        StartCoroutine(Drain());
    }

    IEnumerator Drain()
    {
        while (value > 0 && value < 100)
        {
            yield return new WaitForSeconds(tiempo);
            value -= resta;
        }

        if (value <= 0)
            CancelarTarea();
    }

    public void Pulsar()
    {
        if (!interactuando) return;

        value += suma;
        if (value >= 100)
            Completar();
    }

    protected override void CancelarTarea()
    {
        interactuando = false;
        taskBar.SetActive(false);
        player.GetComponent<PlayerController>().playerOcupado = false;
        StopAllCoroutines();
    }
}
