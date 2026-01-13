using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TareaMear : MonoBehaviour
{

    [SerializeField] private bool playerCerca;
    public GameObject canvasInteractableKey;
    public GameObject canvasTarea;
    public Slider BarraMear;
    public bool tareaEmpezada;
    public bool tareaAcabada;
    public GameObject player;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Entra");
            playerCerca = true;
            canvasInteractableKey.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Sale");
            playerCerca = false;
            canvasInteractableKey.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BarraMear.value = BarraMear.maxValue;
        tareaEmpezada = false;
        canvasTarea.SetActive(false);
    }

    public void Interactuar()
    {
        if (playerCerca && !tareaEmpezada)
        {
            player.GetComponent<PlayerController>().playerOcupado = true;
            tareaEmpezada = true;
            canvasTarea.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(canvasTarea.GetComponent<MearUI>().meandoDentro && tareaEmpezada)
        {
            StartCoroutine(VaciarBarra());
        }
        if (!canvasTarea.GetComponent<MearUI>().meandoDentro || !tareaEmpezada || tareaAcabada)
        {
            StopCoroutine(VaciarBarra());
        }

        if (BarraMear.value <= 0 && tareaEmpezada && !tareaAcabada)
        {
            tareaAcabada = true;
        }

        if (tareaAcabada)
        {
            tareaAcabada = true;
            tareaEmpezada = false;
            canvasTarea.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = false;
            canvasInteractableKey.SetActive(false);
        }
    }
    IEnumerator VaciarBarra()
    {
        yield return new WaitForSeconds(2f);
        BarraMear.value -= BarraMear.maxValue*0.02f;
    }
}
