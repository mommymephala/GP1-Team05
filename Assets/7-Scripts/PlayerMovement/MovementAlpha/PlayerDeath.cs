using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public void Die()
    {
        Debug.Log("Player has been caught");

        UiManager.instance.SwitchtoMode(1);
        Time.timeScale = 0;
    }
}