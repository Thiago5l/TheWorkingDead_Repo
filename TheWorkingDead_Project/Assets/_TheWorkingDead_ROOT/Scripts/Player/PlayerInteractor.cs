using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [SerializeField] Transform respawnPoint; //Posición respawn
    [SerializeField] float respawnFallLimit;//Limite en -y que de ser alcanzado respawn
    Rigidbody playerRB;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y <= respawnFallLimit) Respawn();
    }

    
    void Respawn()
    {
        playerRB.linearVelocity = new Vector3(0, 0, 0);
        transform.position = respawnPoint.position;
    }

}
