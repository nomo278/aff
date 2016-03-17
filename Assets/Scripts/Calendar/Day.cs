using UnityEngine;
using UnityEngine.UI;
using System;

namespace Calendar
{
    public class Day : MonoBehaviour
    {
        public int day;
        public DayOfWeek dayOfWeek;
        public Text text;
        public Image highlight;
    }
}

