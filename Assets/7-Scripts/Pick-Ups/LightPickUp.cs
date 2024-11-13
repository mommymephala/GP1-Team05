using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickUp : MonoBehaviour
{
    public int charges;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("PickupChime");
            other.GetComponent<PlayerMovement1>().AddBoostCharge(charges);
            other.GetComponent<PlayerMovement1>().score += 500;
            UiManager.instance.UpdateScore(other.GetComponent<PlayerMovement1>().score);
            Destroy(gameObject);
        }
        
    }
}
