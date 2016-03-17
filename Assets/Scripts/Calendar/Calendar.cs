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
        public static DateTime now;

        DateTime currentMonth;

        public NavigationBar navBar;

        // Use this for initialization
        void Start()
        {
            weeks = GetComponentsInChildren<Week>();

            now = DateTime.Now;
            currentMonth = new DateTime(now.Year, now.Month, 1);

            SetupCalendarMonth(currentMonth);
        }

        void SetupCalendarMonth(DateTime month)
        {
            string month_name = month.ToString("MMMM yyyy");
            navBar.monthYear.text = month_name;

            // First day of this month
            DateTime first = new DateTime(month.Year, month.Month, 1);
            // first = first.AddMonths(-1);

            // Last day of previous month
            DateTime last = first.AddMonths(-1);

            // First day of next month
            DateTime next = first.AddMonths(1);

            int lastDayOfMonth = DateTime.DaysInMonth(first.Year, first.Month);
            int currWeek = 0;
            // Population calendar with current month
            for (int i = 1; i < lastDayOfMonth + 1; i++)
            {
                DayOfWeek dayOfWeek = first.DayOfWeek;
                if (i == 1)
                {
                    // Process first week
                    int day = Week.GetIntByDayOfWeek(dayOfWeek);
                    if (day > 0)
                    {
                        DayOfWeek dayOfWeekLastMonth = dayOfWeek - 1;
                        int daysInLastMonth = DateTime.DaysInMonth(last.Year, last.Month);
                        for (int j = day; j > 0; j--)
                        {
                            weeks[currWeek].SetDay(dayOfWeekLastMonth, daysInLastMonth);
                            weeks[currWeek].GetDay(dayOfWeekLastMonth).text.color = Color.gray;
                            daysInLastMonth--;
                            dayOfWeekLastMonth--;
                        }
                    }
                }

                if(currWeek < weeks.Length)
                {
                    weeks[currWeek].SetDay(dayOfWeek, first.Day);
                    weeks[currWeek].GetDay(dayOfWeek).text.font = currentMonthNormal;
                    weeks[currWeek].GetDay(dayOfWeek).text.color = Color.white;
                    weeks[currWeek].GetDay(dayOfWeek).highlight.gameObject.SetActive(false);

                    if (i == now.Day)
                    {
                        if (month.Month == now.Month)
                        {
                            Debug.Log("did this");
                            // Set font to bold for current day
                            weeks[currWeek].GetDay(dayOfWeek).text.font = currentMonthBold;
                            weeks[currWeek].GetDay(dayOfWeek).highlight.gameObject.SetActive(true);
                        }
                    }
                }

                first = first.AddDays(1);

                // Process last week
                if (i == lastDayOfMonth && currWeek < weeks.Length)
                {
                    if (dayOfWeek != DayOfWeek.Saturday)
                    {
                        while (first.DayOfWeek != DayOfWeek.Saturday)
                        {
                            weeks[currWeek].SetDay(first.DayOfWeek, first.Day);
                            weeks[currWeek].GetDay(first.DayOfWeek).text.color = Color.gray;
                            first = first.AddDays(1);
                            if (first.DayOfWeek == DayOfWeek.Saturday)
                            {
                                weeks[currWeek].SetDay(first.DayOfWeek, first.Day);
                                weeks[currWeek].GetDay(first.DayOfWeek).text.color = Color.gray;
                            }
                        }
                    }
                }
                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    currWeek++;
                }
            }
        }

        public void SetDayHighlight(int day)
        {
            DateTime first = new DateTime(now.Year, now.Month, 1);
            int firstDayOfMonth = first.Day;
            int lastDayOfMonth = DateTime.DaysInMonth(first.Year, first.Month);

            if(day >= firstDayOfMonth && day <= lastDayOfMonth)
            {
                int currWeek = 0;
                for (int i = 1; i < lastDayOfMonth + 1; i++)
                {
                    DayOfWeek dayOfWeek = first.DayOfWeek;
                    if(i == day)
                    {
                        weeks[currWeek].GetDay(dayOfWeek).highlight.gameObject.SetActive(true);
                        return;
                    }
                    first = first.AddDays(1);
                    if (dayOfWeek == DayOfWeek.Saturday)
                    {
                        currWeek++;
                    }
                }
            }
            Debug.Log("day didn't exist");
        }

        public void PrevMonth()
        {
            currentMonth = currentMonth.AddMonths(-1);
            SetupCalendarMonth(currentMonth);
        }

        public void NextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            SetupCalendarMonth(currentMonth);
        }
    }
}
