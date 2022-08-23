using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static HighScoreManager;

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

    // Start is called before the first frame update
    void Start()
    {
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

        GetHighestScore();
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
        SaveHighScore();
        SceneManager.LoadScene("HighScore");
    }

    public void SaveHighScore()
    {
        HighScoreList score = new HighScoreList();
        score.LoadFromJson();
        score.AddNewScore(m_Points, StartMenuManager.Instance.username);
        string json = JsonUtility.ToJson(score);
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void GetHighestScore()
    {
        HighScoreList score = new HighScoreList();
        score.LoadFromJson();
        Text BestscoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        BestscoreText.text = $"Best Score : {score.username1} : {score.score1}";
    }
}
