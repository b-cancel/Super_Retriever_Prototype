using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpawner : MonoBehaviour {

    //everything is set to public for tweaking gameplay
    public GameObject ConePrefab; //the cone object

    public float spawnWait; //how long inbetween spawns
    public float spawnMostWait; //value for how long will be the max spawn rate time
    public float spawnLeastWait; //value for how short will be the least spawn rate time
    public float difficultyIncrease;

    [HideInInspector]
    public bool stop; // turn on or off manually

    int randCone; //number of the cone pattern for the "Cones" Array
    
    public GameObject spawner; //the actual spawner, used to move infront of the camera

    
    void Start () {
        stop = false;

        spawnLeastWait = .5f; //.375 crazy
        spawnMostWait = 1.25f; //1 getting crazy

        difficultyIncrease = .01f;

        //initializes the coroutine timer, for enmuerable stuff (which I didnt use anymore)
        StartCoroutine(WaitSpawner());
    }

    //enumerator helps with spawning rate
    IEnumerator WaitSpawner()
    {
        while(!stop)
        {
            //select a waitTime for the next cone to be spawned
            spawnWait = Random.Range(spawnLeastWait, spawnMostWait);

            //select a cone pattern at random

            //this with truncation will go from  0 to 5...(6 cases)
            randCone = ((int)Random.Range(0, 6)) + 1; //we dont want to be able to spawn in nothing so we shift by one an now our range is (1 to 6)

            //spawns a pattern of the cones
            ConePatterns(spawner.transform.position, randCone);

            //waits to spawn the next set within the while loop. uses public spawnWait time as the variable for tweaking
            yield return new WaitForSeconds(spawnWait);

            //make game harder as time passes
            spawnMostWait -= difficultyIncrease;
            spawnMostWait = (spawnMostWait < spawnLeastWait) ? spawnLeastWait : spawnMostWait;
        }
    }
    
    //used to spawn cones in patterns
    void ConePatterns(Vector3 spawnLocation, int randCase)
    {
        spawnLocation.z = 0;

        float laneSizeY = Camera.main.GetComponent<Manager>().laneWidth;
        float laneMiddleY = -2.75f;

        spawnLocation.y = laneMiddleY;

        switch(randCase)
        {
            //000 not allowed to happen

            //001
            case 1:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, -laneSizeY, 0), Quaternion.identity);
                break;

            //010
            case 2:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, 0, 0), Quaternion.identity);
                break;

            //011
            case 3:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, 0, 0), Quaternion.identity);
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, -laneSizeY, 0), Quaternion.identity);
                break;

            //100
            case 4:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, laneSizeY, 0), Quaternion.identity);
                break;

            //101
            case 5:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, laneSizeY, 0), Quaternion.identity);
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, -laneSizeY, 0), Quaternion.identity);
                break;

            //110
            case 6:
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, 0, 0), Quaternion.identity);
                Instantiate(ConePrefab, spawnLocation + new Vector3(0, laneSizeY, 0), Quaternion.identity);
                break;

            //111 not allowed to happen
        }
    }
}
