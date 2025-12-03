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
    public void noLongerPaused()
    {
        paperanim.SetBool("reversePause", true);
        StartCoroutine(animReversePauseDone());
    }
    IEnumerator animPauseDone()
    {
        yield return new WaitForSecondsRealtime(1);
        paperanim.SetBool("isPaused", false);
        
    }
    IEnumerator animReversePauseDone()
    {
        yield return new WaitForSecondsRealtime(1);
        paperanim.SetBool("reversePause", false);

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
    public void onReverseFliped()
    {
        paperanim.SetBool("isReverseFliped", true);
        StartCoroutine(animReverseFlipDone());
    }
    IEnumerator animReverseFlipDone()
    {
        yield return new WaitForSecondsRealtime(1f);
        paperanim.SetBool("isReverseFliped", false);

    }
}
