using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class ServerController : MonoBehaviour
{
    private static ServerController _instance;

    public static ServerController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ServerController>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
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

    public List<JSONNode> filmList = new List<JSONNode>();
    void CacheFilmList(JSONNode rootNode)
    {
        if (filmList.Count > 0)
            filmList.Clear();

        foreach (JSONNode jn in rootNode.AsArray)
        {
            filmList.Add(jn);
        }

    }
}
