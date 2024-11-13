using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectSpawner : MonoBehaviour
{
    public List<GameObject> possibleObstacles;
    public float chanceToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.value > 1-chanceToSpawn/100)
            Instantiate(possibleObstacles[Random.Range(0, possibleObstacles.Count)], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
