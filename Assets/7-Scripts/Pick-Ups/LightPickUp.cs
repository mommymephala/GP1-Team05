using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickUp : MonoBehaviour
{
    public int charges;
    public GameObject particle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement1>().AddBoostCharge(charges);
            other.GetComponent<PlayerMovement1>().score += 500;
            UiManager.instance.UpdateScore(other.GetComponent<PlayerMovement1>().score);
            Destroy(gameObject);
        }
        
    }
}
