using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("UI References - Glisse tes boutons ici")]
    public Button phoneButton;
    public Button passengerButton;
    public Button gpsButton;
    public Button fatigueButton;
    public Button quitButton;

    [Header("Scene Names")]
    public string phoneSceneName = "SampleScene";
    public string passengerSceneName = "PassengerScene";
    public string gpsSceneName = "GPSScene";
    public string fatigueSceneName = "FatigueScene";
    public string menuSceneName = "MenuScene";

    void Start()
    {
        SetupButtons();
    }

    void SetupButtons()
    {
        // Connecter chaque bouton à sa fonction
        if (phoneButton != null)
        {
            phoneButton.onClick.AddListener(() => LoadPhoneScenario());
        }
        
        if (passengerButton != null)
        {
            passengerButton.onClick.AddListener(() => LoadPassengerScenario());
        }
        
        if (gpsButton != null)
        {
            gpsButton.onClick.AddListener(() => LoadGPSScenario());
        }
        
        if (fatigueButton != null)
        {
            fatigueButton.onClick.AddListener(() => LoadFatigueScenario());
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => QuitGame());
        }

        Debug.Log("Boutons configurés dans SceneLoader");
    }

    // Fonctions pour chaque scénario
    public void LoadPhoneScenario()
    {
        Debug.Log("Chargement scénario téléphone");
        PlayerPrefs.SetString("CurrentScenario", "phone");
        SceneManager.LoadScene(phoneSceneName);
    }

    public void LoadPassengerScenario()
    {
        Debug.Log("Chargement scénario passager");
        PlayerPrefs.SetString("CurrentScenario", "passenger");
        SceneManager.LoadScene(passengerSceneName);
    }

    public void LoadGPSScenario()
    {
        Debug.Log("Chargement scénario GPS");
        PlayerPrefs.SetString("CurrentScenario", "gps");
        SceneManager.LoadScene(gpsSceneName);
    }

    public void LoadFatigueScenario()
    {
        Debug.Log("Chargement scénario fatigue");
        PlayerPrefs.SetString("CurrentScenario", "fatigue");
        SceneManager.LoadScene(fatigueSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture du jeu");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Fonction pour revenir au menu (à utiliser dans les autres scènes)
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    // Garde ta fonction originale pour compatibilité
    public void LoadScene()
    {
        LoadPhoneScenario();
    }
}