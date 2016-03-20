using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public static class Utility {
    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static void DeleteGameObjects(this List<GameObject> gameObjects)
    {
        if (gameObjects.Count > 0)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                UnityEngine.Object.Destroy(gameObjects[i]);
            }
            gameObjects.Clear();
        }
    }

    public static void PositionEntry(this RectTransform rt, int position, float height)
    {
        rt.anchoredPosition = new Vector2(0f, -position * height);
    }

    public static void FixOffsets(this RectTransform rt)
    {
        rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
        rt.offsetMax = new Vector2(1f, rt.offsetMax.y);
        rt.localScale = Vector3.one;
    }

    public static IEnumerator DelayedFunction(float waitTime, params Action[] actions)
    {
        yield return new WaitForSeconds(waitTime);
        foreach (Action action in actions)
        {
            action();
        }
    }

    public static int CompareJSONNodeShowtimes(JSONNode x, JSONNode y)
    {
        string x_showtime = x["Time-Slot"];
        string y_showtime = y["Time-Slot"];

        if (x_showtime != null && y_showtime != null)
        {
            DateTime x_time = DateTime.MaxValue;
            DateTime.TryParse(x_showtime, out x_time);
            DateTime y_time = DateTime.MaxValue;
            DateTime.TryParse(y_showtime, out y_time);
            return DateTime.Compare(x_time, y_time);
        }
        return -1;
    }
}
