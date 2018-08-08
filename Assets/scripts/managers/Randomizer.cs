using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace someNamespace
{
    //enumerable for easy to understand code
    public enum packageType { none, good, bad };

    public class Randomizer : MonoBehaviour
    {
        //references to 2 objects
        public GameObject van;
        public GameObject dog;

        //enable or disable randomizer
        [HideInInspector]
        public bool on = true;

        // Update is called once per frame
        void Update()
        {

            //TEST CODE: simulates another script calling this script
            if (Input.GetKeyDown(KeyCode.RightControl)) //TODO... remove TEST code
            {
                switch (dropPackage())
                {
                    case packageType.good: print("good package dropped"); break;
                    case packageType.bad: print("bad package dropped"); break;
                    case packageType.none: print("no package dropped"); break;
                }
            }
        }

        public packageType dropPackage() //returns true for good package, returns false for bad package
        {
            if (on)
            {
                //TODO... use Vector2.Distance()
                //the float variable that determines the distance between dog and van
                float distance = van.transform.position.x - dog.transform.position.x; // calculates the distance inbetween the caharacters 

                //TODO... replace this so the randomizer doesnt just work off of these 4 distances...
                // negative numbers are bad luck, 0 is good luck. The highest range number is not included
                int randomizer = 0; //the actual number that will be used to increase / decrease chances

                //ASSUME min distance is 0... for simplicity sake
                //TODO... get max distanceX from my cameraManager Script (that will be on the gameobject labeled camera)

                if (distance < 3.5)
                    randomizer = Random.Range(-7, 3);  //3 out of 10 chance to get a good package
                else if (3.5 <= distance && distance <= 10)
                    randomizer = Random.Range(-5, 5); //5 out of 10 chances to get a good package
                else if (10 < distance)
                    randomizer = Random.Range(-3, 7); //7 out of 10 chances to get a good package

                if (randomizer >= 0)
                    return packageType.good;
                else
                    return packageType.bad;
            }
            else
                return packageType.none;
        }
    }
}