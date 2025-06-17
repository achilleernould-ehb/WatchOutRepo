using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName = "SampleScene";

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}