using UnityEngine;
using System.Collections;

public class CarriAnimations : MonoBehaviour
{
    public Animator animator;
    public NPCConversationTask NPCConversationTask;

    public float tiempoMin = 3f;
    public float tiempoMax = 8f;

    private Quaternion initialRotation;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        // Guardamos la rotación inicial del GameObject
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (NPCConversationTask.talking)
        {
            animator.SetFloat("Talk", 1f);
        }
        else
        {
            animator.SetFloat("Talk", 0f);

            // Reseteamos la rotación cuando no está hablando
            transform.rotation = initialRotation;
        }
    }
}


