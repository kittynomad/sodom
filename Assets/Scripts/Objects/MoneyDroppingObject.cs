/*****************************************************************************
// File Name : MoneyDroppingObject.cs
// Author : Pierce
// Creation Date : 7/4/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class MoneyDroppingObject : MonoBehaviour
{
    [MinMaxSlider(0f, 1000f)] [SerializeField]
    private Vector2 _moneyRange;
}
