using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{

    public float timeBetSpawnMin = 10f;
    public float timeBetSpawnMax = 20f;
    private float timeBetSpawn=0f;
    private float lastSpawnTime=0f;

    public int repositionT=7;
    void Update()
    {
        
        if (transform.position.x >= repositionT)
            if (Time.time >= lastSpawnTime + timeBetSpawn)
            {
                Reposition();
                lastSpawnTime = Time.time;
                timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            }
              
    }
    private void Reposition()
    {
        Vector2 offset = new Vector2(repositionT*-2, transform.position.y);
        transform.position =offset;
    }
}
