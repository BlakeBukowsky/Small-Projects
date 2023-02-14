using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : SmartMover
{
    public GameObject spawn;
    public float spawnCooldown;
    public string[] spawnFor;
    public float spawnDistance;
    float tilSpawn;
    void Start()
    {
        EntityStart();
        tilSpawn = spawnCooldown;
    }

    void Update()
    {
        EntityUpdate();
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        tilSpawn -= Time.deltaTime;
        Collider2D[] seen = Physics2D.OverlapCircleAll(pos, spawnDistance);
        List<Collider2D> seenTargets = new List<Collider2D>();
        for (int i = 0; i < seen.Length; i++) //Find targets in sight
        {
            if(seen[i].gameObject.transform != transform && seen[i].CompareTag("Entity"))
            {
                for (int a = 0; a < spawnFor.Length; a++)
                {
                    if(seen[i].gameObject.GetComponent<Entity>().name == spawnFor[a])
                    {
                        if(tilSpawn < 0)
                        {
                            tilSpawn = spawnCooldown;
                            float spawnDirection = Mathf.Atan2(seen[i].transform.position.y - transform.position.y, seen[i].transform.position.x - transform.position.x);
                            Instantiate(spawn, transform.position, Quaternion.Euler(0, 0, spawnDirection + Mathf.PI/4));
                            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                    }
                }
            }
        };
    }
}
