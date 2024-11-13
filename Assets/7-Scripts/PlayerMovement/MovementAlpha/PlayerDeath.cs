using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private float score;
    public void Die()
    {
        score = GetComponent<PlayerMovement1>().score;
        Debug.Log("Player has been caught");

        UiManager.instance.SwitchtoMode(1);
        UiManager.instance.UpdateGameOverScore(score);

        if (PlayerPrefs.HasKey("BestScore"))
        {
            if(PlayerPrefs.GetInt("BestScore") < Mathf.RoundToInt(score))
                PlayerPrefs.SetInt("BestScore",Mathf.RoundToInt(score));
        }
        else
            PlayerPrefs.SetInt("BestScore", Mathf.RoundToInt(score));
        UiManager.instance.UpdateBestScore(PlayerPrefs.GetInt("BestScore"));
        PlayerPrefs.Save();
        Time.timeScale = 0;
    }
}