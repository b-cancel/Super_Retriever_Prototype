using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace someNamespace
{
    public class Package : MonoBehaviour
    {
        GameObject cam;

        [HideInInspector]
        public packageType pT;

        void Awake()
        {
            cam = GameObject.Find("Camera");
        }

        //when the package collides with our hero
        void OnTriggerEnter2D(Collider2D other) //the ONLY thing with a collider is our dog
        {
            if (pT == packageType.good)
                cam.GetComponent<Packages>().incSpeed(); //speed of hero
            else
                cam.GetComponent<Packages>().decSpeed(); //speed of hero

            Destroy(this.gameObject); // destroy package
        }
    }
}