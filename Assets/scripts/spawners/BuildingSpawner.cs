using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    //MANUALLY ADD THESE DEPENDING ON HOW MANY WE HAVE
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    public GameObject prefab8;
    public GameObject prefab9;
    public GameObject prefab10;
    public GameObject prefab11;
    public GameObject prefab12;
    List<GameObject> prefabList;

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
        //MANUALLY ADD THESE DEPENDING ON HOW MANY WE HAVE
        prefabList = new List<GameObject>();
        prefabList.Add(prefab1);
        prefabList.Add(prefab2);
        prefabList.Add(prefab3);
        prefabList.Add(prefab4);
        prefabList.Add(prefab5);
        prefabList.Add(prefab6);
        prefabList.Add(prefab7);
        prefabList.Add(prefab8);
        prefabList.Add(prefab9);
        prefabList.Add(prefab10);
        prefabList.Add(prefab11);
        prefabList.Add(prefab12);

        //-----Other

        screenOfPrefabs = 5;

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
        for (int i = 0; i < goArr.Length; i++)
        {
            GameObject newGO = Instantiate(randomPrefab(), Vector3.zero, Quaternion.identity);
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
        for (int i = 0; i < goArr.Length; i++)
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
            GameObject newGO = Instantiate(randomPrefab(), Vector3.zero, Quaternion.identity);
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

    GameObject randomPrefab()
    {
        //0 -> .99 = 0
        //lastIndex -> (prefabList.Count-.01) = lastIndex

        int prefabID = (int)Random.Range(0, prefabList.Count);
        //last Index -> prefabList.count = lastIndex
        prefabID = (prefabID > (prefabList.Count-1)) ? (prefabList.Count - 1) : prefabID;

        return prefabList[prefabID];
    }

    void calcScreenCorners()
    {
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }
}