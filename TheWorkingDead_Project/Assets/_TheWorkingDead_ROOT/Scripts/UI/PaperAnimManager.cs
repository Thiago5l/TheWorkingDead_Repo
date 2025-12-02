using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PaperAnimManager : MonoBehaviour
{
    //public GameObject paper;
    public Animator paperanim;
    public GameObject pausePanel;

    private void Start()
    {
        paperanim = GetComponent<Animator>();
    }

    public void onPause()
    {
        paperanim.SetBool("isPaused", true);
        StartCoroutine(animPauseDone());
    }
    IEnumerator animPauseDone()
    {
        yield return new WaitForSecondsRealtime(1);
        paperanim.SetBool("isPaused", false);
        
    }
    public void onFliped()
    {
        paperanim.SetBool("isFliped", true);
        StartCoroutine(animFlipDone());
    }
    IEnumerator animFlipDone()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        paperanim.SetBool("isFliped", false);

    }
}
