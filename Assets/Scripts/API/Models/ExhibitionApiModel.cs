using System;

namespace ArtTechGallery.API.Models
{
    [Serializable]
    public sealed class ExhibitionApiModel
    {
        public string id;
        public string exhibitionCode;
        public string title;
        public string description;
        public string artistDisplayName;
        public string artistProfileCode;
        public ArtworkApiModel[] artworks;
    }
}