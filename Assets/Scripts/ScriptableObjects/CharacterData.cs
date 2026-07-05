using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CharacterData stores, unsurprisingly, data related to a specific character
 * this includes name, portraits, and audio clips
 * combining all of this into a single asset makes referencing characters in
 * things like dialogue much more convenient as all related assets are bundled
 * together
 */
[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string _characterName;
    [TextArea(3, 10)] [SerializeField] private string _characterBio;
    [SerializeField] private Sprite _characterIcon;
    [SerializeField] private Sprite _defaultCharacterPortrait;
    [SerializeField] private Portrait[] _portraits;
    [SerializeField] private RandomizedAudio _characterVoice;
    [SerializeField] private Color _textColor = Color.black;

    public string CharacterName { get => _characterName; set => _characterName = value; }
    public Sprite DefaultCharacterPortrait { get => _defaultCharacterPortrait; set => _defaultCharacterPortrait = value; }
    public RandomizedAudio CharacterVoice { get => _characterVoice; set => _characterVoice = value; }
    public Portrait[] Portraits { get => _portraits; set => _portraits = value; }
    public string CharacterBio { get => _characterBio; set => _characterBio = value; }
    public Sprite CharacterIcon { get => _characterIcon; set => _characterIcon = value; }
    public Color TextColor { get => _textColor; set => _textColor = value; }

    /// <summary>
    /// gets a portrait of the character from its associated ID. if no match is
    /// found, the default portrait is returned.
    /// </summary>
    /// <param name="id">the ID to find the portrait by.</param>
    /// <returns>the portrait associated with the ID.</returns>
    public Sprite GetPortraitByID(string id)
    {
        foreach(Portrait p in _portraits)
        {
            if(p.PortraitID.Equals(id))
            {
                return p.PortraitImage;
            }
        }
        Debug.Log("no portrait matching ID found, returning default");
        return _defaultCharacterPortrait;
    }
}
