using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticesTest : MonoBehaviour
{
    
    public MeshFilter meshFilter;
    public Vector3[] vertices;

    public GameObject testPrefab;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        vertices = meshFilter.mesh.vertices;
        

        foreach (var vert in vertices)
        {
            Instantiate(testPrefab, vert, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
