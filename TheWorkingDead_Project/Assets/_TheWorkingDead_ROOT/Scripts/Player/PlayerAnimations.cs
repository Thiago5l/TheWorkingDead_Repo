using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] float velocidadUmbral = 0.1f;

    private void Awake()
    {
        // Si el script está en un hijo, busca el Rigidbody en el padre
        rb = GetComponentInParent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        Debug.Log("PlayerAnimations activo");
        ActualizarAnimacionMovimiento();
    }

    private void ActualizarAnimacionMovimiento()
    {

        Vector3 velocidadCaminado = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float velocidad = velocidadCaminado.magnitude;
        if (velocidad > velocidadUmbral)
            anim.SetFloat("Movimiento", 1f);
        else
            anim.SetFloat("Movimiento", 0f);
    }
}
