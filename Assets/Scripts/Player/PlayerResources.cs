/*****************************************************************************
// File Name : PlayerResources.cs
// Author : Pierce
// Creation Date : 7/4/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private float _currency;

    public float Currency { get => _currency; set => _currency = value; }
}
