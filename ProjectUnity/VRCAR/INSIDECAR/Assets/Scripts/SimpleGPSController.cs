using UnityEngine;

public class SimpleGPSController : MonoBehaviour
{
    [Header("GPS Setup")]
    public Transform gpsDevice;         // Ton modèle GPS
    public Transform headTransform;     // CenterEyeAnchor
    
    [Header("Positions - AJUSTABLES DANS L'INSPECTOR")]
    public Vector3 startPosition = new Vector3(0.288f, 1.148f, 0.613f);     // Position initiale
    public Vector3 bouncePosition = new Vector3(0.3f, 0.8f, 0.4f);          // Où ça rebondit - MODIFIABLE!
    public Vector3 endPosition = new Vector3(0.4f, -0.2f, 0.3f);            // Position finale au sol
    
    [Header("Animation Settings")]
    public float dropDuration = 3f;        // Durée totale
    public float bounceHeight = 0.2f;      // Hauteur du rebond - MODIFIABLE!
    public float bouncePoint = 0.5f;       // Moment du rebond (0.5 = milieu, 0.3 = plus tôt, 0.7 = plus tard)
    public float fallAcceleration = 2f;    // Accélération chute finale (1 = normal, 2 = 2x plus rapide)
    
    [Header("Danger Setup")]
    public GameObject dangerPrefab;     // SportyGranny
    public Transform spawnPoint;        // Où elle apparaît
    public float dangerSpeed = -10f;
    public AudioSource screamSound;
    
    [Header("Timing & Detection")]
    public float dropDelay = 6f;        // Quand le GPS tombe
    public float lookAngle = 60f;       // Angle de détection
    public float lookTime = 1f;         // Temps à regarder
    
    [Header("Sound")]
    public AudioSource dropSound;
    
    // Variables privées
    private bool gpsDropped = false;
    private bool dangerTriggered = false;
    private float lookTimer = 0f;
    
    void Start()
    {
        // Utiliser la position actuelle du GPS comme point de départ
        if (gpsDevice != null)
        {
            startPosition = gpsDevice.position;
        }
        
        // Trouver la tête automatiquement si pas assignée
        if (headTransform == null)
        {
            OVRCameraRig cameraRig = FindFirstObjectByType<OVRCameraRig>();
            if (cameraRig != null)
            {
                headTransform = cameraRig.centerEyeAnchor;
            }
        }
        
        // Programmer la chute du GPS
        Invoke("DropGPS", dropDelay);
    }
    
    void Update()
    {
        // Vérifier si on regarde vers le GPS tombé
        if (gpsDropped && !dangerTriggered)
        {
            CheckLookingDown();
        }
    }
    
    void DropGPS()
    {
        if (gpsDevice == null) return;
        
        // Jouer le son de chute
        if (dropSound != null)
        {
            dropSound.Play();
        }
        
        // Animer la chute avec rebond personnalisé
        StartCoroutine(AnimateGPSFallWithCustomBounce());
        
        gpsDropped = true;
    }
    
    System.Collections.IEnumerator AnimateGPSFallWithCustomBounce()
    {
        // UTILISE DIRECTEMENT LES VALEURS DE L'INSPECTOR
        Vector3 start = startPosition;
        Vector3 bouncePos = bouncePosition;
        Vector3 end = endPosition;
        
        float totalTime = dropDuration;
        float elapsed = 0f;
        
        while (elapsed < totalTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / totalTime;
            
            Vector3 currentPosition;
            
            if (progress < bouncePoint)
            {
                // PREMIÈRE PARTIE : Chute vers le point de rebond (vitesse normale)
                float phase1Progress = progress / bouncePoint; // 0 à 1
                currentPosition = Vector3.Lerp(start, bouncePos, phase1Progress);
            }
            else
            {
                // DEUXIÈME PARTIE : Du point de rebond vers la fin (avec accélération)
                float phase2Progress = (progress - bouncePoint) / (1f - bouncePoint); // 0 à 1
                
                // Appliquer l'accélération à la chute finale
                float acceleratedProgress = Mathf.Pow(phase2Progress, fallAcceleration);
                currentPosition = Vector3.Lerp(bouncePos, end, acceleratedProgress);
            }
            
            // AJOUTER LE REBOND comme une courbe par-dessus la trajectoire
            float bounceEffect = 0f;
            
            // Le rebond se produit autour du point défini
            float bounceWindow = 0.1f; // Largeur de la fenêtre de rebond
            float bounceStart = bouncePoint - bounceWindow;
            float bounceEnd = bouncePoint + bounceWindow;
            
            if (progress > bounceStart && progress < bounceEnd)
            {
                float bounceProgress = (progress - bounceStart) / (bounceWindow * 2); // 0 à 1 pendant le rebond
                bounceEffect = Mathf.Sin(bounceProgress * Mathf.PI) * bounceHeight;
            }
            
            // Position finale = trajectoire normale + effet de rebond
            gpsDevice.position = currentPosition + Vector3.up * bounceEffect;
            
            // Rotation continue sur plusieurs axes pour plus de réalisme
            float rotationSpeed = progress > bouncePoint ? 180f : 120f;
            
            // Rotation sur Z (comme avant)
            gpsDevice.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
            
            // Rotation sur X pour effet de tumbling réaliste
            gpsDevice.Rotate(Vector3.right, Time.deltaTime * rotationSpeed * 0.7f);
            
            // Rotation sur Y pour rotation complète dans l'espace
            gpsDevice.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * 0.4f);
            
            yield return null;
        }
        
        // S'assurer de la position finale exacte
        gpsDevice.position = end;
    }
    
    void CheckLookingDown()
    {
        if (headTransform == null) return;
        
        // Calculer la direction vers la position où le GPS est tombé
        Vector3 directionToGPS = (endPosition - headTransform.position).normalized;
        Vector3 headDirection = headTransform.forward;
        
        // Calculer l'angle entre la direction de la tête et le GPS au sol
        float angle = Vector3.Angle(headDirection, directionToGPS);
        
        // Si on regarde vers le GPS au sol
        if (angle < lookAngle)
        {
            lookTimer += Time.deltaTime;
            
            if (lookTimer >= lookTime)
            {
                TriggerDanger();
            }
        }
        else
        {
            lookTimer = 0f;
        }
    }
    
    void TriggerDanger()
    {
        if (dangerTriggered) return;
        
        dangerTriggered = true;
        
        // Jouer le cri
        if (screamSound != null)
        {
            screamSound.Play();
        }
        
        // Faire apparaître la grand-mère
        if (dangerPrefab != null && spawnPoint != null)
        {
            GameObject victim = Instantiate(dangerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Ajouter le mouvement
            DangerCube dangerScript = victim.GetComponent<DangerCube>();
            if (dangerScript == null)
            {
                dangerScript = victim.AddComponent<DangerCube>();
            }
            dangerScript.speed = dangerSpeed;
        }
    }
}