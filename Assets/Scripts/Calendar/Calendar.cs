using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

using Events;
using Maps;

namespace Calendars
{
    public class Calendar : MonoBehaviour
    {
        public Font currentMonthNormal, currentMonthBold, otherMonth;
        public Week[] weeks;
        public static DateTime now;

        public DateTime currentMonth;

        public NavigationBar navBar;

        public EventList eventList;
        
        void OnEnable()
        {
            now = DateTime.Now;
            currentMonth = new DateTime(now.Year, now.Month, 1);
            // SetupCalendarMonth(currentMonth);
        }

        public void SetupCalendarMonth(DateTime month)
        {
            string month_name = month.ToString("MMMM yyyy");
            navBar.monthYear.text = month_name;

            // First day of this month
            DateTime first = new DateTime(month.Year, month.Month, 1);

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
                    Day day = weeks[currWeek].GetDay(dayOfWeek);
                    weeks[currWeek].SetDay(dayOfWeek, first.Day);
                    day.day = first.Day;
                    day.text.font = currentMonthNormal;
                    day.text.color = Color.white;
                    day.highlight.gameObject.SetActive(false);

                    if (i == now.Day)
                    {
                        if (month.Month == now.Month)
                        {
                            // Set font to bold for current day
                            weeks[currWeek].GetDay(dayOfWeek).text.font = currentMonthBold;
                            // weeks[currWeek].GetDay(dayOfWeek).highlight.gameObject.SetActive(true);
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
            GetDay(day).SetHighlighted(true);
        }

        public Day GetDay(int date)
        {
            foreach(Week week in weeks)
            {
                foreach (Day day in week.days)
                {
                    if (day.day == date)
                    {
                        return day;
                    }
                }
            }
            return null;
        }

        public void ClearDayHighlights()
        {
            foreach (Week week in weeks)
            {
                foreach (Day day in week.days)
                {
                    day.SetHighlighted(false);
                }
            }
        }

        public void PrevMonth()
        {
            currentMonth = currentMonth.AddMonths(-1);
            SetupCalendarMonth(currentMonth);
            SetupEvents();
        }

        public void NextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            SetupCalendarMonth(currentMonth);
            SetupEvents();
        }

        public void SetupEvents()
        {
            eventList.PopulateCalendar(currentMonth, this);
        }
    }
}
