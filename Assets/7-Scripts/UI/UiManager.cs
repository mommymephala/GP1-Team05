using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
     
    public static UiManager instance;
    public List<GameObject> modes;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI bestScoreText;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    void Start()
    {
        SwitchtoMode(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwitchtoMode(int mode)
    {
        foreach (var UiMode in modes)
        {
            UiMode.SetActive(false);
        }
        modes[mode].SetActive(true);
        print("ModeSwitched");
    }
    
    public void Play()
    {
        SceneManager.LoadScene("BetaShowcase");
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateScore(float score)
    {
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }

    public void UpdateGameOverScore(float score)
    {
        gameOverScoreText.text = $"Score : {Mathf.RoundToInt(score).ToString()}";
    }
    public void UpdateBestScore(float score)
    {
        bestScoreText.text = $"Best Score : {Mathf.RoundToInt(score).ToString()}";
    }
}
