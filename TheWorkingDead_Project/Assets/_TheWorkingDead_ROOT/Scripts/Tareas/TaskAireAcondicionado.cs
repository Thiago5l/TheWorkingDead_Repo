using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TaskAireAcondicionado : MonoBehaviour
{
    public string tagObjetivo = "AireAcondicionado";
    [SerializeField] PlayerController playerController;
    [SerializeField] public CoinManager CoinManager;
    [SerializeField] GameObject TaskCanvasCounter;
    [SerializeField] TMP_Text textocontador;
    [SerializeField] GameObject TaskCanvasWin;

    private List<AireAcondicionado> aires = new List<AireAcondicionado>();
    private void Awake()
    {
        if (playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();
        if (CoinManager == null)
            CoinManager = FindAnyObjectByType<CoinManager>();
    }
    void Start()
    {
        TaskCanvasWin.SetActive(false);
        TaskCanvasCounter.SetActive(false);
        GameObject[] objetosEncontrados = GameObject.FindGameObjectsWithTag(tagObjetivo);

        foreach (GameObject obj in objetosEncontrados)
        {
            if (obj.TryGetComponent<AireAcondicionado>(out AireAcondicionado aire))
            {
                aires.Add(aire);
            }
        }

        Debug.Log("Aires encontrados: " + aires.Count);
    }
    private void Update()
    {
        ActualizarContadorTarea();
        if(playerController.airesapagados>=3)
        {
            TaskCanvasWin.SetActive(true);
            playerController.airesapagados=0;
            playerController.coins++;
            CoinManager.Updatecoins();
            DetenerTareaAires();
        }
    }

    public void EmpezarTareaAires()
    {
        TaskCanvasCounter.SetActive(true);
        Debug.Log("tarea aires activada");
        foreach (AireAcondicionado aire in aires)
        {
            aire.Activar();
        }
    }

    public void DetenerTareaAires()
    {
        TaskCanvasCounter.SetActive(false);
        foreach (AireAcondicionado aire in aires)
        {
            aire.Desactivar();
        }
    }

    public void ActualizarContadorTarea()
    {
        textocontador.text = (playerController.airesapagados.ToString()+"/3");
    }
}
