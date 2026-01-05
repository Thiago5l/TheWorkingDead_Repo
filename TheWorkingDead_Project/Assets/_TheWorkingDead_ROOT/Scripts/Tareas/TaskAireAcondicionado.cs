using UnityEngine;
using System.Collections.Generic;

public class TaskAireAcondicionado : MonoBehaviour
{
    public string tagObjetivo = "AireAcondicionado";
    [SerializeField] PlayerController playerController;
    [SerializeField] public CoinManager CoinManager;

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
        if(playerController.airesapagados>=3)
        { 
            playerController.airesapagados=0;
            playerController.coins++;
            CoinManager.Updatecoins();
        }
    }

    public void EmpezarTareaAires()
    {
        Debug.Log("tarea aires activada");
        foreach (AireAcondicionado aire in aires)
        {
            aire.Activar();
        }
    }

    public void DetenerTareaAires()
    {
        foreach (AireAcondicionado aire in aires)
        {
            aire.Desactivar();
        }
    }
}
