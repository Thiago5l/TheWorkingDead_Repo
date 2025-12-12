using UnityEngine;

public class TutorialTab : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Destroy(this.gameObject);
        }
    }
}
