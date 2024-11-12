using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public LevelGenerator levelGenrator;
    public float generationThreshold;
    
    // Start is called before the first frame update
    void Start()
    {
        levelGenrator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(transform.position,levelGenrator.lastSocket.position)<generationThreshold)
            levelGenrator.GenerateTile();
        
        
    }
}
