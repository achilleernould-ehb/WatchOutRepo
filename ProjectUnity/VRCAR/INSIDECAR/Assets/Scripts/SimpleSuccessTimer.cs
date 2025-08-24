using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSuccessTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float successTimeInSeconds = 20f;    // Temps pour réussir le level
    
    [Header("Scene Reference - GLISSEZ VOTRE SCÈNE ICI")]
    public Object successScene;                 // Glissez directement la scène depuis le Project
    public string fallbackSuccessSceneName = "SuccessScene"; // Nom de backup si pas de référence
    
    [Header("Collision Detection")]
    public bool enableCollisionDetection = true; // Activer détection collision
    
    [Header("Debug")]
    public bool showDebugMessages = true;       // Afficher les messages de debug
    public bool showCountdown = false;          // Afficher décompte chaque 5s
    
    // Variables privées
    private bool timerCancelled = false;
    private float timeRemaining;

    void Start()
    {
        timeRemaining = successTimeInSeconds;
        
        if (showDebugMessages)
        {
            Debug.Log($"⏰ Success timer started - {successTimeInSeconds} seconds to success");
            
            // Debug de la scène
            if (successScene != null)
            {
                Debug.Log($"Success scene reference: {successScene.name}");
            }
            else
            {
                Debug.Log($"Using fallback scene name: {fallbackSuccessSceneName}");
            }
        }
        
        // Programmer le succès après le délai
        Invoke("TriggerSuccess", successTimeInSeconds);
    }
    
    void Update()
    {
        if (timerCancelled) return;
        
        // Décompter pour le debug
        timeRemaining -= Time.deltaTime;
        
        // Afficher le décompte si activé
        if (showCountdown && timeRemaining > 0)
        {
            int secondsLeft = Mathf.CeilToInt(timeRemaining);
            if (secondsLeft % 5 == 0 && secondsLeft != Mathf.CeilToInt(timeRemaining + Time.deltaTime))
            {
                Debug.Log($"⏰ {secondsLeft} seconds remaining...");
            }
        }
    }
    
    void TriggerSuccess()
    {
        if (timerCancelled) return;
        
        if (showDebugMessages)
        {
            Debug.Log("🎉 SUCCESS! No distraction occurred - loading success scene");
        }
        
        // Utiliser la référence de scène si disponible, sinon le nom
        string sceneToLoad = "";
        
        if (successScene != null)
        {
            sceneToLoad = successScene.name;
            Debug.Log($"Loading scene by reference: {sceneToLoad}");
        }
        else
        {
            sceneToLoad = fallbackSuccessSceneName;
            Debug.Log($"Loading scene by name: {sceneToLoad}");
        }
        
        // Vérifier que la scène existe dans Build Settings
        if (IsSceneInBuildSettings(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"Scene '{sceneToLoad}' not found in Build Settings! Add it to File > Build Settings");
        }
    }
    
    bool IsSceneInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    
    // MÉTHODE PUBLIQUE pour annuler le timer
    public void CancelTimer()
    {
        if (timerCancelled) return;
        
        timerCancelled = true;
        CancelInvoke("TriggerSuccess");
        
        if (showDebugMessages)
        {
            Debug.Log("❌ Success timer cancelled - accident will occur");
        }
    }
    
    // Détecter collision avec la voiture (SportyGranny touche voiture = échec)
    void OnTriggerEnter(Collider other)
    {
        if (!enableCollisionDetection) return;
        
        if (other.CompareTag("Car"))
        {
            if (showDebugMessages)
            {
                Debug.Log("🚗 Car collision detected - cancelling success timer");
            }
            CancelTimer();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!enableCollisionDetection) return;
        
        if (collision.gameObject.CompareTag("Car"))
        {
            if (showDebugMessages)
            {
                Debug.Log("🚗 Car collision detected - cancelling success timer");
            }
            CancelTimer();
        }
    }
    
    // MÉTHODES DE TEST pour l'inspecteur
    [ContextMenu("Test Success Now")]
    public void TestSuccess()
    {
        TriggerSuccess();
    }
    
    [ContextMenu("Test Cancel Timer")]
    public void TestCancel()
    {
        CancelTimer();
    }
    
    [ContextMenu("Check Scene In Build")]
    public void CheckSceneInBuild()
    {
        string sceneToCheck = successScene != null ? successScene.name : fallbackSuccessSceneName;
        bool exists = IsSceneInBuildSettings(sceneToCheck);
        Debug.Log($"Scene '{sceneToCheck}' in Build Settings: {exists}");
    }
}