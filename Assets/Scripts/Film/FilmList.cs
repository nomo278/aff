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

        public const string secretKey = "secretKey";
        public const string filmListUrl = "http://www.surconsultinggroup.com/aff/getfilms.php";

        void Start()
        {
            GetFilmList();
        }

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
        }

        void PopulateFilmList()
        {
            // Remove old gameobjects if updating
            if (filmEntries.Count > 0)
            {
                for(int i = 0; i < filmEntries.Count; i++)
                {
                    Destroy(filmEntries[i]);
                }
                filmEntries.Clear();
            }

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
            }
        }

        public void UpdateEntries()
        {

        }
    }
}

