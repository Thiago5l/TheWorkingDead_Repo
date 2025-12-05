using UnityEngine;
using System.Collections;

public class AnimacionAleatoria : MonoBehaviour
{
    public Animator animator;

    public float tiempoMin = 3f;
    public float tiempoMax = 8f;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ControlAnimaciones());
    }

    private IEnumerator ControlAnimaciones()
    {
        while (true)
        {
            // Espera aleatoria
            float espera = Random.Range(tiempoMin, tiempoMax);
            yield return new WaitForSeconds(espera);

            int anim = Random.Range(0, 2);

            if (anim == 0)
            {
                animator.SetFloat("rascar", 1f);
                animator.SetFloat("rascarmano", 0f);
            }
            else
            {
                animator.SetFloat("rascarmano", 1f);
                animator.SetFloat("rascar", 0f);
            }

            // Espera 1 segundo y resetea los parametros
            yield return new WaitForSeconds(1f);

            animator.SetFloat("rascar", 0f);
            animator.SetFloat("rascarmano", 0f);

            // Espera a que salga de Idle
            yield return new WaitUntil(() => !AnimatorEstaEnEstado("anim_Npc_IdleAna"));

            // Espera a que vuelva a Idle
            yield return new WaitUntil(() => AnimatorEstaEnEstado("anim_Npc_IdleAna"));
        }
    }

    private bool AnimatorEstaEnEstado(string nombreEstado)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(nombreEstado);
    }
}
