using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Film
{
    public class FilmEntry : MonoBehaviour
    {
        public Text filmName, filmDirectors;
        public void SetFilmName(string name)
        {
            filmName.text = name;
        }
        public void SetDirectors(string directors)
        {
            filmDirectors.text = directors;
        }
    }
}

