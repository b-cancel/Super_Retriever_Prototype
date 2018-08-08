using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroJump : MonoBehaviour {

    public float jumpSpeed = 10.0f;
    public bool grounded = true;
    public GameObject hero;

    public Rigidbody2D rb;


    // Use this for initialization
    void Start ()
    {
        rb = hero.GetComponent<Rigidbody2D>();  
    }
	
	// Update is called once per frame
	void Update () {

        OnTriggerEnter2D(hero.GetComponent<Collider2D>());

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (grounded == true)
        {
            rb.AddForce(Vector3.up * jumpSpeed);
            grounded = false;
        }
    }
}
