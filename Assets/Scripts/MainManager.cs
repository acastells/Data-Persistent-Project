using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private HighScore HighScoreHistory;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(Application.persistentDataPath);

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        LoadHighScore();
        if (StartMenuManager.Instance)
        {
            ScoreText.text = $"{StartMenuManager.Instance.username}'s score : {m_Points}";
        }

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{StartMenuManager.Instance.username}'s score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (HighScoreHistory == null)
        {
            SaveHighScore();
            LoadHighScore();
        }
        else if (HighScoreHistory != null && HighScoreHistory.score < m_Points)
        {
            SaveHighScore();
            LoadHighScore();
        }

    }

    [System.Serializable]
    public class HighScore
    {
        public string username;
        public int score;
    }

    public void SaveHighScore()
    {
        HighScore data = new HighScore();
        data.username = StartMenuManager.Instance.username;
        data.score = m_Points;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        Text BestscoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore data = JsonUtility.FromJson<HighScore>(json);
            BestscoreText.text = $"Best Score : {data.username} : {data.score}";
            HighScoreHistory = data;
        }
        else
        {
            BestscoreText.text = $"No high score registered yet";
        }
    }
}
