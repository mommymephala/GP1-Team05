using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
     
    public static UiManager instance;
    public List<GameObject> modes;
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
}
