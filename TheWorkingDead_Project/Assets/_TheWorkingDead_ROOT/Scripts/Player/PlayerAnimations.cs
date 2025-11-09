using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerAnimations : MonoBehaviour
{
    Vector2 moveImput;
    Animator anim;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); // referencia al Animator
    }

    void Update()
    {
        Movimiento();
    }
    void Movimiento()
    {
        if (moveImput.magnitude != 0) { anim.SetFloat("Movimiento", 1f); }
        else { anim.SetFloat("Movimiento", 0f); }
    }
    public void DetectarMovimiento(InputAction.CallbackContext context) //context bontón físico
    {
        moveImput = context.ReadValue<Vector2>();

    }
}
