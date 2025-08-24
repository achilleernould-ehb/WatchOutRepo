using UnityEngine;
using UnityEngine.SceneManagement;

public class PassengerNurseController : MonoBehaviour
{
    [Header("VR Head Tracking")]
    public Transform headTransform;         // CenterEyeAnchor (comme vos autres scripts)
    
    [Header("Nurse Setup")]
    public AudioSource nurseVoice;          // AudioSource séparé dans la hiérarchie (son 3D)
    public AudioClip[] distractionPhrases; // Les phrases "je saigne", etc.
    
    [Header("Danger Spawn - MÊME SYSTÈME QUE VOS AUTRES SCÈNES")]
    public GameObject dangerPrefab;         // SportyGranny (même prefab que GPS/Phone)
    public Transform spawnPoint;            // Votre spawnpoint existant
    public float dangerSpeed = -10f;        // Vitesse (même que vos autres scripts)
    public AudioSource screamSound;         // Son de cri (comme GPS)
    
    [Header("Detection Settings - LOGIQUE DROITE COMME HeadRotationTrigger")]
    public float triggerAngle = 45f;        // Angle pour regarder vers la droite (comme HeadRotationTrigger)
    public float lookDuration = 1f;         // Temps à regarder (comme vos autres)
    
    [Header("Timing - MÊME PATTERN QUE VOS SCRIPTS")]
    public float startDelay = 6f;           // Délai avant que l'infirmier parle
    public float maxWaitTime = 15f;         // Temps max avant déclenchement forcé
    
    // Variables privées - MÊME STYLE QUE VOS SCRIPTS
    private float lookTimer = 0f;
    private bool hasTriggered = false;
    private bool nurseStartedTalking = false;
    private float startTime;

    void Start()
    {
        // Trouver la tête automatiquement (MÊME CODE QUE VOS AUTRES SCRIPTS)
        if (headTransform == null)
        {
            OVRCameraRig cameraRig = FindFirstObjectByType<OVRCameraRig>();
            if (cameraRig != null)
            {
                headTransform = cameraRig.centerEyeAnchor;
            }
        }
        
        // DÉBOGAGE DU DÉLAI
        Debug.Log($"=== SCENE STARTED - Nurse will talk in {startDelay} seconds ===");
        Debug.Log($"AudioSource assigned: {nurseVoice != null}");
        Debug.Log($"Number of phrases: {distractionPhrases.Length}");
        
        // Programmer le début de la distraction
        Invoke("StartNurseDistraction", startDelay);
        startTime = Time.time;
    }

    void Update()
    {
        // SEULEMENT vérifier le regard APRÈS que l'infirmier ait commencé à parler
        if (nurseStartedTalking && !hasTriggered)
        {
            CheckLookingRight();
            // SUPPRIMÉ: CheckTimeOut(); - Plus de timeout forcé
        }
    }
    
    void StartNurseDistraction()
    {
        Debug.Log($"=== StartNurseDistraction CALLED at time {Time.time} ===");
        
        if (nurseStartedTalking) 
        {
            Debug.Log("Already started talking - returning");
            return;
        }
        
        nurseStartedTalking = true;
        
        Debug.Log("=== NURSE STARTS TALKING NOW ===");
        
        // Jouer une phrase de distraction
        PlayDistractionPhrase();
        
        Debug.Log("Nurse started talking - player should look RIGHT (passenger seat)");
        Debug.Log("Detection is now ACTIVE - look right to trigger danger!");
    }
    
    void CheckLookingRight()
    {
        if (headTransform == null) return;
        
        // MÊME LOGIQUE QUE HeadRotationTrigger - Détecter rotation Y vers la droite
        float yRotation = headTransform.eulerAngles.y;
        float yaw = Mathf.DeltaAngle(0, yRotation);
        
        // Vérifier si on regarde vers la droite (angle positif)
        if (Mathf.Abs(yaw) > triggerAngle && yaw > 0) // yaw > 0 = droite
        {
            lookTimer += Time.deltaTime;
            
            Debug.Log($"Looking RIGHT - Yaw: {yaw:F1}°, Timer: {lookTimer:F1}s");
            
            if (lookTimer >= lookDuration)
            {
                TriggerDanger();
            }
        }
        else
        {
            // Reset timer si on regarde ailleurs
            lookTimer = 0f;
        }
    }
    
    void CheckTimeOut()
    {
        // Timeout seulement APRÈS que l'infirmier ait commencé à parler
        if (nurseStartedTalking && Time.time - startTime > maxWaitTime)
        {
            Debug.Log("Timeout - triggering danger anyway");
            TriggerDanger();
        }
    }
    
    void PlayDistractionPhrase()
    {
        if (distractionPhrases.Length > 0 && nurseVoice != null)
        {
            // Choisir une phrase aléatoire
            int randomIndex = Random.Range(0, distractionPhrases.Length);
            nurseVoice.PlayOneShot(distractionPhrases[randomIndex]);
            
            Debug.Log($"Playing distraction phrase: {randomIndex + 1}");
        }
        else
        {
            Debug.LogWarning("No distraction phrases assigned or no AudioSource assigned!");
        }
    }
    
    void TriggerDanger()
    {
        if (hasTriggered) return;
        
        hasTriggered = true;
        
        Debug.Log("PASSENGER DISTRACTION TRIGGERED - Spawning danger!");
        
        // Jouer le cri (MÊME CODE QUE SimpleGPSController)
        if (screamSound != null)
        { 
            screamSound.Play();
        }
        
        // Faire apparaître la grand-mère (EXACTEMENT COMME VOS AUTRES SCRIPTS)
        if (dangerPrefab != null && spawnPoint != null)
        {
            GameObject victim = Instantiate(dangerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Ajouter le mouvement (MÊME CODE QUE SimpleGPSController)
            DangerCube dangerScript = victim.GetComponent<DangerCube>();
            if (dangerScript == null)
            {
                dangerScript = victim.AddComponent<DangerCube>();
            }
            dangerScript.speed = dangerSpeed;
            
            Debug.Log("SportyGranny spawned and moving towards car");
        }
    }
    
    // MÉTHODES DE TEST (comme dans vos autres scripts)
    [ContextMenu("Test Nurse Talk")]
    public void TestNurseTalk()
    {
        if (!nurseStartedTalking)
        {
            StartNurseDistraction();
        }
    }
    
    [ContextMenu("Force Trigger Danger")]
    public void ForceTriggerDanger()
    {
        if (!hasTriggered)
        {
            TriggerDanger();
        }
    }
}