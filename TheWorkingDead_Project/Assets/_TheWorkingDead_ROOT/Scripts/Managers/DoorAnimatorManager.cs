using UnityEngine;

public class DoorAnimatorManager : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator=GetComponent<Animator>();
    }
    public void opendoor()
    { animator.Play("Anim_door_open"); }
}
