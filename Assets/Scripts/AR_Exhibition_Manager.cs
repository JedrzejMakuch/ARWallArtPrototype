using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AR_Exhibition_Manager : MonoBehaviour
{
    [Header("Wystawa (Dane z API)")]
    [SerializeField] private List<ArtworkData> exhibitionArtworks = new List<ArtworkData>();

    [Header("Referencje Systemowe")]
    [SerializeField] private AR_QuickMode_Manager quickModeManager;

    [Header("UI Elementy")]
    [SerializeField] private TMP_Text titleText; 

    private int currentArtworkIndex = 0;

    private void Start()
    {
        ApplyArtworkToPrefab();
    }

    public void SetArtworks(List<ArtworkData> artworks)
    {
        exhibitionArtworks = artworks ?? new List<ArtworkData>();
        currentArtworkIndex = 0;

        ApplyArtworkToPrefab();
    }

    public void SelectArtwork(int index)
    {
        if (index < 0 || index >= exhibitionArtworks.Count) return;

        currentArtworkIndex = index;
        ApplyArtworkToPrefab();
    }

    private void ApplyArtworkToPrefab()
    {
        if (exhibitionArtworks.Count == 0) return;

        GameObject spawnedPrefab = quickModeManager.GetSpawnedImage();
        if (spawnedPrefab == null) return;

        ArtworkData currentData = exhibitionArtworks[currentArtworkIndex];

        if (titleText != null)
        {
            int widthCm = Mathf.RoundToInt(currentData.widthInMeters * 100);
            int heightCm = Mathf.RoundToInt(currentData.heightInMeters * 100);

            titleText.text = $"{currentData.title}\n<size=30>{widthCm} x {heightCm} cm</size>";
        }

        Transform canvasQuadTransform = spawnedPrefab.transform.Find("Visuals/Canvas_Quad");

        if (canvasQuadTransform != null)
        {
            Renderer quadRenderer = canvasQuadTransform.GetComponent<Renderer>();
            if (quadRenderer != null && currentData.texture != null)
            {
                quadRenderer.material.mainTexture = currentData.texture;
            }

            canvasQuadTransform.localScale = new Vector3(currentData.widthInMeters, currentData.heightInMeters, 1f);
        }
        else
        {
            Debug.LogError("[Exhibition] Nie znaleziono obiektu 'Visuals/Canvas_Quad' wewnątrz prefaba!");
        }
    }
}