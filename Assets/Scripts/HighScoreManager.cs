using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField]
    Text t;
    private void Start()
    {
        t.text += GetHighScore();
    }

    static public int GetHighScore() 
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
           return 0;
        }
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    static public void SetHighscore(int s) 
    {
            PlayerPrefs.SetInt("HighScore", s);
    }
}
