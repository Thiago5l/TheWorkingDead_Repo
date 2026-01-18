using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic_loadscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadsceneMainMenu()
    { SceneManager.LoadScene("SCN_MainMenu_Test"); }
    public void loadsceneGame()
    { SceneManager.LoadScene("SCN_Tutorial"); }
}
