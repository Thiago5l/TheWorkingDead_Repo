using System.Collections;
using UnityEngine;

public class GregMainMenu : MonoBehaviour
{
    [SerializeField] Transform gregRecortable;
    [SerializeField] int randomNumber;
    private void Update()
    {
        StartCoroutine(GetNumber(1, 6000));
    }
    IEnumerator GetNumber(int min, int max)
    {
        yield return new WaitForSecondsRealtime(1f);
        randomNumber = Random.Range(min, max);
        yield return new WaitForSecondsRealtime(1f);
        GrooveIt();
    }

    public void GrooveIt()
    {
        if (randomNumber == 33)
        {
            gregRecortable.localPosition = new Vector2(0, -Screen.height);
            gregRecortable.LeanMoveLocalY(-195, 2f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(Hide);
        }
    }
    public void Hide()
    {
        gregRecortable.LeanMoveLocalY(-Screen.height, 0.8f).setEaseInExpo().setIgnoreTimeScale(true).delay = 3f;
    }
}
