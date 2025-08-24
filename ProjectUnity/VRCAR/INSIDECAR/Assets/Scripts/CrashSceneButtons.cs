using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrashSceneReturn : MonoBehaviour
{
    [Header("Settings")]
    public float buttonDelayTime = 4f;       // Temps avant apparition du bouton
    public string menuSceneName = "MenuScene";
    
    [Header("UI")]
    public Button returnButton;              // Le bouton à créer
    
    void Start()
    {
        // Cacher le bouton au début
        if (returnButton != null)
        {
            returnButton.gameObject.SetActive(false);
            returnButton.onClick.AddListener(GoBackToMenu);
        }
        
        // Programmer l'apparition du bouton
        Invoke("ShowReturnButton", buttonDelayTime);
    }
    
    void ShowReturnButton()
    {
        if (returnButton != null)
        {
            returnButton.gameObject.SetActive(true);
        }
    }
    
    public void GoBackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
