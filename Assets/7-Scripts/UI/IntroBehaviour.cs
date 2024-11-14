using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroBehaviour : MonoBehaviour
{
    public VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        player.loopPointReached += GoToMainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");
    }


    public void GoToMainMenu(VideoPlayer player)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
