using System;

namespace ArtTechGallery.API.Models
{
    [Serializable]
    public sealed class ArtworkApiModel
    {
        public string id;
        public string title;
        public string description;
        public int creationYear;
        public float widthCm;
        public float heightCm;
        public string imageUrl;
        public int sortOrder;
    }
}