using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string name;
    public int hp;
    public float size;
    public Color color;
    void Start()
    {
        EntityStart();
    }

        void Update()
    {
        EntityUpdate();
    }

    public void EntityStart()
    {
        transform.localScale = new Vector3(size, size, 1);
        gameObject.GetComponent<SpriteRenderer>().color = color;
        gameObject.name = name;
        Debug.Log("Started " + name);
    }

    public void EntityUpdate()
    {
        if(hp <= 0)
        {
            Debug.Log("Destoyed " + name);
            Destroy(gameObject);
        }
    }
}
