using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FatigueVREyelids : MonoBehaviour
{
    [Header("VR Tracking")]
    public Transform cameraRig;             // Le Camera Rig VR
    public Transform centerEyeAnchor;       // CenterEyeAnchor pour suivre la tête
    
    [Header("Eyelids UI")]
    public Image topEyelid;          // Image noire du haut
    public Image bottomEyelid;       // Image noire du bas
    public Canvas eyelidsCanvas;     // Canvas pour les paupières
    
    [Header("Timing Settings")]
    public float startDelay = 8f;              // Délai avant premiers clignements
    public float soundDelayAfterCrash = 3f;    // Temps pour entendre le son avant changement de scène
    
    [Header("Eyelid Animation")]
    public float maxEyelidHeight = 0.5f;       // Hauteur max paupière (0.5 = moitié écran)
    
    [Header("VR Distance Settings")]
    public float canvasDistance = 0.1f;        // Distance du canvas (10cm par défaut)
    public float canvasScale = 0.001f;         // Échelle du canvas
    
    [Header("Crash Settings")]
    public AudioSource crashSound;             // Son de crash de voiture
    public string crashSceneName = "CrashScene"; // Scène de fin
    
    [Header("Progression")]
    public int totalBlinksBeforeSleep = 5;     // Nombre de clignements avant fermeture finale
    
    private int currentBlinkCount = 0;
    private bool isBlinking = false;
    private bool finalCloseStarted = false;
    private bool crashTriggered = false;
    private RectTransform topRect;
    private RectTransform bottomRect;
    private float screenHeight;

    void Start()
    {
        SetupEyelidsForVR();
        
        // Commencer la séquence de fatigue
        Invoke("StartFatigueSequence", startDelay);
        
        Debug.Log("Fatigue sequence will start in " + startDelay + " seconds");
    }
    
    void SetupEyelidsForVR()
    {
        // Trouver automatiquement le Camera Rig si pas assigné
        if (cameraRig == null)
        {
            OVRCameraRig ovrRig = FindFirstObjectByType<OVRCameraRig>();
            if (ovrRig != null)
            {
                cameraRig = ovrRig.transform;
                centerEyeAnchor = ovrRig.centerEyeAnchor;
            }
        }
        
        if (eyelidsCanvas != null && centerEyeAnchor != null)
        {
            // ATTACHER le Canvas au CenterEyeAnchor pour qu'il suive la tête
            eyelidsCanvas.transform.SetParent(centerEyeAnchor, false);
            
            // Configuration du Canvas
            eyelidsCanvas.renderMode = RenderMode.WorldSpace;
            eyelidsCanvas.sortingOrder = 1000;
            
            // Position DEVANT les yeux - AJUSTEMENT PRINCIPAL
            eyelidsCanvas.transform.localPosition = new Vector3(0, 0, canvasDistance); // 10cm devant par défaut
            eyelidsCanvas.transform.localRotation = Quaternion.identity;
            eyelidsCanvas.transform.localScale = new Vector3(canvasScale, canvasScale, canvasScale); // Ajustable
            
            // Taille du Canvas pour couvrir complètement la vision
            RectTransform canvasRect = eyelidsCanvas.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(2000, 1200); // Taille adaptée à la distance
            
            screenHeight = 1200; // Hauteur de référence
            
            Debug.Log($"Eyelids Canvas positioned at {canvasDistance}m from eyes with scale {canvasScale}");
        }
        else
        {
            Debug.LogError("Cannot find CenterEyeAnchor! Make sure you have OVRCameraRig in scene.");
        }
        
        // Configuration paupière du haut
        if (topEyelid != null)
        {
            topEyelid.color = Color.black;
            topRect = topEyelid.GetComponent<RectTransform>();
            
            // Ancrer en haut de l'écran
            topRect.anchorMin = new Vector2(0, 1);
            topRect.anchorMax = new Vector2(1, 1);
            topRect.pivot = new Vector2(0.5f, 1);
            topRect.anchoredPosition = Vector2.zero;
            
            // Commencer avec hauteur 0 (invisible)
            topRect.sizeDelta = new Vector2(2000, 0); // Largeur adaptée
        }
        
        // Configuration paupière du bas
        if (bottomEyelid != null)
        {
            bottomEyelid.color = Color.black;
            bottomRect = bottomEyelid.GetComponent<RectTransform>();
            
            // Ancrer en bas de l'écran
            bottomRect.anchorMin = new Vector2(0, 0);
            bottomRect.anchorMax = new Vector2(1, 0);
            bottomRect.pivot = new Vector2(0.5f, 0);
            bottomRect.anchoredPosition = Vector2.zero;
            
            // Commencer avec hauteur 0 (invisible)
            bottomRect.sizeDelta = new Vector2(2000, 0); // Largeur adaptée
        }
        
        Debug.Log("Eyelids setup complete for VR");
    }
    
    void StartFatigueSequence()
    {
        Debug.Log("Starting fatigue sequence - first blinks coming!");
        
        if (!isBlinking && !finalCloseStarted)
        {
            StartCoroutine(BlinkSequence());
        }
    }
    
    IEnumerator BlinkSequence()
    {
        isBlinking = true;
        
        // PARAMÈTRES CODÉS EN DUR (plus modifiables dans l'inspecteur)
        float baseBlinkDuration = 0.8f;        // Durée de base : 0.8s
        float constantPause = 3f;              // Pause CONSTANTE entre clignements (pas de progression)
        float finalCloseDuration = 4f;         // Fermeture finale LENTE comme le dernier clignement
        
        // Série de clignements progressifs
        for (int i = 0; i < totalBlinksBeforeSleep; i++)
        {
            currentBlinkCount++;
            
            Debug.Log($"Blink {currentBlinkCount}/{totalBlinksBeforeSleep} - FULL CLOSE");
            
            // CLIGNEMENTS TRÈS LENTS DÈS LE DÉBUT : 0.8s puis augmentation drastique
            float progressionFactor = Mathf.Pow((float)currentBlinkCount / totalBlinksBeforeSleep, 2.5f);
            float blinkDurationCurrent = baseBlinkDuration * (1f + progressionFactor * 5f);
            
            Debug.Log($"Blink duration: {blinkDurationCurrent:F2}s");
            
            // Fermeture complète à chaque fois
            yield return StartCoroutine(SingleBlink(blinkDurationCurrent));
            
            // Pause CONSTANTE entre clignements (plus de progression)
            Debug.Log($"Constant pause: {constantPause}s");
            yield return new WaitForSeconds(constantPause);
        }
        
        // Fermeture finale (endormissement) - AUSSI LENTE que les derniers clignements
        Debug.Log("Starting final sleep phase - SLOW close");
        yield return StartCoroutine(FinalEyeClose(finalCloseDuration));
        
        isBlinking = false;
    }
    
    IEnumerator SingleBlink(float duration)
    {
        float elapsed = 0f;
        // CHANGEMENT : Les paupières vont TOUJOURS jusqu'au bout (se touchent complètement)
        float maxHeight = screenHeight * maxEyelidHeight;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // Courbe de clignement : monte puis descend
            float blinkProgress = Mathf.Sin(progress * Mathf.PI);
            float currentHeight = blinkProgress * maxHeight;
            
            SetEyelidHeight(currentHeight);
            
            yield return null;
        }
        
        // S'assurer que les yeux sont ouverts
        SetEyelidHeight(0);
    }
    
    IEnumerator FinalEyeClose(float closeDuration)
    {
        finalCloseStarted = true;
        
        // Fermeture lente et complète
        float elapsed = 0f;
        float maxHeight = screenHeight * maxEyelidHeight;
        
        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / closeDuration;
            
            // Fermeture progressive jusqu'à la moitié de l'écran
            float currentHeight = Mathf.Lerp(0, maxHeight, progress);
            
            SetEyelidHeight(currentHeight);
            
            yield return null;
        }
        
        // Yeux complètement fermés
        SetEyelidHeight(maxHeight);
        
        Debug.Log("Eyes closed - IMMEDIATE CRASH SOUND!");
        
        // JOUER LE SON IMMÉDIATEMENT quand les yeux se ferment
        if (crashSound != null)
        {
            crashSound.Play();
            Debug.Log("Crash sound playing now");
        }
        
        // Attendre le temps configuré dans l'inspecteur
        Debug.Log($"Waiting {soundDelayAfterCrash} seconds for sound to finish...");
        yield return new WaitForSeconds(soundDelayAfterCrash);
        
        // Crash après avoir entendu le son
        TriggerCrash();
    }
    
    void SetEyelidHeight(float height)
    {
        if (topRect != null)
        {
            topRect.sizeDelta = new Vector2(2000, height);
        }
        
        if (bottomRect != null)
        {
            bottomRect.sizeDelta = new Vector2(2000, height);
        }
    }
    
    void TriggerCrash()
    {
        if (crashTriggered) return;
        
        crashTriggered = true;
        Debug.Log("CRASH! - Changing to crash scene (sound already played)");
        
        // LE SON A DÉJÀ ÉTÉ JOUÉ dans FinalEyeClose()
        // Juste changer de scène maintenant
        StartCoroutine(LoadCrashScene());
    }
    
    IEnumerator LoadCrashScene()
    {
        // Pas de délai supplémentaire - le son a eu le temps de jouer
        yield return null; // Juste un frame pour éviter les conflits
        
        // Charger la scène de crash
        SceneManager.LoadScene(crashSceneName);
    }
    
    // Méthodes de test et d'ajustement
    [ContextMenu("Test Single Blink")]
    public void TestBlink()
    {
        if (!isBlinking)
        {
            StartCoroutine(SingleBlink(1f));
        }
    }
    
    [ContextMenu("Test Final Close")]
    public void TestFinalClose()
    {
        if (!finalCloseStarted)
        {
            StartCoroutine(FinalEyeClose(2f)); // Ajouter le paramètre closeDuration
        }
    }
    
    [ContextMenu("Test Eyelid Height")]
    public void TestEyelidHeight()
    {
        SetEyelidHeight(screenHeight * 0.3f);
    }
    
    [ContextMenu("Adjust Canvas Distance")]
    public void AdjustCanvasDistance()
    {
        if (eyelidsCanvas != null)
        {
            eyelidsCanvas.transform.localPosition = new Vector3(0, 0, canvasDistance);
            Debug.Log($"Canvas distance adjusted to {canvasDistance}m");
        }
    }
}