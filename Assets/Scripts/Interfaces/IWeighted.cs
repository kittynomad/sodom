/*****************************************************************************
// File Name : IWeighted.cs
// Author : Arcadia Koederitz
// Creation Date : 6/29/2026
// Last Modified : 6/29/2026
//
// Brief Description : Interface for any object that can be used for weighted random selection.
*****************************************************************************/
using UnityEngine;

public interface IWeighted
{
    int GetWeight(float param);
}
