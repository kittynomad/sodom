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

    [MinMaxSlider(0f, 250f)]
    [SerializeField] private Vector2 _coinLaunchStrength;

    [SerializeField] private float[] _targetDivisions;

    [SerializeField] private GameObject _moneyPrefab;

    [Button]
    public void DropMoney()
    {
        float moneyToDrop = Random.Range(_moneyRange.x, _moneyRange.y);
        System.Array.Sort(_targetDivisions);
        for(int i = _targetDivisions.Length - 1; i >= 0; i--)
        {
            if(moneyToDrop / _targetDivisions[i] > 1f)
            {
                for(int j = 0; j < (int)(moneyToDrop / _targetDivisions[i]); j++)
                {
                    GameObject temp = Instantiate(_moneyPrefab, transform.position, Quaternion.identity);
                    temp.GetComponent<Rigidbody2D>().linearVelocity = Random.Range(_coinLaunchStrength.x, _coinLaunchStrength.y)
                        * Vector2.Normalize(new Vector2(Random.Range(-1f, 1f), Random.Range(0, 1f)));

                    temp.GetComponent<CurrencyObject>().UpdateValue(_targetDivisions[i]);
                }
            }
        }
    }
}
