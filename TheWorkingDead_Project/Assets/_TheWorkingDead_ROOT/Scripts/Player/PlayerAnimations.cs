using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] float velocidadUmbral = 0.1f;
    [SerializeField] PlayerController playerController;
    private void Awake()
    {
        // Si el script está en un hijo, busca el Rigidbody en el padre
        rb = GetComponentInParent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        if (playerController==null)
        playerController = GetComponentInChildren<PlayerController>();
    }

    private void Update()
    {
        ActualizarAnimacionMovimiento();
    }

    private void ActualizarAnimacionMovimiento()
    {

        Vector3 velocidadCaminado = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float velocidad = velocidadCaminado.magnitude;
        if (playerController.isSprinting)
            anim.SetFloat("Movimiento", 1f);
        else if (velocidad >= 0.1)
            anim.SetFloat("Movimiento", 0.5f);
        else
            anim.SetFloat("Movimiento", 0f);
    }
}
