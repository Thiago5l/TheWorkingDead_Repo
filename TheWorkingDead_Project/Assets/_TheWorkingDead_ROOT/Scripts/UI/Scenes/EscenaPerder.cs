using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaPerder : MonoBehaviour
{
    void Start()
    {
    }

    public void CambiarImage()
    {
        // video.SetActive(false);
        // imagen.SetActive(true);
    }

    public void IrScenaMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir()
    {
        SceneManager.LoadScene("SCN_Main_Menu");
    }

    //void Update()
    //{
    //    if (Input.anyKey)
    //    {
    //        SceneManager.LoadScene("SCN_MainMenu_Test");
    //    }
    //}
}
