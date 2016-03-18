using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace Calendars
{
    [CustomEditor(typeof(Day))]
    [CanEditMultipleObjects]
    public class CalendarEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Setup Properties"))
            {
                if(target != null)
                {
                    Day day = (Day)target;
                    RectTransform rt = day.transform.GetComponent<RectTransform>();
                    day.text = rt.GetChild(0).GetComponent<Text>();
                    day.highlight = rt.GetChild(1).GetChild(0).GetComponent<Image>();
                    RectTransform highlightParent = rt.GetChild(1).GetComponent<RectTransform>();
                    highlightParent.anchoredPosition = Vector2.zero;

                    // Weird swapping code since it doesn't re-render instantly
                    Vector2 original = day.highlight.GetComponent<RectTransform>().anchoredPosition;
                    day.highlight.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    day.highlight.GetComponent<RectTransform>().anchoredPosition = original;
                }

                if (targets.Length > 0)
                {
                    foreach (Object obj in targets)
                    {
                        Day day = (Day)obj;
                        RectTransform rt = day.transform.GetComponent<RectTransform>();
                        day.text = rt.GetChild(0).GetComponent<Text>();
                        day.highlight = rt.GetChild(1).GetChild(0).GetComponent<Image>();
                        RectTransform highlightParent = rt.GetChild(1).GetComponent<RectTransform>();
                        highlightParent.anchoredPosition = Vector2.zero;

                        // Weird swapping code since it doesn't re-render instantly
                        Vector2 original = day.highlight.GetComponent<RectTransform>().anchoredPosition;
                        day.highlight.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        day.highlight.GetComponent<RectTransform>().anchoredPosition = original;
                    }
                }
            }

        }
    }

    [CustomEditor(typeof(Week))]
    [CanEditMultipleObjects]
    public class WeekEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Setup Properties"))
            {
                if (target != null)
                {
                    Week week = (Week)target;
                    week.days = week.gameObject.GetComponentsInChildren<Day>();
                }

                if (targets.Length > 0)
                {
                    foreach (Object obj in targets)
                    {
                        Week week = (Week)obj;
                        week.days = week.gameObject.GetComponentsInChildren<Day>();
                    }
                }
            }

        }
    }
}
