using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSimple : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
