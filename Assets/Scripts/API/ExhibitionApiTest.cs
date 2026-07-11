using ArtTechGallery.API.Models;
using UnityEngine;

namespace ArtTechGallery.API
{
    public sealed class ExhibitionApiTest : MonoBehaviour
    {
        [SerializeField]
        private ExhibitionApiClient apiClient;

        [SerializeField]
        private string exhibitionCode = "colors-of-nature";

        private void Start()
        {
            StartCoroutine(
                apiClient.GetExhibition(
                    exhibitionCode,
                    HandleSuccess,
                    HandleError));
        }

        private static void HandleSuccess(
            ExhibitionApiModel exhibition)
        {
            int artworksCount =
                exhibition.artworks == null
                    ? 0
                    : exhibition.artworks.Length;

            Debug.Log(
                $"Loaded exhibition: {exhibition.title}, " +
                $"artist: {exhibition.artistDisplayName}, " +
                $"artworks: {artworksCount}");

            if (exhibition.artworks == null)
            {
                return;
            }

            foreach (ArtworkApiModel artwork in exhibition.artworks)
            {
                Debug.Log(
                    $"Artwork: {artwork.title}, " +
                    $"{artwork.widthCm} x {artwork.heightCm} cm");
            }
        }

        private static void HandleError(string error)
        {
            Debug.LogError($"Exhibition API error: {error}");
        }
    }
}