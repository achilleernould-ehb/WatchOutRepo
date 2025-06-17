using UnityEngine;

public class HeadRotationTriggerTimed : MonoBehaviour
{
    public Transform headTransform;         // La tête du joueur (ex: CenterEyeAnchor)
    public GameObject dangerCubePrefab;     // Le prefab du cube
    public Transform spawnPoint;            // Point d'apparition du cube
    public float triggerAngle = 45f;        // Angle à partir duquel on déclenche
    public float speed = -10f;               // Vitesse du cube
    public float lookDuration = 1f;         // Temps minimum à regarder vers la droite

    private float lookTimer = 0f;
    private bool hasTriggered = false;

    void Update()
    {
        float yRotation = headTransform.eulerAngles.y;
        float yaw = Mathf.DeltaAngle(0, yRotation);

        // Check si on regarde bien vers la droite
        if (Mathf.Abs(yaw) > triggerAngle)
        {
            lookTimer += Time.deltaTime;

            if (!hasTriggered && lookTimer >= lookDuration)
            {
                hasTriggered = true;
                TriggerEvent();
            }
        }
        else
        {
            // Reset le timer si le joueur regarde à nouveau devant ou à gauche
            lookTimer = 0f;
        }
    }

    void TriggerEvent()
    {
        GameObject cube = Instantiate(dangerCubePrefab, spawnPoint.position, spawnPoint.rotation);
        DangerCube cubeScript = cube.AddComponent<DangerCube>();
        cubeScript.speed = speed;
    }
}