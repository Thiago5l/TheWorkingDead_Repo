using UnityEngine;

public class ShowWithTab : MonoBehaviour
{
    public GameObject panelToShow;

    void Start()
    {
        panelToShow.SetActive(false);
    }

    void Update()
    {
        while (Input.GetKeyDown(KeyCode.Tab))
        {
            panelToShow.SetActive(!panelToShow.activeSelf);
        }
    }
}
