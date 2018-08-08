using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESThelper : MonoBehaviour {

    Vector3 currPosition;
    Vector3 prevPosition;

    [HideInInspector]
    public Vector3 heading; //this should instead be replaced by the vector that is constantly pushing us in one particular direction

    void Start()
    {
        heading = new Vector2(1, 0); //heading right
    }

    // Update is called once per frame
    void Update()
    {
        currPosition = gameObject.transform.position;
        Vector3 newHeading = (currPosition - prevPosition).normalized;
        newHeading.y = 0; //TODO.... modify this when adding  vertical movement if we ever decie to add it
        if (newHeading != Vector3.zero) //in reality we will NEVER stop moving so over TEST heading must reflect this
            heading = newHeading;
        prevPosition = currPosition;
    }
}
