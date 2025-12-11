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
        // Activo mientras se mantiene pulsado Tab
        if (Input.GetKey(KeyCode.Tab))
        {
            panelToShow.SetActive(true);
        }
        else
        {
            panelToShow.SetActive(false);
        }
    }
}
