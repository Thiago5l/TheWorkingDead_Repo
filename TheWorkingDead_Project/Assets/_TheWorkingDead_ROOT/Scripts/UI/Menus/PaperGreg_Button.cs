using UnityEngine;
using System.Collections; 

public class PaperGreg_Button : MonoBehaviour
{
    public Animator animator;
    public GameObject grr;
    public void onclick()
    {
        Debug.Log("greg clicado");
        StartCoroutine(ControlAnimaciones());
        StartCoroutine(grrr());
    }

    private IEnumerator ControlAnimaciones()
    {
        animator.SetFloat("grr", 1f);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("grr", 0f);
    }
    private IEnumerator grrr()
    {
        grr.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        grr.SetActive(false);

    }
}
