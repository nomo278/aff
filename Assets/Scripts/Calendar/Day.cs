using UnityEngine;
using UnityEngine.UI;
using System;

namespace Calendars
{
    public class Day : MonoBehaviour
    {
        public int day;
        public DayOfWeek dayOfWeek;
        public Text text;
        public Image highlight;

        Button button;

        public void SetOnClick(Action action)
        {
            if (button == null)
                button = highlight.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action());
        }

        public void SetHighlighted(bool highlighted)
        {
            highlight.gameObject.SetActive(highlighted);
        }
    }
}

