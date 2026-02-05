using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("SCN_Main_Menu");
    }
}
