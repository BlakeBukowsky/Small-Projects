using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleTrianle : SmartMover
{
    public string[] convertable;
    public float convertCooldown;
    float tilConvert;

    void Start()
    {
        SmartMoverStart();
        tilConvert = convertCooldown;
    }

    void Update()
    {
        SmartMoverUpdate();
        tilConvert -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(tilConvert < 0)
        {
            if (collision.gameObject.tag == "Entity")
        {
            for (int i = 0; i < convertable.Length; i++)
            {
                if(collision.gameObject.GetComponent<Entity>().name == convertable[i])
                {
                    Vector3 pos = collision.gameObject.transform.position;
                    Quaternion rot = collision.gameObject.transform.rotation;
                    Destroy(collision.gameObject);
                    Instantiate(gameObject, pos, rot);
                    tilConvert = convertCooldown;
                    break;
                }
            }
        }
        }
    }
}
