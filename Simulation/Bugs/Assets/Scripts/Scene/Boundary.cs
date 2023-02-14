using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public bool isVertical;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!isVertical)
        {
            if(collider.gameObject.transform.position.x < 0)
                collider.gameObject.transform.position = new Vector3(-collider.gameObject.transform.position.x - .5f, collider.gameObject.transform.position.y, 0);
            else
                collider.gameObject.transform.position = new Vector3(-collider.gameObject.transform.position.x + .5f, collider.gameObject.transform.position.y, 0);
        }
        else
        {
            if(collider.gameObject.transform.position.y < 0)
                collider.gameObject.transform.position = new Vector3(collider.gameObject.transform.position.x, -collider.gameObject.transform.position.y - .5f, 0);
            else
                collider.gameObject.transform.position = new Vector3(collider.gameObject.transform.position.x, -collider.gameObject.transform.position.y + .5f, 0);
        }
    }
}
