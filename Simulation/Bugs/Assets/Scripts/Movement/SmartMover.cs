using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartMover : Mover
{
    public string[] moveTo;
    public string[] moveFrom;
    public float sight;
    public bool search;
    public float searchTurnLength;
    public float searchTurnAmount;
    float tilTurn;
    bool isRight;
    float currentDirection;
    void Start()
    {
        SmartMoverStart();
    }

    void Update()
    {
        SmartMoverUpdate();
    }

    public void SmartMoverStart()
    {
        EntityStart();
        tilTurn = searchTurnLength;
        int willBeRight = Random.Range(0, 2);
        if(willBeRight == 1)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        currentDirection = Random.value * 2 * Mathf.PI;

    }
    public void SmartMoverUpdate()
    {
        EntityUpdate();
        tilTurn -= Time.deltaTime;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] seen = Physics2D.OverlapCircleAll(pos, sight);
        List<Collider2D> seenTargets = new List<Collider2D>();
        for (int i = 0; i < seen.Length; i++) //Find targets in sight
        {
            if(seen[i].gameObject.transform != transform && seen[i].CompareTag("Entity"))
            {
                for (int a = 0; a < moveTo.Length; a++)
                {
                    if(seen[i].gameObject.GetComponent<Entity>().name == moveTo[a])
                        seenTargets.Add(seen[i]);
                }
                for (int a = 0; a < moveFrom.Length; a++)
                {
                    if(seen[i].gameObject.GetComponent<Entity>().name == moveFrom[a])
                        seenTargets.Add(seen[i]);
                }
            }
        };
        float[] distance = new float[seenTargets.Count];
        for (int i = 0; i < seenTargets.Count; i++) //Calculates distances from targets in sight
        {
            distance[i] = Mathf.Sqrt(Mathf.Pow(seenTargets[i].gameObject.transform.position.x - transform.position.x, 2) + Mathf.Pow(seenTargets[i].gameObject.transform.position.y - transform.position.y, 2));
        }
        
        if(distance.Length > 0) //Determines if there is a target in sight
        {
            tilTurn = 0;
            GameObject closest = seenTargets[0].gameObject;
            float closestDistance = distance[0];
            for (int i = 1; i < seenTargets.Count; i++) //Finds clostest target in sight
            {
                if(distance[i] < closestDistance)
                {
                    closestDistance = distance[i];
                    closest = seenTargets[i].gameObject;
                }
            }
            Vector3 distanceFrom = Vector3.Normalize(new Vector3(closest.transform.position.x - transform.position.x, closest.transform.position.y - transform.position.y, 0f));
            bool isMoveTo = false;
            foreach (string item in moveTo)
            {   
                if (item == closest.GetComponent<Entity>().name)
                {
                    isMoveTo = true;
                    break;
                }
            }
            if(isMoveTo) //Determins if it should move to or away from the target
            {
                transform.position += distanceFrom * speed * Time.deltaTime;
            }
            else
            {
                transform.position -= distanceFrom * speed * Time.deltaTime;
            }
        }
        else if(search)
        {
            if(tilTurn <= 0)
            {
                int willBeRight = Random.Range(0, 2);
                if(willBeRight == 1)
                {
                    isRight = true;
                }
                else
                {
                    isRight = false;
                }
                tilTurn = searchTurnLength;
            }
            if(isRight)
            {
                currentDirection += searchTurnAmount * Time.deltaTime;
            }
            else
            {
                currentDirection -= searchTurnAmount * Time.deltaTime;
            }
            transform.position += Vector3.Normalize(new Vector3(Mathf.Cos(currentDirection), Mathf.Sin(currentDirection), 0)) * speed * Time.deltaTime;
        }
    }
}
