using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

using Calendars;
using Maps;

public class ServerController : MonoBehaviour
{
    private static ServerController _instance;

    public static ServerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ServerController>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    public const string secretKey = "secretKey";
    public const string filmListUrl = "http://www.surconsultinggroup.com/aff/getfilms.php";

    public Calendar calendar;

    void Start()
    {
        GetFilmList();
    }

    public void GetFilmList()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("hash", Utility.Md5Sum(secretKey));
        WWW post = new WWW(filmListUrl, wwwForm);
        StartCoroutine(GetFilmList(post));
    }

    JSONNode rootNode;

    IEnumerator GetFilmList(WWW post)
    {
        yield return post;
        rootNode = JSON.Parse(post.text);
        CacheFilmList(rootNode);
    }

    public Dictionary<string, JSONNode> filmList = new Dictionary<string, JSONNode>();
    void CacheFilmList(JSONNode rootNode)
    {
        if (filmList.Count > 0)
            filmList.Clear();

        foreach (JSONNode jn in rootNode.AsArray)
        {
            filmList.Add(jn["Film"],jn);
        }
        CacheData();
    }

    public Dictionary<string, DateTime> filmDates = new Dictionary<string, DateTime>();
    public Dictionary<string, List<string>> venueToFilms = new Dictionary<string, List<string>>();

    public Dictionary<string, List<string>> categoryToFilm = new Dictionary<string, List<string>>();

    public void CacheData()
    {
        foreach (KeyValuePair<string, JSONNode> kvp in filmList)
        {
            string dateRaw = kvp.Value["Screening Day"];
            bool found = false;
            if (dateRaw != null)
            {
                if (dateRaw.Length > 0)
                {
                    if (dateRaw.Contains("(") && dateRaw.Contains(")"))
                    {
                        string dateString = dateRaw.Split('(', ')')[1];
                        string parseDateString = dateString += "/" + Calendars.Calendar.now.Year;
                        DateTime dt = Convert.ToDateTime(parseDateString);
                        filmDates.Add(kvp.Key, dt);
                        found = true;
                    }
                }
            }

            string venueRaw = kvp.Value["Venue"];
            if (venueRaw != null)
            {
                if (venueRaw.Length > 0)
                {
                    if (MapMarkerPlacer.inputVenueToVenue.ContainsKey(venueRaw))
                    {
                        string venue = MapMarkerPlacer.inputVenueToVenue[venueRaw];
                        if (!venueToFilms.ContainsKey(venue))
                        {
                            venueToFilms.Add(venue, new List<string>());
                        }
                        string filmRaw = kvp.Value["Film"];
                        if (filmRaw != null)
                        {
                            if (filmRaw.Length > 0)
                            {
                                venueToFilms[venue].Add(filmRaw);
                                string categoryRaw = kvp.Value["Category"];
                                if (venueRaw != null)
                                {
                                    if (venueRaw.Length > 0)
                                    {
                                        if (!categoryToFilm.ContainsKey(categoryRaw))
                                            categoryToFilm.Add(categoryRaw, new List<string>());
                                        categoryToFilm[categoryRaw].Add(filmRaw);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!found)
                filmDates.Add(kvp.Key, DateTime.MinValue);
        }

    }
}
