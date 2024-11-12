using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public enum RandomMode
{
    Surface,
    Vertex
}

public class LevelComponent : MonoBehaviour
{

    public Transform socket;
    public GameObject tile;
    public GameObject plane;
    
    [Space(10)]
    [Header("Static Object Settings")]
    public List<GameObject> staticObjects;
    public bool allowStaticObjects;
    public RandomMode randomMode;
    public int staticObjectCount;
    public float minDistance;
    public bool randomizeStaticObjectCount;
    
    [Space(10)]
    [Header("Pick-Up Settings")]
    public bool allowPickUp;
    public GameObject pickUpPrefab;
    public int pickUpCount;  
    
    private MeshFilter _meshFilter;
    private Bounds _bounds;
    private int _maxObjectCount;
    private List<Vector3> _points;
    // Start is called before the first frame update

    public virtual void Awake()
    {
        _maxObjectCount = staticObjectCount;
        _meshFilter= plane.GetComponent<MeshFilter>();
        _bounds = _meshFilter.mesh.bounds;

        if (allowStaticObjects)
        {
            if(randomizeStaticObjectCount)
                staticObjectCount = Random.Range(0, _maxObjectCount+1);
            GenerateStaticObjects();
        }
        
        if(allowPickUp)
            SpawnPickUp();
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetRandomStaticObject()
    {
        return staticObjects[Random.Range(0, staticObjects.Count)];
    }


    public void GenerateStaticObjects()
    {
        List<GameObject> selectedObjects = new List<GameObject>();
        List<GameObject> spawnedObjects = new List<GameObject>();
        for(int i = 0; i < staticObjectCount; i++)
            selectedObjects.Add(GetRandomStaticObject());

        foreach (var staticObject in selectedObjects)
        {
            Vector3 position = new Vector3(); 

            switch (randomMode)
            {
                case RandomMode.Surface:
                    position = GetRandomPosition();
                    break;
                case RandomMode.Vertex:
                    position = GetRandomVertex();
                    break;
                default:
                   position = GetRandomPosition();
                    break;
            }
            
            
            var objectInstance = Instantiate(staticObject, position, Quaternion.identity,gameObject.transform);
            EvaluatePosition(spawnedObjects, objectInstance);
            
            spawnedObjects.Add(objectInstance);
        }
        
        
    }

    public void EvaluatePosition(List<GameObject> objects, GameObject newObject)
    {
        bool isValid = false;
        
        while (!isValid)
        {
            if(objects.Count == 0)
                break;
            
            foreach (var obj in objects)
            {
                isValid = true;
                if (Vector3.Distance(newObject.transform.position, obj.transform.position) < minDistance)
                {
                    newObject.transform.position = GetRandomPosition();
                    isValid = false;
                }
                    
            }
            
            
        }
    }

    public void SpawnPickUp()
    {
        List<GameObject> spawnedPickUp = new List<GameObject>();
        
        
        Vector3 position = new Vector3();
        for (int i = 0; i < pickUpCount; i++)
        {
            switch (randomMode)
            {
                case RandomMode.Surface:
                    position = GetRandomPosition();
                    break;
                case RandomMode.Vertex:
                    position = GetRandomVertex();
                    break;
                default:
                    position = GetRandomPosition();
                    break;
            }

            position.y += 1f;
            var objectInstance = Instantiate(pickUpPrefab, position, Quaternion.identity, gameObject.transform);
            spawnedPickUp.Add(objectInstance);
        }

        
    }
    
    

    public Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-plane.transform.localScale.x*10/2,plane.transform.localScale.x*10/2), Random.Range(0f,0.15f), Random.Range(0,plane.transform.localScale.z*10));
        position = gameObject.transform.position + position;
        return position;
    }

    public Vector3 GetRandomVertex()
    {
        List<Vector3> vertices = plane.GetComponent<MeshFilter>().mesh.vertices.ToList();
        
        Vector3 position = vertices[Random.Range(0, vertices.Count)];
        position = gameObject.transform.position + position;
        return position;

    }
    
    
}
