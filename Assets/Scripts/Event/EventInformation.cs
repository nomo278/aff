using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using SimpleJSON;

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
            animPlayer.EnterAnimation();

            Debug.Log("show information for film: " + rootNode["Film"]);

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
                    go.GetComponent<Button>().onClick.AddListener(() => animPlayer.ExitAnimation());
                    eventInfoEntriesParent.offsetMin = new Vector2(0f, eventInfoEntriesParent.offsetMin.y - height);
                }
            }
            /*
            RectTransform temp = eventInfoEntriesParent.parent.GetComponent<RectTransform>();
            temp.offsetMin = new Vector2(temp.offsetMin.x, 0f);
            temp.offsetMax = new Vector2(temp.offsetMax.x, 0f);*/
        }
    }
}
