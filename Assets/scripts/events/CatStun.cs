using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStun : MonoBehaviour {

    public AudioSource catStunSound;

    public void catStun()
    {
        StartCoroutine(catStunCoR());
    }

    IEnumerator catStunCoR()
    {
        if (gameObject.GetComponent<Move>().inCoRoutine == false)
        {
            gameObject.GetComponent<Animator>().SetBool("inCoR", true); //start cat stun anim

            catStunSound.Play(); //play cat stun

            yield return new WaitForSeconds(.85f); //wait for cat stun anim to play

            gameObject.GetComponent<Animator>().SetBool("inCoR", false); //end cat stun anim
        }
    }
}
