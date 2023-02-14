using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMover : Mover
{
    public bool isRandomDirection;
    public float startingDirection;
    float direction;
    void Start()
    {
        StraightMoverStart();
    }

    void Update()
    {
        StraightMoverUpdate();
    }

    void StraightMoverStart()
    {
        EntityStart();
        if(isRandomDirection)
        {
            direction = Random.value * 2 * Mathf.PI;
        }
        else
        {
            direction = startingDirection;
        }
    }

    void StraightMoverUpdate()
    {
        EntityUpdate();
        transform.position += Vector3.Normalize(new Vector3(Mathf.Cos(direction + transform.eulerAngles.z), Mathf.Sin(direction + transform.eulerAngles.z), 0)) * speed * Time.deltaTime;
    }
}
