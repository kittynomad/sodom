using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToSpawn;
    [SerializeField] private int _numToSpawn = 0;

    public void SpawnObjects(int quantity = -1)
    {
        int n = quantity == -1 ? _numToSpawn : quantity;

        foreach (GameObject g in _objectsToSpawn)
        {
            for (int i = 0; i < n; i++)
            {
                Instantiate(g, GetSpawnLocation(), Quaternion.identity);
            }
        }

    }

    private Vector2 GetSpawnLocation()
    {
        try
        {
            BoxCollider2D b = gameObject.GetComponent<BoxCollider2D>();
            float x = Random.Range(b.bounds.min.x, b.bounds.max.x);
            float y = Random.Range(b.bounds.min.y, b.bounds.max.y);

            return new Vector2(x, y);
        }
        catch
        {
            Debug.Log("Possibly no box collider, spawning exactly on spawner instead");
            return transform.position;
        }

    }

    public void SpawnObjectsOverTime(float timeBetween = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(ObjectSpawnTimer(timeBetween));
    }

    public IEnumerator ObjectSpawnTimer(float t)
    {
        for (int i = 0; i < _numToSpawn; i++)
        {
            foreach (GameObject g in _objectsToSpawn)
            {
                Instantiate(g, GetSpawnLocation(), Quaternion.identity);
            }
            yield return new WaitForSeconds(t);
        }
    }
}
