using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
     
    public static UiManager instance;
    public List<GameObject> modes;

    private bool isPaused = false;
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
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            SwitchtoMode(2); 
        }
        else
        {
            SwitchtoMode(0);
        }
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
        Time.timeScale = 1;
    }
}
