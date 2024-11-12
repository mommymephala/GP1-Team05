using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public List<LevelComponent> tiles;
    public List<LevelComponent> spawnedTiles;
    public GameObject wavePrefab;
    public float timeBetweenSpawns;
    
    
    public Transform lastSocket;

    private Vector3 _Offset;
    private float _t; 
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        GenerateTile();
        StartCoroutine(GenerateWaveRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    
        
    }

    public LevelComponent GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }


    public void GenerateTile()
    {
        var tile = GetRandomTile();

        Vector3 spawnPoint = new Vector3();

        if (spawnedTiles.Count == 0)
            spawnPoint = Vector3.zero;
        else
        {
            spawnPoint = spawnedTiles[^1].socket.position;
            
        }
        
        var tileInstance = Instantiate(tile.tile, spawnPoint, Quaternion.identity);
        spawnedTiles.Add(tileInstance.GetComponent<LevelComponent>());
        lastSocket = spawnedTiles[^1].socket;
    }

    
    public void GenerateObstacleWave()
    {
        Instantiate(wavePrefab, lastSocket.position, Quaternion.identity);
    }


    public IEnumerator GenerateWaveRoutine()
    {
        yield return new WaitForSeconds(timeBetweenSpawns);
        GenerateObstacleWave();
        StartCoroutine(GenerateWaveRoutine());
    }


    
    
    
    

}