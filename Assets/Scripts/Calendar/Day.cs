using UnityEngine;
using UnityEngine.UI;
using System;

namespace Calendar
{
    public class Day : MonoBehaviour
    {
        public int day;
        public DayOfWeek dayOfWeek;
        public Text _text;
        public Text text
        {
            get
            {
                if(_text == null)
                {
                    _text = GetComponent<Text>();
                }
                return _text;
            }
        }
        void Awake()
        {
            day = 1;
            _text = GetComponentInChildren<Text>();
        }
    }
}

