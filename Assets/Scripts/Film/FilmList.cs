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
        public FilmFilter filmFilter;

        List<GameObject> filmEntries = new List<GameObject>();
        Dictionary<string,JSONNode> filmList = new Dictionary<string, JSONNode>();

        Dictionary<string, RectTransform> filmToGameObject = new Dictionary<string, RectTransform>();

        public Dictionary<string, List<JSONNode>> blockToFilms = new Dictionary<string, List<JSONNode>>();

        void OnEnable()
        {
            if(filmEntries.Count < 1)
            {
                PopulateFilmList();
            }
            GlobalController.OnBackMenuClick += OnBackMenuClick;
        }

        void OnDisable()
        {
            GlobalController.OnBackMenuClick -= OnBackMenuClick;
        }

        void OnBackMenuClick()
        {
            filmListEntriesParent.offsetMin = offsetMin;
            filmListEntriesParent.offsetMax = offsetMax;
        }

        Dictionary<JSONNode, int> originalPositions = new Dictionary<JSONNode, int>();

        void PopulateFilmList()
        {
            filmList = ServerController.instance.filmList;

            int j = 0;
            foreach (KeyValuePair<string, JSONNode> kvp in filmList)
            {
                string film = kvp.Key;
                JSONNode jn = kvp.Value;

                originalPositions.Add(jn, j);

                filmEntries.Add((GameObject)Instantiate(filmListEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = filmEntries[filmEntries.Count - 1];
                go.transform.parent = filmListEntriesParent;
                RectTransform rt = go.GetComponent<RectTransform>();

                filmToGameObject.Add(film, rt);
                currentFilms.Add(film);

                float height = rt.rect.height;
                rt.PositionEntry(j, height);
                rt.FixOffsets();

                FilmEntry fe = go.GetComponent<FilmEntry>();
                fe.SetFilmName(jn["Film"]);
                fe.SetDirectors(jn["Director(s)"]);

                JSONNode rootNode = kvp.Value;
                go.GetComponent<Button>().onClick.AddListener(() => eventInformation.SetEventInformation(rootNode));

                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
                j++;

                // Add films to blocks for filtering
                string block_raw = jn["Block(s)"];
                if (block_raw != null) { 
                    if (block_raw.Length > 0)
                    {
                        string[] blocks = block_raw.Split(',');
                        foreach (string block in blocks)
                        {
                            if (!blockToFilms.ContainsKey(block))
                                blockToFilms.Add(block, new List<JSONNode>());
                            if(!blockToFilms[block].Contains(jn))
                                blockToFilms[block].Add(jn);
                        }
                    }
                }

                originalFilms = new List<string>(currentFilms);
            }

            CacheOffset();
            UpdateEntries();
            filmFilter.SetupFilters(blockToFilms);
        }
        List<string> originalFilms = new List<string>();
        List<string> currentFilms = new List<string>();
        public void SetFilmList(List<string> films)
        {
            if(films.Count < 1)
            {
                films = originalFilms;
            }
            else
            {
                // Disable all game objects
                foreach (KeyValuePair<string, RectTransform> kvp in filmToGameObject)
                {
                    if(kvp.Value.gameObject.activeInHierarchy)
                        kvp.Value.gameObject.SetActive(false);
                }
            }


            filmListEntriesParent.offsetMin = new Vector2(0f, Screen.height);
            // Enable and position correct gameobjects
            for (int i = 0; i < films.Count; i++)
            {
                RectTransform rt = filmToGameObject[films[i]];
                rt.gameObject.SetActive(true);
                float height = rt.rect.height;
                rt.PositionEntry(i, height);
                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
            }

            currentFilms = films;
        }

        Vector2 offsetMin, offsetMax;
        void CacheOffset()
        {
            offsetMin = filmListEntriesParent.offsetMin;
            offsetMax = filmListEntriesParent.offsetMax;
        }

        public void UpdateEntries()
        {
            float y = filmListEntriesParent.offsetMax.y;
            int topBuffer = ((int)((y - 750f) / 250f));
            int botBuffer = topBuffer + 10;
            /*
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
            } */

            for(int i = 0; i < currentFilms.Count; i++)
            {
                RectTransform rt = filmToGameObject[currentFilms[i]];
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

