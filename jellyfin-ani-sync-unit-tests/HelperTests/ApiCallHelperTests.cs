using System;
using System.Collections.Generic;
using jellyfin_ani_sync.Helpers;
using jellyfin_ani_sync.Models;
using jellyfin_ani_sync.Models.Kitsu;
using jellyfin_ani_sync.Models.Mal;
using NUnit.Framework;

namespace jellyfin_ani_sync_unit_tests.HelperTests;

public class ApiCallHelperTests {
    [Test]
    public void AniListSearchAnimeConvertedListTest() {
        List<AniListSearch.Media> mediaList = new List<AniListSearch.Media>();
        for (int i = 0; i < 10; i++) {
            mediaList.Add(GetAniListMedia(true));
        }

        ApiCallHelpers apiCallHelpers = new ApiCallHelpers();
        List<Anime> animeList = apiCallHelpers.AniListSearchAnimeConvertedList(mediaList, true);
        Assert.AreEqual(10, animeList.Count);
        // Assert that the anime list contains the expected anime objects
        for (int i = 0; i < animeList.Count; i++) {
            Assert.AreEqual(mediaList[i].Id, animeList[i].Id);
            Assert.AreEqual(mediaList[i].Episodes, animeList[i].NumEpisodes);
            Assert.AreEqual(mediaList[i].Title.English, animeList[i].Title);
            Assert.AreEqual(mediaList[i].Title.English, animeList[i].AlternativeTitles.En);
            Assert.AreEqual(mediaList[i].Title.Native, animeList[i].AlternativeTitles.Ja);
            Assert.AreEqual(new List<string> { mediaList[i].Title.Romaji, mediaList[i].Title.UserPreferred, "Synonym1", "Synonym2" }, animeList[i].AlternativeTitles.Synonyms);
        }
    }

    [Test]
    public void KitsuSearchAnimeConvertedListTest() {
        List<KitsuSearch.KitsuAnime> mediaList = new List<KitsuSearch.KitsuAnime>();
        for (int i = 0; i < 10; i++) {
            mediaList.Add(GetKitsuAnime(true));
        }

        List<Anime> animeList = ApiCallHelpers.KitsuSearchAnimeConvertedList(mediaList);
        Assert.AreEqual(10, animeList.Count);
        // Assert that the anime list contains the expected anime objects
        for (int i = 0; i < animeList.Count; i++) {
            Assert.AreEqual(mediaList[i].Id, animeList[i].Id);
            Assert.AreEqual(mediaList[i].Attributes.EpisodeCount, animeList[i].NumEpisodes);
            Assert.AreEqual(mediaList[i].Attributes.Titles.English, animeList[i].Title);
            Assert.AreEqual(mediaList[i].Attributes.Titles.English, animeList[i].AlternativeTitles.En);
            Assert.AreEqual(mediaList[i].Attributes.Titles.Japanese, animeList[i].AlternativeTitles.Ja);
            Assert.AreEqual(new List<string> { mediaList[i].Attributes.Slug, mediaList[i].Attributes.CanonicalTitle, "Synonym1", "Synonym2" }, animeList[i].AlternativeTitles.Synonyms);
        }
    }
    
    private AniListSearch.Media GetAniListMedia(bool createRelations) {
        Random random = new Random();
        AniListSearch.MediaConnection mediaConnection = new AniListSearch.MediaConnection();
        if (createRelations) {
            mediaConnection.Media = new List<AniListSearch.MediaEdge>();
            for (int i = 0; i < random.Next(0, 5); i++) {
                mediaConnection.Media.Add(new AniListSearch.MediaEdge {
                    Media = GetAniListMedia(false),
                    RelationType = (AniListSearch.MediaRelation)random.Next(0, 12)
                });
            }
        }

        return new AniListSearch.Media {
            Id = random.Next(1, 100),
            Episodes = random.Next(1, 100),
            IsAdult = random.Next(0, 1) == 0,
            MediaListEntry = new AniListSearch.MediaListEntry {
                CompletedAt = new AniListSearch.FuzzyDate {
                    Day = random.Next(1, 31),
                    Month = random.Next(1, 12),
                    Year = random.Next(1970, 2024)
                },
                MediaListStatus = (AniListSearch.MediaListStatus)random.Next(0, 5),
                Progress = random.Next(0, 100),
                RepeatCount = random.Next(0, 10),
                StartedAt = new AniListSearch.FuzzyDate {
                    Day = random.Next(1, 31),
                    Month = random.Next(1, 12),
                    Year = random.Next(1970, 2024)
                }
            },
            Relations = mediaConnection,
            Title = new AniListSearch.Title { Romaji = "Title", English = "Title", Native = "Title", UserPreferred = "Title" },
            Synonyms = new List<string> { "Synonym1", "Synonym2" },
            Status = (AniListSearch.AiringStatus)random.Next(0, 4),
            SiteUrl = "https://example.com"
        };
    }
    
    private KitsuSearch.KitsuAnime GetKitsuAnime(bool createRelations) {
        Random random = new Random();
        KitsuSearch.MediaRelationships mediaRelationships = new KitsuSearch.MediaRelationships();
        if (createRelations) {
            mediaRelationships.Data = new List<KitsuSearch.KitsuAnime>();
            for (int i = 0; i < random.Next(0, 5); i++) {
                KitsuSearch.KitsuAnime anime = GetKitsuAnime(false);
                anime.RelationType = (KitsuMediaRelationship.RelationType?)random.Next(0, 12);
                mediaRelationships.Data.Add(anime);
            }
        }
        KitsuSearch.Relationships relationships = new KitsuSearch.Relationships 
        {
            MediaRelationships = mediaRelationships
        };
        return new KitsuSearch.KitsuAnime {
            Id = random.Next(1, 100),
            Attributes = new KitsuSearch.Attributes {
                EpisodeCount = random.Next(1, 200),
                CanonicalTitle = "Title",
                Titles = new KitsuSearch.Titles { English = "Title", EnJp = "Title", Japanese = "Title" },
                AbbreviatedTitles = new List<string> { "Synonym1", "Synonym2" },
            },
            Relationships = relationships
        };
    }
}