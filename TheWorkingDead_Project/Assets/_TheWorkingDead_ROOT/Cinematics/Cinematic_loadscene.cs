using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic_loadscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.musicSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadsceneMainMenu()
    { SceneManager.LoadScene("SCN_MainMenu_Test"); AudioManager.Instance.musicSource.Play(); }
    public void loadsceneGame()
    { AudioManager.Instance.musicSource.Play(); SceneManager.LoadScene("SCN_Tutorial");  }
}
