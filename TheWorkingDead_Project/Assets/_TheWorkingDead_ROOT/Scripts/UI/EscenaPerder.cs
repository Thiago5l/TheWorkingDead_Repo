using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaPerder : MonoBehaviour
{

   // [SerializeField] public GameObject imagen;
    //[SerializeField] public GameObject video;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void CambiarImage()
    {
      //  video.SetActive(false);
     //   imagen.SetActive(true);
    }

    public void IrScenaMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void Salir()
    {
        Application.Quit();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.anyKey)
    //    {
    //        SceneManager.LoadScene("SCN_MainMenu_Test");
    //    }
    //}
}
