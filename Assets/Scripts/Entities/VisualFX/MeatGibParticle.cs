using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class MeatGibParticle : MonoBehaviour
{
    [SerializeField] private GameObject _gibSplatFX;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        

        int num = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);

        for(int i = 0; i < num; i++)
        {
            Instantiate(_gibSplatFX, collisionEvents[i].intersection, Quaternion.identity);
        }
    }
}
