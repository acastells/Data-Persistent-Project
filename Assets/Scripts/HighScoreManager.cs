using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{

    public TextMeshProUGUI textScore1;
    public TextMeshProUGUI textScore2;
    public TextMeshProUGUI textScore3;

    void Start()
    {
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class HighScoreList // Bad practice, we should be using NewtonSoft json utilities to wrap List and Dicts to Json. Native Unity can not do it!
    {
        public string username1 = "";
        public string username2 = "";
        public string username3 = "";
        public int score1 = 0;
        public int score2 = 0;
        public int score3 = 0;
        public Dictionary<int, string> sortedDictionary = new Dictionary<int, string>();

        public void AddNewScore(int score, string username)
        {
            if (!sortedDictionary.TryGetValue(score, out _))
            {
                sortedDictionary.Add(score, username);
                UpdateVars();
            }
        }

        public void UpdateVars()
        {
            int i = 1;
            foreach (var item in sortedDictionary.OrderBy(i => i.Key).Reverse())
            {
                if (i == 1)
                {
                    score1 = item.Key;
                    username1 = item.Value;
                }
                else if (i == 2)
                {
                    score2 = item.Key;
                    username2 = item.Value;
                }
                else if (i == 3)
                {
                    score3 = item.Key;
                    username3 = item.Value;
                }
                else
                {
                    return;
                }
                i++;
            }
        }

        public void FromVarsToObject()
        {
            AddNewScore(score1, username1);
            AddNewScore(score2, username2);
            AddNewScore(score3, username3);
        }

        public void LoadFromJson()
        {
            string path = Application.persistentDataPath + "/highscore.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                HighScoreList newListScores = JsonUtility.FromJson<HighScoreList>(json);
                username1 = newListScores.username1;
                username2 = newListScores.username2;
                username3 = newListScores.username3;
                score1 = newListScores.score1;
                score2 = newListScores.score2;
                score3 = newListScores.score3;
                FromVarsToObject();
            }
        }


    }


    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreList newListScores = JsonUtility.FromJson<HighScoreList>(json);
            newListScores.FromVarsToObject();
            textScore1.SetText(newListScores.score1 + " - " + newListScores.username1);
            textScore2.SetText(newListScores.score2 + " - " + newListScores.username2);
            textScore3.SetText(newListScores.score3 + " - " + newListScores.username3);
        }
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene("main");
    }

    public void RestartWithNewUsernameButtonPressed()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
