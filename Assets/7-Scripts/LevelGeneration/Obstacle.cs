using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float velocityReduction = 5f;
    public float blinkDuration = 0.5f;
    public int blinkCount = 3;
    public GameObject hitParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerMovement1>(out var playerMovement))
            {
                playerMovement.GetHit();
                playerMovement.currentVelocity = Mathf.Max(playerMovement.defaultVelocity, playerMovement.currentVelocity - velocityReduction);
                playerMovement.StopAcceleration();
                
                StartCoroutine(BlinkEffect(other.gameObject));
                AudioManager.Instance.Play3DSoundOnObject("Monster1", gameObject);
            }
            
        }
    }

    private IEnumerator BlinkEffect(GameObject player)
    {
        Renderer playerRenderer = player.GetComponentInChildren<Renderer>();

        if (playerRenderer == null)
        {
            Debug.LogError("Player Renderer not found. TRYING SOMETHING");
            yield break;
        }

        for (int i = 0; i < blinkCount; i++)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(blinkDuration / (2 * blinkCount));

            playerRenderer.enabled = true;
            yield return new WaitForSeconds(blinkDuration / (2 * blinkCount));
        }
    }
}