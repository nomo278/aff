using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

namespace Film
{
    public class FilmList : MonoBehaviour
    {
        public RectTransform filmListEntriesParent;
        public GameObject filmListEntryPrefab;

        List<GameObject> filmEntries = new List<GameObject>();
        Dictionary<string,JSONNode> filmList = new Dictionary<string, JSONNode>();

        /*
        public const string secretKey = "secretKey";
        public const string filmListUrl = "http://www.surconsultinggroup.com/aff/getfilms.php";

        public void GetFilmList()
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("hash", Utility.Md5Sum(secretKey));
            WWW post = new WWW(filmListUrl, wwwForm);
            StartCoroutine(GetFilmList(post));
        }

        IEnumerator GetFilmList(WWW post)
        {
            yield return post;
            CacheFilmList(JSON.Parse(post.text));
        }

        List<JSONNode> filmList = new List<JSONNode>();
        void CacheFilmList(JSONNode rootNode)
        {
            if(filmList.Count > 0)
                filmList.Clear();

            foreach(JSONNode jn in rootNode.AsArray)
            {
                filmList.Add(jn);
            }
            PopulateFilmList();
        } */

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
            if (filmEntries.Count > 0)
            {
                for(int i = 0; i < filmEntries.Count; i++)
                {
                    Destroy(filmEntries[i]);
                }
                filmEntries.Clear();
            }

            int j = 0;
            foreach(KeyValuePair<string,JSONNode> kvp in filmList)
            {
                filmEntries.Add((GameObject)Instantiate(filmListEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = filmEntries[filmEntries.Count - 1];
                go.transform.parent = filmListEntriesParent;
                RectTransform rt = go.GetComponent<RectTransform>();
                float height = rt.rect.height;
                rt.anchoredPosition = new Vector2(0f, -j * height);
                rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
                rt.offsetMax = new Vector2(1f, rt.offsetMax.y);
                rt.localScale = Vector3.one;

                JSONNode jn = kvp.Value;
                FilmEntry fe = go.GetComponent<FilmEntry>();
                fe.SetFilmName(jn["Film"]);
                fe.SetDirectors(jn["Director(s)"]);

                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
                j++;
            }
            /*
            for (int i = 0; i < filmList.Count; i++)
            {
                filmEntries.Add((GameObject)Instantiate(filmListEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = filmEntries[filmEntries.Count - 1];
                go.transform.parent = filmListEntriesParent;
                RectTransform rt = go.GetComponent<RectTransform>();
                float height = rt.rect.height;
                rt.anchoredPosition = new Vector2(0f, -i * height);
                rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
                rt.offsetMax = new Vector2(1f, rt.offsetMax.y);
                rt.localScale = Vector3.one;

                JSONNode jn = filmList[i];
                FilmEntry fe = go.GetComponent<FilmEntry>();
                fe.SetFilmName(jn["Film"]);
                fe.SetDirectors(jn["Director(s)"]);

                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
            } */

            UpdateEntries();
        }

        // Populate film list in alphabetical order over time to reduce instantiation workload
        /*
        IEnumerator PopulateList(float timeSpacing)
        {
            while(filmEntries.Count < filmList.Count)
            {
                filmEntries.Add((GameObject)Instantiate(filmListEntryPrefab, Vector3.zero, Quaternion.identity));
                GameObject go = filmEntries[filmEntries.Count - 1];
                go.transform.parent = filmListEntriesParent;
                RectTransform rt = go.GetComponent<RectTransform>();
                float height = rt.rect.height;
                rt.anchoredPosition = new Vector2(0f, - (filmEntries.Count - 1) * height);
                rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
                rt.offsetMax = new Vector2(1f, rt.offsetMax.y);
                rt.localScale = Vector3.one;

                JSONNode jn = filmList[(filmEntries.Count - 1)];
                FilmEntry fe = go.GetComponent<FilmEntry>();
                fe.SetFilmName(jn["Film"]);
                fe.SetDirectors(jn["Director(s)"]);

                filmListEntriesParent.offsetMin = new Vector2(0f, filmListEntriesParent.offsetMin.y - height);
                yield return new WaitForSeconds(timeSpacing);
            }
        } */

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

