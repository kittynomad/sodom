/*****************************************************************************
// File Name : SerializedInterface.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Generic class that allows for "serialization" of an object with a given interface.
*****************************************************************************/
using System;
using UnityEngine;

namespace AriUtils
{
    [Serializable]
    public abstract class SerializedInterface
    {
        public abstract Type InterfaceType { get; }
    }

    [Serializable]
    public class SerializedInterface<T> : SerializedInterface where T : class
    {
        [SerializeReference] private T _managedReferenceValue;
        [SerializeField] private UnityEngine.Object _objectReferenceValue;
        [SerializeField] private bool _referenceType; // True for managed reference, false for object reference

        public T Interface => _referenceType ? _managedReferenceValue : _objectReferenceValue as T;
        public override Type InterfaceType => typeof(T);

        public static implicit operator T(SerializedInterface<T> instance)
        {
            return instance.Interface;
        }
    }
}
