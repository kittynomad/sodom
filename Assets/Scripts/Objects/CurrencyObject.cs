/*****************************************************************************
// File Name : CurrencyObject.cs
// Author : Pierce
// Creation Date : 7/4/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using NaughtyAttributes;

public class CurrencyObject : MonoBehaviour
{
    [SerializeField] private float _value;

    [SerializeField] private SpriteAndValue[] _sprites;

    private void Start()
    {
        UpdateValue(_value);
    }

    public void UpdateValue(float value)
    {
        _value = value;

        foreach(SpriteAndValue sv in _sprites)
        {
            if (_value >= sv.Value)
                gameObject.GetComponent<SpriteRenderer>().sprite = sv.Image;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.CollectCurrency(_value);
            Destroy(gameObject);
        }
    }

    [Button]
    public void SetCurrencyToZero()
    {
        UpdateValue(0);
    }

    [Button]
    public void SetCurrencyToOne()
    {
        UpdateValue(1);
    }
    [Button]
    public void SetCurrencyToTwo()
    {
        UpdateValue(2);
    }
    [Button]
    public void SetCurrencyToThree()
    {
        UpdateValue(3);
    }
}
