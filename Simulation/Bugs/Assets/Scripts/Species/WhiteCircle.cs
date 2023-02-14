using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCircle : SmartMover
{
    public float hitCooldown;
    public int damage;
    float tilHit;

    void Start()
    {
        SmartMoverStart();
        tilHit = hitCooldown;
    }

    void Update()
    {
        SmartMoverUpdate();
        tilHit -= Time.deltaTime;

    }
    void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Entity")
        {
            if(tilHit < 0)
            {
                collision.gameObject.GetComponent<Entity>().hp -= damage;
                tilHit = hitCooldown;
            }
        }
    }
}
