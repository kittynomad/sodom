/*****************************************************************************
// File Name : SpriteAndValue.cs
// Author : Pierce
// Creation Date : 7/4/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

[System.Serializable]
public class SpriteAndValue
{
    [SerializeField] private Sprite _image;
    [SerializeField] private float _value;

    public Sprite Image { get => _image; set => _image = value; }
    public float Value { get => _value; set => _value = value; }
}
