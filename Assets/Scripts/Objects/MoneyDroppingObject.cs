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

    [SerializeField] private float[] _targetDivisions;

    [SerializeField] private GameObject _moneyPrefab;

    public void DropMoney()
    {
        float moneyToDrop = Random.Range(_moneyRange.x, _moneyRange.y);
        System.Array.Sort(_targetDivisions);
    }
}
