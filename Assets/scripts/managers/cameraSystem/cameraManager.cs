using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour {

    public GameObject runner;
    public GameObject chaser;
    public GameObject mainCamera;

    public GameObject defaultBack;

    [Range(.1f,10)]
    public float divVar;

    //---Require Readjustment
    public float minEdgeX;
    float[] minEdgeXY; //so the runner has some time to react

    //---other vars
    bool screenSizeChanges;
    float[] screenLimit;
    float[] screenExtentXY;
    float[] runnerExtentXY;
    float[] chaserExtentXY;
    float[] maxDistBetweenXY;

    float lerpSpeed;

    //--------------------MonoBehavior Scripts--------------------

    void Start()
    {
        lerpSpeed = 1f; //1 is instant

        divVar = 2;

        screenSizeChanges = true;

        minEdgeX = 3;
        minEdgeXY = new float[2] {3,3};

        screenLimit = getScreenLimits();
        screenExtentXY = getScreenExtents();


        runnerExtentXY = getSpriteExtents(runner);
        chaserExtentXY = getSpriteExtents(chaser);
        maxDistBetweenXY = getMaxDistances();
    }

    void Update()
    {
        minEdgeXY[0] = minEdgeX;

        if (screenSizeChanges)
        {
            screenLimit = getScreenLimits();
            screenExtentXY = getScreenExtents();
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, calcNewCamDestination(), lerpSpeed);
        defaultBack.transform.position = new Vector3(mainCamera.transform.position.x, defaultBack.transform.position.y, defaultBack.transform.position.z);
    }

    //----------Other Scripts----------

    float[] getScreenLimits()
    {
        float camZ = mainCamera.transform.position.z;

        Vector2 screenUpperRight = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camZ));
        Vector2 screenLowerLeft = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, 0, camZ));

        //n,e,s,w -corresponds to indices- 0,1,2,3
        return new float[4] { screenUpperRight.y, screenUpperRight.x, screenLowerLeft.y, screenLowerLeft.x };
    }

    float[] getScreenExtents()
    {
        return new float[2] { (screenLimit[1] - screenLimit[3]) / 2, (screenLimit[0] - screenLimit[2]) / 2 };
    }

    float[] getSpriteExtents(GameObject GO)
    {
        return new float[2] { GO.GetComponent<SpriteRenderer>().bounds.extents.x, GO.GetComponent<SpriteRenderer>().bounds.extents.y};
    }

    //---

    //TODO... update this...
    float[] getMaxDistances() //will return 2 POSITIVE distances
    {
        //x max distance (moving right)
        float screenWidth = screenExtentXY[0] * 2;
        screenWidth = screenWidth - (chaserExtentXY[0] * 2) - (runnerExtentXY[0] * 2) - minEdgeXY[0];


        float screenHeight = screenExtentXY[1] * 2;
        screenHeight = screenHeight - (chaserExtentXY[1] * 2) - (runnerExtentXY[1] * 2) - minEdgeXY[1];

        return new float[2] { screenWidth, screenHeight};
    }

    Vector3 calcNewCamDestination()
    {
        bool movingVertically = (runner.GetComponent<TESThelper>().heading.y != 0);
        Vector2 distanceBetweenXY = runner.transform.position - chaser.transform.position; //with no adjustments

        Vector3 newCamPos = new Vector3(0, 0, mainCamera.transform.position.z);
        if (movingVertically)
        {
            /*
            newCamPos.x = (runner.transform.position.x + chaser.transform.position.x) / 2; //the health bar is vertical... use simple average for horizontal
            //TODO... remove this extra adjustment once we can move in all directions
            newCamPos.x += 2.5f;

            if (runner.GetComponent<TESThelper>().heading.y > 0) //we are moving towards the top
            {
                newCamPos.y = (runner.transform.position.y - screenExtentXY[1]); //our car's middle is at the very right edge of the screen
                newCamPos.y += runnerExtentXY[1]; //the front of our car is at the very right edge of the screen
                newCamPos.y += minEdgeXY[1]; //the car gets a little extra space because they have to be able to see what's ahead of them

                if (Mathf.Abs(distanceBetweenXY[1]) < maxDistBetweenXY[1]) //the chaser is gaining... we get a bit of a perk
                    newCamPos.y = newCamPos.y + ((maxDistBetweenXY[1] - Mathf.Abs(distanceBetweenXY[1])) / divVar);
            }
            else //we are moving towards the bottom
            {
                newCamPos.y = (runner.transform.position.y + screenExtentXY[1]); //our car's middle is at the very right edge of the screen
                newCamPos.y -= runnerExtentXY[1]; //the front of our car is at the very right edge of the screen
                newCamPos.y -= minEdgeXY[1]; //the car gets a little extra space because they have to be able to see what's ahead of them

                if (Mathf.Abs(distanceBetweenXY[1]) < maxDistBetweenXY[1]) //the chaser is gaining... we get a bit of a perk
                    newCamPos.y = newCamPos.y - ((maxDistBetweenXY[1] - Mathf.Abs(distanceBetweenXY[1])) / divVar);
            }
            */
        }
        else
        {
            //newCamPos.y = (runner.transform.position.y + chaser.transform.position.y) / 2; //the health bar is horizontal... use simple average for vertical
            //TODO... remove this extra adjustment once we can move in all directions
            //newCamPos.y += 2.5f;

            //??????????!!!!!!!!!!??????????!!!!!!!!!!??????????
            if (runner.GetComponent<TESThelper>().heading.x > 0) //we are moving towards right
            {
                newCamPos.x = (runner.transform.position.x - screenExtentXY[0]); //our car's TAIL is at the very right edge of the screen
                
                newCamPos.x += (runnerExtentXY[0]*2); //the FRONT of our car is at the very right edge of the screen
                newCamPos.x += minEdgeXY[0]; //the car gets a little extra space because they have to be able to see what's ahead of them
       
                if (Mathf.Abs(distanceBetweenXY[0]) < maxDistBetweenXY[0]) //the chaser is gaining... we get a bit of a perk
                    newCamPos.x = newCamPos.x + ((maxDistBetweenXY[0] - Mathf.Abs(distanceBetweenXY[0])) / divVar);
            }
            else //we are moving towards left
            {
                /*
                newCamPos.x = (runner.transform.position.x + screenExtentXY[0]); //our car's middle is at the very right edge of the screen
                newCamPos.x -= runnerExtentXY[0]; //the front of our car is at the very right edge of the screen
                newCamPos.x -= minEdgeXY[0]; //the car gets a little extra space because they have to be able to see what's ahead of them

                if (Mathf.Abs(distanceBetweenXY[0]) < maxDistBetweenXY[0]) //the chaser is gaining... we get a bit of a perk
                    newCamPos.x = newCamPos.x - ((maxDistBetweenXY[0] - Mathf.Abs(distanceBetweenXY[0])) / divVar);
                    */
            }
        }

        return newCamPos;
    }
}
