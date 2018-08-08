using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Purpose:
	* automatic run (running right)
	* each of the 2 buttons will be to switch lanes in either direction (IF there is a lane to switch to)*/

public class Move : MonoBehaviour
{
    public AudioSource changeLaneSound;

    //vars set by manager
    [HideInInspector]
    public KeyCode[] keys;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float laneWidth;

    int laneNum;

    Vector2 velocity;

    [HideInInspector]
    public bool inCoRoutine;

    void Start()
    {
        inCoRoutine = false;

        //2,1,0 (top to bottom)
        laneNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(keys != null) //make sure manager has assigned our values
        {
            velocity = new Vector2(speed, 0);
            basicChangeLanges();

            // Entity will always move right
            transform.position = transform.position + ((Vector3)velocity * Time.deltaTime);
        }
    }

    // This sets the correct keys for each player/entity
    void basicChangeLanges()
    {
        if(Input.GetKey(keys[0]) || Input.GetKey(keys[1]))
        {
            if (Input.GetKey(keys[0]) && Input.GetKey(keys[1]))
                ; //both keys are pressed do nothing
            else
            {
                changeLaneSound.Play();

                Vector3 pos = transform.position;
                //one key is pressed... the player has made a conscious decision
                if (Input.GetKeyDown(keys[0]) && laneNum < 2) //move left / up
                {
                    transform.position = new Vector3(pos.x, pos.y + laneWidth, pos.z);
                    laneNum++;
                }
                if (Input.GetKeyDown(keys[1]) && laneNum > 0) //move right / down
                {
                    transform.position = new Vector3(pos.x, pos.y - laneWidth, pos.z);
                    laneNum--;
                }
            }
        }
    }
}