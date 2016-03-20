using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using SimpleJSON;
using Maps;

namespace Events
{
    public class EventInformation : MonoBehaviour
    {
        public RectTransform eventInfoEntriesParent;
        public GameObject eventInfoEntryPrefab;

        List<GameObject> eventInfoEntries = new List<GameObject>();

        static string[] keys = new string[] { "Film", "Director(s)", "Category", "RT", "Synposis", "Trailer",
                "ATLFF Webpage", "Venue", "Screening Day", "Time-Slot",  "Premier Status", "Country", "Language",
                "Subtitles", "Website", "Facebook", "Twitter"};

        public void SetEventInformation(JSONNode rootNode)
        {
            AnimationPlayer animPlayer = GetComponent<AnimationPlayer>();

            // Debug.Log("show information for film: " + rootNode["Film"]);

            eventInfoEntries.DeleteGameObjects();

            for(int i = 0, j = 0; i < keys.Length; i++, j++)
            {
                string info = rootNode[keys[i]];
                string label = keys[i];

                bool addEntry = false;

                if(info == null)
                {
                    j--;
                }
                else
                {
                    if(info.Length < 1)
                    {
                        j--;
                    }
                    else
                    {
                        addEntry = true;
                    }
                }

                if(addEntry)
                {
                    eventInfoEntries.Add((GameObject)Instantiate(eventInfoEntryPrefab, Vector3.zero, Quaternion.identity));
                    GameObject go = eventInfoEntries[eventInfoEntries.Count - 1];
                    go.transform.parent = eventInfoEntriesParent;

                    RectTransform rt = go.GetComponent<RectTransform>();
                    float height = rt.rect.height;
                    rt.PositionEntry(j, height);
                    rt.FixOffsets();

                    EventInfoEntry eventInfoEntry = go.GetComponent<EventInfoEntry>();
                    eventInfoEntry.info.text = info;
                    eventInfoEntry.label.text = label;
                    eventInfoEntriesParent.offsetMin = new Vector2(0f, eventInfoEntriesParent.offsetMin.y - height);

                    Button button = go.GetComponent<Button>();
                    switch(keys[i])
                    {
                        case "RT":
                            eventInfoEntry.label.text = "Runtime";
                            break;
                        case "Trailer":
                            button.onClick.AddListener(() => Application.OpenURL(info));
                            break;
                        case "ATLFF Webpage":
                            button.onClick.AddListener(() => Application.OpenURL(info));
                            break;
                        case "Website":
                            button.onClick.AddListener(() => Application.OpenURL(info));
                            break;
                        case "Venue":
                            eventInfoEntry.info.text = MapMarkerPlacer.inputVenueToVenue[info];
                            string address = "http://maps.google.com/maps?q=" + MapMarkerPlacer.venueAddresses[MapMarkerPlacer.inputVenueToVenue[info]];
                            button.onClick.AddListener(() => Application.OpenURL(address));
                            break;
                        case "Twitter":
                            string twitter_url = "http://www.twitter.com/" + info.Trim('@');
                            button.onClick.AddListener(() => Application.OpenURL(twitter_url));
                            break;
                    }

                }

                if(i == keys.Length-1)
                {
                    eventInfoEntries.Add((GameObject)Instantiate(eventInfoEntryPrefab, Vector3.zero, Quaternion.identity));
                    GameObject go = eventInfoEntries[eventInfoEntries.Count - 1];
                    go.transform.parent = eventInfoEntriesParent;

                    RectTransform rt = go.GetComponent<RectTransform>();
                    float height = rt.rect.height;
                    rt.PositionEntry(j+1, height);
                    rt.FixOffsets();

                    EventInfoEntry eventInfoEntry = go.GetComponent<EventInfoEntry>();
                    eventInfoEntry.info.text = "Back";
                    eventInfoEntry.label.gameObject.SetActive(false);
                    go.GetComponent<Button>().onClick.AddListener(() => GlobalController.instance.BackMenuNoEnter());
                    eventInfoEntriesParent.offsetMin = new Vector2(0f, eventInfoEntriesParent.offsetMin.y - height);
                }
            }

            GlobalController.instance.GoToMenuNoExit(animPlayer);
        }
    }
}
