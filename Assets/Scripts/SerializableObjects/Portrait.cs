using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Portrait
{
    //image associated with portrait
    [SerializeField] private Sprite _portraitImage;
    //id to find portrait from
    [SerializeField] private string _portraitID;

    public Sprite PortraitImage { get => _portraitImage; set => _portraitImage = value; }
    public string PortraitID { get => _portraitID; set => _portraitID = value; }
}
