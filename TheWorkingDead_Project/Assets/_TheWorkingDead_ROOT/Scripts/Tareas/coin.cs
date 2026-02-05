using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField] private CoinManager CoinManager;
    [SerializeField] private PlayerController playerController;
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    [Header("flags")]
    [SerializeField] private bool playercerca = false;
    [SerializeField] private bool cangivecoin;
    private void Start()
    {
        cangivecoin=true;
        if (Random.value <= 0.5f)
        {
            if (spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                transform.position = spawnPoints[randomIndex].position;
            }
        }
    }
    public void givecoin()
    {
        if (!playercerca || !cangivecoin) return;

        cangivecoin = false;
        playerController.coins++;
        CoinManager.Updatecoins();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playercerca = false;
        }
    }
}
