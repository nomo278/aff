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
                bool isOn = filterSelections[i].toggle.isOn;
                filterSelections[i].toggle.onValueChanged.AddListener(delegate { ValueChange(isOn, key); });
                i++;
            }
            // ShowFilmFilter(true);
        }

        public void ValueChange(bool isOn, string key)
        {
            Debug.Log("key: " + key);
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

