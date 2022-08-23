using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;

    public string username;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterGamePressed()
    {
        username = GameObject.Find("UsernameText").GetComponent<TextMeshProUGUI>().text;
        SceneManager.LoadScene("main");
    }
}
