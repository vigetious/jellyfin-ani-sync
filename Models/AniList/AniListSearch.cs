using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace jellyfin_ani_sync.Models; 

public class AniListSearch {
    public class PageInfo
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("perPage")]
        public int PerPage { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("lastPage")]
        public int LastPage { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }

    public class Title
    {
        [JsonPropertyName("romaji")]
        public string Romaji { get; set; }

        [JsonPropertyName("english")]
        public string English { get; set; }

        [JsonPropertyName("native")]
        public string Native { get; set; }

        [JsonPropertyName("userPreferred")]
        public string UserPreferred { get; set; }
    }
    
    public class MediaEdge
    {
        [JsonPropertyName("relationType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaRelation RelationType { get; set; }

        [JsonPropertyName("node")]
        public Media Media { get; set; }
    }

    /// <summary>
    /// Relations.
    /// </summary>
    public class MediaConnection
    {
        [JsonPropertyName("edges")]
        public List<MediaEdge> Edges { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public Title Title { get; set; }
        
        [JsonPropertyName("episodes")]
        public int Episodes { get; set; }

        [JsonPropertyName("relations")]
        public MediaConnection MediaConnection { get; set; }
    }

    public class Page
    {
        [JsonPropertyName("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonPropertyName("media")]
        public List<Media> Media { get; set; }
    }

    public class AniListSearchData
    {
        [JsonPropertyName("Page")]
        public Page Page { get; set; }
    }

    public class AniListSearchMedia
    {
        [JsonPropertyName("data")]
        public AniListSearchData Data { get; set; }
    }
    
    public enum MediaRelation {
        Adaptation,
        Prequel,
        Sequel,
        Parent,
        Side_Story,
        Character,
        Summary,
        Alternative,
        Spin_Off,
        Other,
        Source,
        Compilation,
        Contains
    }
}