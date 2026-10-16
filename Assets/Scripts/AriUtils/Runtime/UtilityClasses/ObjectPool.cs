/*****************************************************************************
// File Name : ObjectPool.cs
// Author : Arcadia Koederitz
// Creation Date : 5/31/2026
// Last Modified : 5/31/2026
//
// Brief Description : Generic implementation of the object pooling pattern.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace AriUtils
{
    [System.Serializable]
    public class ObjectPool<T> where T : Component
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform parentTransform;

        private Queue<T> objectPool = new Queue<T>();

        public ObjectPool(T prefabReference, Transform parentTransform)
        {
            this.prefab = prefabReference;
            this.parentTransform = parentTransform;
        }

        /// <summary>
        /// Gets an object from the object pool.
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            objectPool ??= new Queue<T>();
            T obj = objectPool.Count > 0 ? objectPool.Dequeue() : GameObject.Instantiate(prefab, parentTransform);
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns an unused object to the pool
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

}