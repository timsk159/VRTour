using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class that moves an object back-and-forth along the x axis.
//Has a maximum speed in both directions, and overshoots the target smoothly before heading back.
public class CodeReview : MonoBehaviour
{
    //Renamed all fields to proper coding conventions

    //Changed these public fields to private, but serialized for inspector editing
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float accel = 0.1f;

    private float currentAccel;

    //Now private
    private float currentSpeed = 0;

    //Now private
    private Vector3 initialPos;

    private Vector3 targetPos = new Vector3(100f, 0f, 0f);

    //Changed all lifecycle methods to private

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        //Had an uneeded if check here (caused a bug meaning we would never turn around).

        currentSpeed += currentAccel;
        if (currentSpeed > speed)
        {
            currentSpeed = speed;
        }
        if(currentSpeed < -speed)
        {
            currentSpeed = -speed;
        }
        transform.position = new Vector3(transform.position.x + currentSpeed,
            transform.position.y, transform.position.z);
    }

    //Change direction once we reach the target
    private void FixedUpdate()
    {
        //We only care about the x axis
        //Also seperated desired total acceleration from the current acceleration. 
        //Safer, and allows tweaking of the value at runtime.
        if(transform.position.x <= initialPos.x)
        {
            currentAccel = accel;
        }
        else if(transform.position.x >= (initialPos + targetPos).x)
        {
            currentAccel = -accel;
        }
    }
}
