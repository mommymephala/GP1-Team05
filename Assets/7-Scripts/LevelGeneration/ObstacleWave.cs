using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleWave : MonoBehaviour
{
    
    public GameObject obstacle;
    public int waveRows;
    public float spacing;
    public int[] possiblePoints = {-12,-10,-8,-6,-4,-2,0,2,4,6,8,10,12};
    public float obstacleSpeed;
    public int maxObstacleAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateObstacleRows(); 
        maxObstacleAmount = Mathf.Clamp(maxObstacleAmount, 0, possiblePoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0,-obstacleSpeed*Time.deltaTime));
    }

    public void GenerateObstacleRows()
    {
        float currentSpacing = 0f;
        for (int i = 0; i < waveRows; i++)
        {
            
            int row = Random.Range(1, maxObstacleAmount);
            List<int> selectedPoints = GetRandomPoints(row);
            for (int j = 0; j < selectedPoints.Count; j++)
            {
                Instantiate(obstacle, new Vector3(selectedPoints[j], 1f, currentSpacing)+LevelGenerator.instance.lastSocket.position, Quaternion.identity, gameObject.transform);

            }
            currentSpacing += spacing;
        }
    }


    public List<int> GetRandomPoints(int numberOfPoints)
    {
        List<int> points = possiblePoints.ToList();
        List<int> randomPoints = new List<int>();
        
        
        for (int i = 0; i < numberOfPoints; i++)
        {
            randomPoints.Add(points[Random.Range(0, points.Count)]);
            points.Remove(randomPoints[i]);
        }

        return randomPoints;
    }
    
    
    
}
