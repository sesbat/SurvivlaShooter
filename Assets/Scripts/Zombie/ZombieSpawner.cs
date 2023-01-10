using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombies;
    public float delay;
    private void Start()
    {
       InvokeRepeating("CreateZombie", delay, delay);
    }

    public void CreateZombie()
    {
        Instantiate(zombies, transform.position, Quaternion.identity);
    }
}
