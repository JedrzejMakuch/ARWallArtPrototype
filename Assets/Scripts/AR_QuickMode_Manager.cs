using UnityEngine;
using TMPro;

public class AR_QuickMode_Manager : MonoBehaviour
{
    [Header("Ustawienia Obiektu")]
    [SerializeField] private GameObject objectToPlace;

    [Header("Ustawienia Dystansu")]
    [SerializeField] private float currentDistance = 2.0f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 5.0f;

    [Header("Ustawienia Płynności")]
    [SerializeField] private float smoothingSpeed = 8.0f;

    [Header("Elementy UI (TextMeshPro)")]
    [SerializeField] private TMP_Text buttonText;       
    [SerializeField] private TMP_Text distanceValueText; 

    private GameObject spawnedImage;
    private Transform cameraTransform;
    private bool isLocked = false;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("[Quick Mode] Brak Main Camera!");
            return;
        }

        if (objectToPlace != null)
        {
            spawnedImage = Instantiate(objectToPlace);
        }

        UpdateUpdateButtonText();
        UpdateDistanceText(); 
    }

    private void Update()
    {
        if (isLocked || spawnedImage == null || cameraTransform == null)
            return;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 targetPosition = cameraTransform.position + (cameraForward * currentDistance);
        targetPosition.y = cameraTransform.position.y;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);

        spawnedImage.transform.position = Vector3.Lerp(spawnedImage.transform.position, targetPosition, Time.deltaTime * smoothingSpeed);
        spawnedImage.transform.rotation = Quaternion.Slerp(spawnedImage.transform.rotation, targetRotation, Time.deltaTime * smoothingSpeed);
    }

    public void SetDistance(float newDistance)
    {
        currentDistance = Mathf.Clamp(newDistance, minDistance, maxDistance);

        UpdateDistanceText();
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;
        UpdateUpdateButtonText();
    }

    private void UpdateUpdateButtonText()
    {
        if (buttonText == null) return;
        buttonText.text = isLocked ? "ODKLEJ OBRAZ" : "PRZYKLEJ OBRAZ";
    }

    private void UpdateDistanceText()
    {
        if (distanceValueText == null) return;
        distanceValueText.text = $"Dystans do ściany: {currentDistance:F1} m";
    }

    public GameObject GetSpawnedImage()
    {
        return spawnedImage;
    }
}