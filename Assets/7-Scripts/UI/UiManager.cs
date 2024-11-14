using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
     
    public static UiManager instance;
    public List<GameObject> modes;
    public GameObject SettingsUI;
    public GameObject CreditsUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI bestScoreText;
    public Image volumeImage;
    
    
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

        if (SceneManager.GetActiveScene().buildIndex == 1)
            AudioManager.Instance.PlayMusic("MainMenuMusic");
        
        
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

    public void Settings()
    {
        SettingsUI.SetActive(true);
    }

    public void Back()
    {
        SettingsUI.SetActive(false);
        CreditsUI.SetActive(false);
    }

    public void Credits()
    {
        CreditsUI.SetActive(true);
    }

    public void UpdateScore(float score)
    {
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }

    public void UpdateGameOverScore(float score)
    {
        gameOverText.text = $"Score : {Mathf.RoundToInt(score).ToString()}";
    }

    public void UpdateBestScore(float score)
    {
        bestScoreText.text = $"Best Score : {Mathf.RoundToInt(score).ToString()}";
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeImage.fillAmount);
        PlayerPrefs.Save();
        AudioManager.Instance.SetVolume();
    }
    
}
