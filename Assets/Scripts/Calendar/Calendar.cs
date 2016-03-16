using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Calendar
{
    public class Calendar : MonoBehaviour
    {
        public Font currentMonthNormal, currentMonthBold, otherMonth;
        Week[] weeks;
        // Use this for initialization
        void Start()
        {
            DateTime now = DateTime.Now;

            // First day of this month
            DateTime first = new DateTime(now.Year, now.Month, 1);
            first = first.AddMonths(-1);

            // Last day of previous month
            DateTime last = first.AddMonths(-1);

            // First day of next month
            DateTime next = first.AddMonths(1);

            weeks = GetComponentsInChildren<Week>();

            int lastDayOfMonth = DateTime.DaysInMonth(first.Year,first.Month);
            int currWeek = 0;
            // Population calendar with current month
            for(int i = 1; i < lastDayOfMonth; i++)
            {
                DayOfWeek dayOfWeek = first.DayOfWeek;
                if(i == 1)
                {
                    // Process first week
                    int day = Week.GetIntByDayOfWeek(dayOfWeek);
                    if(day > 0)
                    {
                        DayOfWeek dayOfWeekLastMonth = dayOfWeek - 1;
                        int daysInLastMonth = DateTime.DaysInMonth(last.Year, last.Month);
                        for(int j = day; j > 0; j--)
                        {
                            weeks[currWeek].SetDay(dayOfWeekLastMonth, daysInLastMonth);
                            weeks[currWeek].GetDay(dayOfWeekLastMonth).text.color = Color.gray;
                            daysInLastMonth--;
                            dayOfWeekLastMonth--;
                        }
                    }
                }

                if(i == lastDayOfMonth-1)
                {
                    // TODO: Process last week
                    /*
                    int day = Week.GetIntByDayOfWeek(dayOfWeek);
                    if (day < 6)
                    {
                        DayOfWeek dayOfWeekLastMonth = dayOfWeek - 1;
                        int daysInNextMonth = DateTime.DaysInMonth(last.Year, last.Month);
                        for (int j = 0; j < 6; j++)
                        {
                            weeks[currWeek].SetDay(dayOfWeekLastMonth, daysInLastMonth);
                            weeks[currWeek].GetDay(dayOfWeekLastMonth).text.color = Color.gray;
                            daysInLastMonth--;
                            dayOfWeekLastMonth--;
                        }
                    } */
                }


                if (i == now.Day)
                {
                    // Set font to bold for current day
                    weeks[currWeek].GetDay(dayOfWeek).text.font = currentMonthBold;
                }

                weeks[currWeek].SetDay(dayOfWeek, first.Day);
                first = first.AddDays(1);
                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    currWeek++;
                }
            }
        }
    }
}
