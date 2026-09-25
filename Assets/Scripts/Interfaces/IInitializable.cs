/*****************************************************************************
// File Name : IInitializable.cs
// Author : Pierce Nunnelley
// Creation Date : February 11, 2026
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

public interface IInitializable
{
    public abstract bool HasFinishedInitializing { get; }
    public abstract Awaitable Initialize();
    public abstract void DeInitialize();
}
