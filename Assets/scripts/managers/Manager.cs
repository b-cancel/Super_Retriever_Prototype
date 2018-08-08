using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Purpose:
	* assigns keys to players based on saved data; -> I put this in Move.cs
	* check if either character has won ;
	* after brief delay so sounds can play out [use coroutines]*/

public class Manager : MonoBehaviour
{
    public GameObject backGroundMusic;

    //link references in inspector
    public GameObject hero;
    public GameObject villain;

    KeyCode[] heroKeys;
    KeyCode[] villainKeys;

    Vector2 topRight;
    Vector2 bottomLeft;
    
    [HideInInspector]
    public float laneWidth;

    public bool packagesAffectDog;

    //---timer code
    public GameObject Timer;
    public float time;
    public float startTime;

    //---speed code
    public float speed; //curr speed
    public float speedChangePerFrame;
    public float maxSpeed;

    //---pitch or tempo code
    public float tempoOrPitch;
    public float tempoChangePerFrame;
    public float maxTempo;

    // Use this for initialization
    void Start()
    {
        packagesAffectDog = true;

        tempoOrPitch = 1;

        //speed change code
        speedChangePerFrame = .005f;
        tempoChangePerFrame = .0001f;

        //max
        maxSpeed = 25;
        maxTempo = 2;

        //timer settings
        startTime = 90f;
        time = startTime;

        //set keycodes
        heroKeys = new KeyCode[] {KeyCode.W, KeyCode.S};
        villainKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow };

        //assign keycodes [ONCE]
        hero.GetComponent<Move>().keys = heroKeys;
        villain.GetComponent<Move>().keys = villainKeys;

        //assign other values [can be adjusted while runing]
        speed = 5;
        laneWidth = .85f;

        //pass this value to our children... 
        hero.GetComponent<Move>().speed = speed;
        villain.GetComponent<Move>().speed = speed;
        hero.GetComponent<Move>().laneWidth = laneWidth;
        villain.GetComponent<Move>().laneWidth = laneWidth;
    }

    void Update()
    {
        //---change speed slightly overtime
        if(speed < maxSpeed)
            speed += speedChangePerFrame;
        if(tempoOrPitch < maxTempo)
            tempoOrPitch += tempoChangePerFrame;

        backGroundMusic.GetComponent<AudioSource>().pitch = tempoOrPitch;

        //---pass over speed
        if (hero.GetComponent<Move>().inCoRoutine == false)
            hero.GetComponent<Move>().speed = speed;
        villain.GetComponent<Move>().speed = speed;

        //--get vars ready for win conditions

        bool heroWin = false;
        bool villainWin = false;

        //-----Timer

        time -= Time.deltaTime;
        Timer.GetComponent<Text>().text = (time + 1).ToString("0");
        if (time < 4.5f) //because the line above truncates and we dont want our timer = 6 to ever be red
            Timer.GetComponent<Text>().color = Color.red;

        //this is because the dog always has the upper hand because they always have space to dodge... 
        //the truck does not have this luxury
        if(time <= 0)
            villainWin = true;

        //-----manual win conditions

        if (Input.GetKey(KeyCode.Escape))
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) //1 or 2
            {
                if ((Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1)) == false) //only 1
                {
                    if (Input.GetKey(KeyCode.Mouse0)) //hero
                        heroWin = true;
                    else
                        villainWin = true;
                }
                //both
            }
            //none
        }

        //-----NON manual... win conditions

        if (checkForHeroWin() || heroWin) //this one is first because its less expensive
            SceneManager.LoadScene("dogWin", LoadSceneMode.Single);
        else if (checkForVillainWin() || villainWin)
            SceneManager.LoadScene("catWin", LoadSceneMode.Single);
    }

    // Hero wins when it catches up to villain
    bool checkForHeroWin()
    {
        if (getVillainLeftTipX() <= getHeroRightTipX())
            return true;
        else
            return false;
    }

    // Villain wins when hero is out of game screen
    bool checkForVillainWin()
    {
        if (getHeroRightTipX() <= getScreenLeftEdge())
            return true;
        else
            return false;
    }

    //---calcs we need before conditions

    float getHeroRightTipX()
    {
        //since the position of the dog is set now on its right tip (head) [using sprite settings]
        return hero.transform.position.x;
    }

    float getVillainLeftTipX()
    {
        //since the position of the van is set now on its left tip (tail) [using sprite settings]
        return villain.transform.position.x;
    }

    float getScreenLeftEdge()
    {
        calcScreenCorners();
        return bottomLeft[0];
    }

    //---helpers

    void calcScreenCorners()
    {
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }
}