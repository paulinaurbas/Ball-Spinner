﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerateObstacles : IGenerator
{
    private static readonly GenerateObstacles _instance = new GenerateObstacles();
    public GameObject cylinder { get; set; }
    public Bounds cylinderBounds { get; set; }
    public float radius { get; set; }
    public List<GameObject> listOfInstances { get; set; }
    public int numberOfInstances { get; set; }
    public GameObject baseObject { get; set; }
    public Destroyer destroyer { get; set; }
    private GenerateObstacles()
    {
        listOfInstances = new List<GameObject>();
    }
    public IGenerator GetInstance()
    {
        return Instance;
    }

    public static GenerateObstacles Instance
    {
        get { return _instance; }
    }


    public void InitializeGameObjects()
    {
        cylinderBounds = cylinder.GetComponent<MeshCollider>().bounds;
        radius = (cylinderBounds.max.x - cylinderBounds.min.x) / 2;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < numberOfInstances; i++)
        {
            bool obstacleIntersects = false;

            float angle = UnityEngine.Random.Range(0, 360);
            float posZ = UnityEngine.Random.Range(cylinderBounds.min.z, cylinderBounds.max.z);
            float posX = radius * (float)Math.Cos(angle);
            float posY = radius * (float)Math.Sin(angle);
            float size = UnityEngine.Random.Range(1, 5);

            var newObstacle = GameObject.Instantiate(baseObject, new Vector3(posX, posY, posZ), Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)));
            newObstacle.transform.localScale = new Vector3(size, size, size);
            newObstacle.transform.parent = cylinder.transform;
            newObstacle.name = "obstacleCube_" + i.ToString();


            //Prevent cubes from spawning inside eachother

            if (player.GetComponent<CapsuleCollider>().bounds.Intersects(newObstacle.GetComponent<BoxCollider>().bounds))
            {
                destroyer.DestroyObject(newObstacle);
                obstacleIntersects = true;
            }


            if (obstacleIntersects == false)
                listOfInstances.Add(newObstacle);
        }
    }
}