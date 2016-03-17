using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

using Events;

namespace Film
{
    public class FilmList : MonoBehaviour
    {
        public RectTransform filmListEntriesParent;
        public GameObject filmListEntryPrefab;

        public EventInformation eventInformation;

        List<GameObject> filmEntries = new List<GameObject>();
        Dictionary<string,JSONNode> filmList = new Dictionary<string, JSONNode>();

        void OnEnable()
        {
            if(filmEntries.Count < 1)
            {
                PopulateFilmList();
            }
        }

        void PopulateFilmList()
        {
            filmList = ServerController.instance.filmList;
            // Remove old gameobjects if updating
            filmEntries.DeleteGameObjects();

            int j = 0;
            foreach(KeyValuePair<string,JSONNode> kvp in filmList)
            {
                filmEntries.Add((GameObject)Instantiate(filmListEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = filmEntries[filmEntries.Count - 1];
                go.transform.parent = filmListEntriesParent;
                RectTransform rt = go.GetComponent<RectTransform>();

                float height = rt.rect.height;
                rt.PositionEntry(j,height);
                rt.FixOffsets();

                JSONNode jn = kvp.Value;
                FilmEntry fe = go.GetComponent<FilmEntry>();
                fe.SetFilmName(jn["Film"]);
                fe.SetDirectors(jn["Director(s)"]);

                JSONNode rootNode = kvp.Value;
                go.GetComponent<Button>().onClick.AddListener(() => eventInformation.SetEventInformation(rootNode));

                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
                j++;
            }
            UpdateEntries();
        }

        public void UpdateEntries()
        {
            float y = filmListEntriesParent.offsetMax.y;
            int topBuffer = ((int)((y - 750f) / 250f));
            int botBuffer = topBuffer + 10;
            for(int i = 0; i < filmEntries.Count; i++)
            {
                if (i < topBuffer)
                    if (filmEntries[i].activeInHierarchy)
                        filmEntries[i].SetActive(false);
                if (i > botBuffer)
                    if (filmEntries[i].activeInHierarchy)
                        filmEntries[i].SetActive(false);
                if(i > topBuffer && i < botBuffer)
                {
                    if(!filmEntries[i].activeInHierarchy)
                        filmEntries[i].SetActive(true);
                }
            }
        }
    }
}

