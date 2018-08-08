using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawns an object continuously with a different X value... same Y value
public class ContinuousSpawner : MonoBehaviour {

    public GameObject prefab;

    Queue<GameObject> ourGOs;

    Vector2 topRight;
    Vector2 bottomLeft;

    public GameObject passY;
    float prefab_valueY;
    float prefab_widthX;

    //NOTE: anything bigger than 1 will be used as buffer on left and right side
    float screenOfPrefabs; //how many screen of prefabs should be seen at any point
    float extrasOnSides;

    GameObject lastGO;

    void Awake()
    {
        screenOfPrefabs = 3;

        prefab_valueY = passY.transform.position.y;
        prefab_widthX = passY.GetComponent<SpriteRenderer>().bounds.size.x;
        Destroy(passY);

        calcScreenCorners();

        Vector2 bl_to_tr = topRight - bottomLeft;
        Vector2 screenSize = new Vector2(Mathf.Abs(bl_to_tr.x), Mathf.Abs(bl_to_tr.y)); //in world space

        //find out how many of the prefabs it takes to fill the screen
        int numberOfObjects = (int)((screenSize.x * screenOfPrefabs) / prefab_widthX) + 1; //to cover truncation issues

        //insert numberOfObjects into some data structure...
        GameObject[] goArr = new GameObject[numberOfObjects];

        extrasOnSides = (((screenOfPrefabs - 1) * screenSize.x) / 2);

        //insert from left tip of screen... onwards
        for (int i=0; i<goArr.Length; i++)
        {
            GameObject newGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newGO.transform.parent = this.gameObject.transform;

            //we need to maintain a reference to the last GO in the QUEUE
            lastGO = newGO;

            //position them based on the screen or the previous value
            if (i == 0)
            {
                float valueX = bottomLeft[0] - extrasOnSides;

                valueX += prefab_widthX;
                newGO.transform.position = new Vector2(valueX, prefab_valueY);
            }
            else
            {
                float valueX = goArr[i - 1].transform.position.x;

                valueX += prefab_widthX;
                newGO.transform.position = new Vector2(valueX, prefab_valueY);
            }

            goArr[i] = newGO;
        }

        ourGOs = new Queue<GameObject>();

        //insert into "Queue"
        for (int i=0; i<goArr.Length; i++)
            ourGOs.Enqueue(goArr[i]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        calcScreenCorners();

        //NOTE: the object FURTHEST from the left side of the screen will be LAST
        //NOTE: the object CLOSEST from the left side of the screen will be FIRST

        if (ourGOs.Peek().transform.position.x < (bottomLeft[0] + prefab_widthX - extrasOnSides))
        {
            //ORDER: first... others... last...

            //delete first
            Destroy(ourGOs.Peek());
            ourGOs.Dequeue();

            //create a new go
            GameObject newGO = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newGO.transform.parent = this.gameObject.transform;

            float valueX = lastGO.transform.position.x;
            valueX += prefab_widthX;
            newGO.transform.position = new Vector2(valueX, prefab_valueY);

            //update it as being last
            lastGO = newGO;

            //place it last
            ourGOs.Enqueue(newGO);
        }
        //ELSE... current prefab config is fine
    }

    void calcScreenCorners()
    {
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }
}