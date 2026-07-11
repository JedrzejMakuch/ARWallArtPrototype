using ArtTechGallery.API.Models;
using UnityEngine;
using System.Collections.Generic;

namespace ArtTechGallery.API
{
    public sealed class ExhibitionApiTest : MonoBehaviour
    {
        [SerializeField]
        private ExhibitionApiClient apiClient;

        [SerializeField]
        private string exhibitionCode = "colors-of-nature";

        [SerializeField]
        private AR_Exhibition_Manager exhibitionManager;

        private void Start()
        {
            StartCoroutine(
                apiClient.GetExhibition(
                    exhibitionCode,
                    HandleSuccess,
                    HandleError));
        }

        private void HandleSuccess(ExhibitionApiModel exhibition)
        {
            int artworksCount =
                exhibition.artworks == null
                    ? 0
                    : exhibition.artworks.Length;

            Debug.Log(
                $"Loaded exhibition: {exhibition.title}, " +
                $"artist: {exhibition.artistDisplayName}, " +
                $"artworks: {artworksCount}");

            List<ArtworkData> runtimeArtworks = new List<ArtworkData>();

            if (exhibition.artworks != null)
            {
                foreach (ArtworkApiModel artwork in exhibition.artworks)
                {
                    ArtworkData runtimeArtwork = new ArtworkData
                    {
                        title = artwork.title,
                        texture = null,
                        widthInMeters = artwork.widthCm / 100f,
                        heightInMeters = artwork.heightCm / 100f
                    };

                    runtimeArtworks.Add(runtimeArtwork);

                    Debug.Log(
                        $"Mapped artwork: {runtimeArtwork.title}, " +
                        $"{runtimeArtwork.widthInMeters} x " +
                        $"{runtimeArtwork.heightInMeters} m");
                }
            }

            exhibitionManager.SetArtworks(runtimeArtworks);

            Debug.Log(
                $"Runtime exhibition updated with " +
                $"{runtimeArtworks.Count} artworks.");
        }

        private static void HandleError(string error)
        {
            Debug.LogError($"Exhibition API error: {error}");
        }
    }
}