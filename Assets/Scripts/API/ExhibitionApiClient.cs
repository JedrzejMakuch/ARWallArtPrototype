using System;
using System.Collections;
using ArtTechGallery.API.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace ArtTechGallery.API
{
    public sealed class ExhibitionApiClient : MonoBehaviour
    {
        [SerializeField]
        private string baseUrl = "http://localhost:5184";

        public IEnumerator GetExhibition(
            string exhibitionCode,
            Action<ExhibitionApiModel> onSuccess,
            Action<string> onError)
        {
            if (string.IsNullOrWhiteSpace(exhibitionCode))
            {
                onError?.Invoke("Exhibition code cannot be empty.");
                yield break;
            }

            string encodedCode = UnityWebRequest.EscapeURL(exhibitionCode);
            string url = $"{baseUrl}/api/exhibitions/{encodedCode}";

            using UnityWebRequest request = UnityWebRequest.Get(url);

            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMessage =
                    $"Request failed. Status: {request.responseCode}, " +
                    $"Error: {request.error}, " +
                    $"Body: {request.downloadHandler.text}";

                onError?.Invoke(errorMessage);
                yield break;
            }

            string json = request.downloadHandler.text;

            ExhibitionApiModel exhibition;

            try
            {
                exhibition = JsonUtility.FromJson<ExhibitionApiModel>(json);
            }
            catch (Exception exception)
            {
                onError?.Invoke(
                    $"Could not deserialize exhibition JSON: {exception.Message}");

                yield break;
            }

            if (exhibition == null)
            {
                onError?.Invoke("API returned an empty exhibition response.");
                yield break;
            }

            onSuccess?.Invoke(exhibition);
        }
    }
}