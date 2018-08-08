using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace someNamespace
{
    public class cone : MonoBehaviour
    {
        public AudioSource truckRunOver;
        public AudioSource dogRunOver;

        //when the package collides with our hero
        void OnTriggerEnter2D(Collider2D other) //the ONLY thing with a collider is our dog
        {
            if (other.gameObject.name == "cat")
            {
                truckRunOver.Play(); //play the sound of truck hitting cone

                //cat stuff
                gameObject.GetComponent<Animator>().SetBool("ranOver", true); //play the animation of the cone being ran over
                Camera.main.gameObject.GetComponent<Packages>().SpawnObject(); //cause a package to drop from the truck
                Camera.main.GetComponent<Manager>().villain.GetComponent<CatStun>().catStun(); //stun the cat with animation and sound

                //dont collide twice
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else if(other.gameObject.name == "dog" && Camera.main.GetComponent<Manager>().packagesAffectDog)
            {
                dogRunOver.Play();

                Camera.main.GetComponent<Packages>().decSpeed();

                //dont collide twice
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>()); //decrease speed of hero
            }
        }
    }
}