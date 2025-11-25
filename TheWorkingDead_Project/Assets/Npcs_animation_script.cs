using UnityEngine;
using UnityEngine.AI;

public class Npcs_animation_script : MonoBehaviour
{
    public Animator anim;
    private NavMeshAgent agent;

    [SerializeField] float velocidadUmbral = 0.1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        ActualizarAnimacionMovimiento();
    }

    private void ActualizarAnimacionMovimiento()
    {
        // Velocidad REAL del NavMeshAgent
        float velocidad = agent.velocity.magnitude;


        if (velocidad > velocidadUmbral)
            anim.SetFloat("Movimiento", 1f);
        else
            anim.SetFloat("Movimiento", 0f);
    }
}
