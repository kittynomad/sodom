/*****************************************************************************
// File Name : CurrencyObject.cs
// Author : Pierce
// Creation Date : 7/4/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

public class CurrencyObject : MonoBehaviour
{
    [SerializeField] private float _value;

    [SerializeField] private SpriteAndValue[] _sprites;
    public void UpdateValue(float value)
    {
        _value = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            //add currency here
        }
    }
}
