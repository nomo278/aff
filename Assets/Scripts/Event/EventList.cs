using UnityEngine;
using UnityEngine.UI;
using System;
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
            calendar.SetupCalendarMonth(calendar.currentMonth);
            PopulateCalendar(calendar.currentMonth, calendar);
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
            int i = 0;
            foreach(string str in calendarToFilms[calendarPos])
            {
                string film = filmList[str]["Film"];

                eventEntries.Add((GameObject)Instantiate(eventEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = eventEntries[eventEntries.Count - 1];
                go.transform.parent = eventEntriesParent;

                JSONNode rootNode = filmList[str];
                go.GetComponent<Button>().onClick.AddListener(() => eventInformation.SetEventInformation(rootNode));

                RectTransform rt = go.GetComponent<RectTransform>();

                float height = rt.rect.height;
                rt.PositionEntry(i, height);
                rt.FixOffsets();

                EventEntry eventEntry = go.GetComponent<EventEntry>();
                eventEntry.filmName.text = filmList[str]["Film"];
                eventEntry.directors.text = filmList[str]["Director(s)"];
                eventEntry.venue.text = filmList[str]["Venue"];
                eventEntry.timeSlot.text = filmList[str]["Time-Slot"];

                eventEntriesParent.offsetMin = new Vector2(0f, eventEntriesParent.offsetMin.y - height);

                i++;
            }
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

