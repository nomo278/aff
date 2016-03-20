using UnityEngine;
using System.Collections.Generic;

using SimpleJSON;

namespace Film
{
    public enum FilmCategory
    {
        NarrativeFeatures, AnniversaryScreenings, ShortBlocks, DocumentaryFeatures
    }

    public class FilmCategories : MonoBehaviour
    {
        public FilmList filmList;

        public static Dictionary<string, string> inputCategoryToCategory = new Dictionary<string, string>
        {
            { "ES", "Experimental Short" },
            { "DFC", "Documentary Feature Competition" },
            { "NF", "Narrative Feature" },
            { "NFC", "Narrative Feature Competition" },
            { "DF", "Documentary Feature"},
            { "NS", "Narrative Short" },
            { "ASC", "Animated Short Competition" },
            { "NSC", "Narrative Short Competition"},
            { "DS", "Documentary Short" },
            { "PS", "Puppetry Short" },
            { "MV", "Music Video"},
            { "PI", ""},
            { "DSC", "Documentary Short Competition"},
            { "AS", "Animated Short"},
        };

        public void SetFilmList(int category)
        {
            SetFilmList((FilmCategory)category);
        }

        public void SetFilmList(FilmCategory filmCategory)
        {
            Dictionary<string, List<string>> filmToCategory = ServerController.instance.categoryToFilm;
            List<string> films = null;
            List<string> keys = null;
            switch (filmCategory)
            {
                case FilmCategory.NarrativeFeatures:
                    keys = new List<string> { "NF", "NFC" };
                    films = new List<string>();
                    foreach (string key in keys)
                    {
                        foreach (string film in filmToCategory[key])
                        {
                            films.Add(film);
                        }
                    }
                    films.Sort();
                    filmList.OnEnableAfter = delegate {
                        filmList.SetFilmList(films);
                        filmList.SetCategory(filmCategory);
                        filmList.SetCategoryText("Narrative Features");
                    };
                    break;
                case FilmCategory.AnniversaryScreenings:
                    keys = new List<string> { "AS" };
                    films = new List<string>();
                    foreach (string key in keys)
                    {
                        foreach (string film in filmToCategory[key])
                        {
                            films.Add(film);
                        }
                    }
                    films.Sort();
                    filmList.OnEnableAfter = delegate {
                        filmList.SetFilmList(films);
                        filmList.SetCategory(filmCategory);
                        filmList.SetCategoryText("Anniversary Screenings");
                    };
                    break;
                case FilmCategory.ShortBlocks:
                    filmList.OnEnableAfter = delegate {
                        filmList.SetCategory(filmCategory);
                        filmList.SetCategoryText("Short Blocks");
                    };
                    break;
                case FilmCategory.DocumentaryFeatures:
                    keys = new List<string> { "DF", "DFC" };
                    films = new List<string>();
                    foreach (string key in keys)
                    {
                        foreach (string film in filmToCategory[key])
                        {
                            films.Add(film);
                        }
                    }
                    films.Sort();
                    filmList.OnEnableAfter = delegate {
                        filmList.SetFilmList(films);
                        filmList.SetCategory(filmCategory);
                        filmList.SetCategoryText("Documentary Features");
                    };
                    break;
            }
            GlobalController.instance.GoToMenu(filmList.GetComponent<AnimationPlayer>());
        }
    }
}

