using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Calendar
{
    public class Week : MonoBehaviour
    {
        public Text Sun, Mon, Tue, Wed, Thu, Fri, Sat;
        public Day[] days;

        public static Dictionary<int, DayOfWeek> intToDayOfWeek = new Dictionary<int, DayOfWeek>()
        {
            { 0, DayOfWeek.Sunday },
            { 1, DayOfWeek.Monday },
            { 2, DayOfWeek.Tuesday },
            { 3, DayOfWeek.Wednesday },
            { 4, DayOfWeek.Thursday },
            { 5, DayOfWeek.Friday },
            { 6, DayOfWeek.Saturday }
        };

        public static int GetIntByDayOfWeek(DayOfWeek dayOfWeek)
        {
            foreach(KeyValuePair<int, DayOfWeek> kvp in intToDayOfWeek)
            {
                if(kvp.Value == dayOfWeek)
                {
                    return kvp.Key;
                }
            }
            return 0;
        }

        void Start()
        {
            days = GetComponentsInChildren<Day>();
            for(int i = 0; i < days.Length; i++)
            {
                days[i].dayOfWeek = intToDayOfWeek[i];
                days[i].text.text = "";
            }
        }

        public void SetDay(DayOfWeek dayOfWeek, int num)
        {
            foreach(Day day in days)
            {
                if(day.dayOfWeek == dayOfWeek)
                {
                    day.text.text = num.ToString();
                    return;
                }
            }
        }

        public Day GetDay(DayOfWeek dayOfWeek)
        {
            foreach (Day day in days)
            {
                if (day.dayOfWeek == dayOfWeek)
                {
                    return day;
                }
            }
            return null;
        }
    }
}
