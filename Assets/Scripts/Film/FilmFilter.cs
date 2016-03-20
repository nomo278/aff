using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

namespace Film
{
    public class FilmFilter : MonoBehaviour
    {
        RectTransform rectTransform;

        public FilmList filmList;
        public Button showFilters;

        public List<FilterSelection> filterSelections = new List<FilterSelection>();

        Dictionary<string, List<JSONNode>> blockToFilms;

        void CacheFilterSelections()
        {
            RectTransform rt = GetComponent<RectTransform>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    filterSelections.Add(rt.GetChild(i).GetChild(j).GetComponent<FilterSelection>());
                    filterSelections[filterSelections.Count - 1].toggle.isOn = false;
                    filterSelections[filterSelections.Count - 1].gameObject.SetActive(false);
                }
            }
            rectTransform = rt;
        }

        public void SetupFilters(Dictionary<string, List<JSONNode>> blockToFilms)
        {
            CacheFilterSelections();
            this.blockToFilms = blockToFilms;
            int i = 0;
            foreach (KeyValuePair<string, List<JSONNode>> kvp in blockToFilms)
            {
                filterSelections[i].gameObject.SetActive(true);
                filterSelections[i].text.text = kvp.Key;
                string key = kvp.Key;
                Toggle toggle = filterSelections[i].toggle;
                filterSelections[i].toggle.onValueChanged.AddListener(delegate { ValueChange(toggle, key); });
                i++;
            }
        }
        List<string> activeKeys = new List<string>();
        List<string> currentFilms = new List<string>();
        public void ValueChange(Toggle toggle, string key)
        {
            // Add to current list
            if(toggle.isOn)
            {
                if (!activeKeys.Contains(key))
                    activeKeys.Add(key);
                // Debug.Log("Added key: " + key);
            }
            else
            {
                if (activeKeys.Contains(key))
                    activeKeys.Remove(key);
                // Debug.Log("Remove key: " + key);
            }

            currentFilms.Clear();
            foreach(KeyValuePair<string, List<JSONNode>> kvp in blockToFilms)
            {
                if(activeKeys.Contains(kvp.Key))
                {
                    foreach(JSONNode node in kvp.Value)
                    {
                        string film = node["Film"];
                        currentFilms.Add(film);
                        // Debug.Log("Added: " + film);
                    }
                }
            }

            currentFilms.Sort();
            filmList.SetFilmList(currentFilms);
        }

        bool isShowing = false;
        public void ShowFilmFilter(bool isOn)
        {
            if(isOn)
            {
                rectTransform.anchorMin = new Vector2(0f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(1f, rectTransform.anchorMax.y);
            }
            else
            {
                rectTransform.anchorMin = new Vector2(-1f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(0f, rectTransform.anchorMax.y);
            }
            isShowing = isOn;
        }

        public void ToggleFilmFilter()
        {
            if(isShowing)
            {
                rectTransform.anchorMin = new Vector2(-1f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(0f, rectTransform.anchorMax.y);
            }
            else
            {
                rectTransform.anchorMin = new Vector2(0f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(1f, rectTransform.anchorMax.y);
            }
            isShowing = !isShowing;
        }
    }
}

