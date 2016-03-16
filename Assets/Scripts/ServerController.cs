using UnityEngine;
using System;
using System.Collections;

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
}
