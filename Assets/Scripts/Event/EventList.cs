using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;
using Calendars;

namespace Events
{
    public class EventList : MonoBehaviour
    {
        public RectTransform eventEntriesParent;
        public GameObject eventEntryPrefab;

        public GameObject emptyEntryPrefab;
        public GameObject emptyEntry;

        public Text eventMonth;

        public EventInformation eventInformation;

        List<GameObject> eventEntries = new List<GameObject>();

        Dictionary<int, List<string>> calendarToFilms = new Dictionary<int, List<string>>();

        bool eventInfoCached = false;

        void OnEnable()
        {
            EmptyEventList();
            Calendar calendar = FindObjectOfType<Calendar>();
            StartCoroutine(Utility.DelayedFunction(0.1f, () => calendar.SetupCalendarMonth(calendar.currentMonth)));
            StartCoroutine(Utility.DelayedFunction(0.2f, () => PopulateCalendar(calendar.currentMonth, calendar)));
            GlobalController.OnBackMenuClick += OnBackMenuClick;
        }

        void OnDisable()
        {
            Destroy(emptyEntry);
            eventEntries.DeleteGameObjects();
            GlobalController.OnBackMenuClick -= OnBackMenuClick;
        }

        void OnBackMenuClick()
        {
            eventEntriesParent.offsetMin = offsetMin;
            eventEntriesParent.offsetMax = offsetMax;
        }

        public void PopulateCalendar(DateTime dt, Calendar calendar)
        {
            if(!eventInfoCached)
                CacheEventInformation();

            // Remove old gameobjects if updating
            eventEntries.DeleteGameObjects();
            Destroy(emptyEntry);
            calendar.ClearDayHighlights();

            calendarToFilms.Clear();
            int month = dt.Month;
            eventMonth.text = dt.ToString("MMMM yyyy");

            // Get dates for current month
            foreach (KeyValuePair<string, DateTime> kvp in filmDates)
            {
                if (kvp.Value.Month == dt.Month)
                {
                    string film = filmList[kvp.Key]["Film"];
                    int date = kvp.Value.Day;
                    Day day = calendar.GetDay(date);

                    if (!calendarToFilms.ContainsKey(date))
                        calendarToFilms.Add(date, new List<string>());

                    calendarToFilms[date].Add(film);

                    day.SetHighlighted(true);
                    day.SetOnClick(() => PopulateEventList(date));
                }
            }
            EmptyEventList();
        }

        public void PopulateEventList(int calendarPos)
        {
            eventEntries.DeleteGameObjects();
            eventEntriesParent.offsetMin = new Vector2(0f, 0f);
            Destroy(emptyEntry);

            List<string> films = calendarToFilms[calendarPos];

            List<JSONNode> film_nodes = new List<JSONNode>();

            foreach(string str in films)
            {
                if(filmList.ContainsKey(str))
                    film_nodes.Add(filmList[str]);
            }
            film_nodes.Sort(Utility.CompareJSONNodeShowtimes);

            int i = 0;
            foreach(JSONNode node in film_nodes)
            {
                string film = node["Film"];

                eventEntries.Add((GameObject)Instantiate(eventEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = eventEntries[eventEntries.Count - 1];
                go.transform.parent = eventEntriesParent;

                // JSONNode rootNode = filmList[str];
                go.GetComponent<Button>().onClick.AddListener(() => eventInformation.SetEventInformation(node));

                RectTransform rt = go.GetComponent<RectTransform>();

                float height = rt.rect.height;
                rt.PositionEntry(i, height);
                rt.FixOffsets();

                EventEntry eventEntry = go.GetComponent<EventEntry>();
                eventEntry.filmName.text = node["Film"];
                eventEntry.directors.text = node["Director(s)"];
                eventEntry.venue.text = node["Venue"];
                eventEntry.timeSlot.text = node["Time-Slot"];

                eventEntriesParent.offsetMin = new Vector2(0f, eventEntriesParent.offsetMin.y - height);

                i++;
            }
            CacheOffset();
        }

        Vector2 offsetMin, offsetMax;
        void CacheOffset()
        {
            offsetMin = eventEntriesParent.offsetMin;
            offsetMax = eventEntriesParent.offsetMax;
        }

        public void EmptyEventList()
        {
            emptyEntry = (GameObject)Instantiate(emptyEntryPrefab, Vector3.zero, Quaternion.identity);
            emptyEntry.transform.parent = eventEntriesParent;
            RectTransform rt = emptyEntry.GetComponent<RectTransform>();
            float height = rt.rect.height;
            rt.PositionEntry(0, height);
            rt.FixOffsets();
        }

        Dictionary<string, JSONNode> filmList;
        Dictionary<string, DateTime> filmDates;

        public void CacheEventInformation()
        {
            filmList = ServerController.instance.filmList;
            filmDates = ServerController.instance.filmDates;
            eventInfoCached = true;
        }
    }
}

