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
        ActualizarAnimacionMovimiento();
    }

    private void ActualizarAnimacionMovimiento()
    {

        Vector3 velocidadCaminado = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float velocidad = velocidadCaminado.magnitude;
        if (velocidad > velocidadUmbral && velocidad<2.1f)
            anim.SetFloat("Movimiento", 0.5f);
        else if (velocidad >= 2.1f)
            anim.SetFloat("Movimiento", 01f);
        else
            anim.SetFloat("Movimiento", 0f);
    }
}
