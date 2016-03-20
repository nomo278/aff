using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using SimpleJSON;

using Maps;
using Events;

namespace Venues
{

    public class VenueList : MonoBehaviour
    {
        public GameObject venueEntryPrefab;
        public RectTransform venueEntriesParent;
        public Text venueTitle;

        public Button navigateButton;

        public EventInformation eventInformation;

        List<GameObject> venueEntries = new List<GameObject>();
        List<string> currentFilms;

        void OnEnable()
        {
            MapMarkerPlacer.OnVenueClick += OnVenueClick;
        }

        void OnDisable()
        {
            MapMarkerPlacer.OnVenueClick -= OnVenueClick;
        }

        void OnVenueClick(OnlineMapsMarker marker)
        {
            SetVenue(marker.venue);
            GetComponent<Animator>().enabled = true;
            GlobalController.instance.GoToMenuNoExit(GetComponent<AnimationPlayer>());
        }

        public void SetVenue(string venue)
        {
            Dictionary<string,List<string>> venueToFilms = ServerController.instance.venueToFilms;
            Dictionary<string, JSONNode> filmList = ServerController.instance.filmList;

            venueEntries.DeleteGameObjects();

            Dictionary<int, List<JSONNode>> dayToFilm = new Dictionary<int, List<JSONNode>>();
            float height = 0f;

            venueTitle.text = venue;

            navigateButton.onClick.RemoveAllListeners();
            string address = "http://maps.google.com/maps?q=" + MapMarkerPlacer.venueAddresses[venue];
            navigateButton.onClick.AddListener(() => Application.OpenURL(address));

            if (venueToFilms.ContainsKey(venue))
            {
                currentFilms = venueToFilms[venue];
                for(int i = 0; i < currentFilms.Count; i++)
                {
                    string film = currentFilms[i];
                    if (ServerController.instance.filmDates.ContainsKey(film))
                    {
                        int day = ServerController.instance.filmDates[film].Day;
                        if (!dayToFilm.ContainsKey(day))
                            dayToFilm.Add(day, new List<JSONNode>());
                        dayToFilm[day].Add(filmList[film]);
                    }
                }
                foreach(KeyValuePair<int, List<JSONNode>> kvp in dayToFilm)
                {
                    kvp.Value.Sort(Utility.CompareJSONNodeShowtimes);
                }

                // nodes.Sort(Utility.CompareJSONNodeShowtimes);
                List<int> days = new List<int>(dayToFilm.Keys);
                days.Sort();

                int p = 0;
                for(int i = 0; i < days.Count; i++)
                {
                    List<JSONNode> films = dayToFilm[days[i]];
                    for(int j = 0; j < films.Count; j++, p++)
                    {
                        venueEntries.Add((GameObject)Instantiate(venueEntryPrefab, Vector3.zero, Quaternion.identity));
                        GameObject go = venueEntries[venueEntries.Count - 1];
                        go.transform.SetParent(venueEntriesParent);
                        RectTransform rt = go.GetComponent<RectTransform>();
                        height = rt.rect.height;
                        rt.PositionEntry(p, height);
                        rt.FixOffsets();

                        string film = films[j]["Film"];
                        VenueEntry venueEntry = go.GetComponent<VenueEntry>();
                        venueEntry.filmName.text = film;
                        venueEntry.date.text = films[j]["Screening Day"];
                        venueEntry.timeSlot.text = films[j]["Time-Slot"];

                        JSONNode node = films[j];
                        go.GetComponent<Button>().onClick.AddListener(() => eventInformation.SetEventInformation(node));
                    }
                }
                venueEntriesParent.offsetMin = new Vector2(0f, Screen.height - (p * height));
                if(p < 5)
                    venueEntriesParent.offsetMin = new Vector2(0f, Screen.height - (4 * height));
            }
            else
            {
                venueEntries.Add((GameObject)Instantiate(venueEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = venueEntries[venueEntries.Count - 1];
                go.transform.SetParent(venueEntriesParent);
                RectTransform rt = go.GetComponent<RectTransform>();
                height = rt.rect.height;
                rt.PositionEntry(0, height);
                rt.FixOffsets();
                VenueEntry venueEntry = go.GetComponent<VenueEntry>();
                venueEntry.filmName.text = "No film showings found.";
                RectTransform rt_film = venueEntry.gameObject.GetComponent<RectTransform>();
                venueEntry.date.GetComponentInChildren<Text>().text = "";
                venueEntry.date.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
                venueEntry.date.text = "View events scheduled at this venue.";
                venueEntry.timeSlot.gameObject.SetActive(false);
                venueEntry.GetComponent<Button>().onClick.AddListener(() => Application.OpenURL("https://atlantafilmfestival2016.sched.org/"));
                venueEntriesParent.offsetMin = new Vector2(0f, Screen.height - (4 * height));
            }
            UpdateEntries();
        }

        public void UpdateEntries()
        {
            float y = venueEntriesParent.offsetMax.y;
            int topBuffer = ((int)((y - 750f) / 250f));
            int botBuffer = topBuffer + 10;

            for (int i = 0; i < venueEntries.Count; i++)
            {
                RectTransform rt = venueEntries[i].GetComponent<RectTransform>();
                if (i < topBuffer)
                    if (rt.gameObject.activeInHierarchy)
                        rt.gameObject.SetActive(false);
                if (i > botBuffer)
                    if (rt.gameObject.activeInHierarchy)
                        rt.gameObject.SetActive(false);
                if (i > topBuffer && i < botBuffer)
                {
                    if (!rt.gameObject.activeInHierarchy)
                        rt.gameObject.SetActive(true);
                }
            }
        }
    }

}
