using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

namespace Film
{
    public class FilmEditor : Editor
    {

        [CustomEditor(typeof(FilmFilter))]
        [CanEditMultipleObjects]
        public class WeekEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("Setup Properties"))
                {
                    FilmFilter filmFilter = (FilmFilter)target;

                    RectTransform rt = filmFilter.GetComponent<RectTransform>();
                    for(int i = 0; i < 3; i++)
                    {
                        for(int j = 0; j < 5; j++)
                        {
                            FilterSelection fs = rt.GetChild(i).GetChild(j).GetComponent<FilterSelection>();
                            fs.text = fs.GetComponent<RectTransform>().GetChild(0).GetChild(0).GetComponent<Text>();
                            fs.toggle = fs.GetComponent<RectTransform>().GetChild(0).GetChild(1).GetComponent<Toggle>();
                        }
                    }
                }

            }
        }
    }
}

