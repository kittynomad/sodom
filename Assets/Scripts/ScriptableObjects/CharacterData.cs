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
    //name is how character is labeled in-game, doesn't need to be unique
    [SerializeField] private string _characterName;

    //characterID is used internally, must be unique and should be camelCase
    [SerializeField] private string _characterID;

    [Header("Character Assets")]
    //portrait to use if no portrait ID is specified
    [SerializeField] private Sprite _defaultCharacterPortrait;
    [SerializeField] private Portrait[] _portraits;

    //list of sound bytes to play while character is speaking
    [SerializeField] private RandomizedAudio _characterVoice;

    [TextArea(3, 10)] [SerializeField] private string _characterBio;

    [Space(10)]

    [Header("Character Stats")]
    [SerializeField] private int _characterHealth;
    [SerializeField] private int _attackPower;

    [Space(10)]

    [Header("Character Tags")]
    //Interaction types are the ways in which a character can be added to the bestiary
    //[SerializeField] private Enums.InteractType[] _interactionTypes;
    //Character types are primarily for organization
    //[SerializeField] private Enums.CharacterType[] _characterTypes;
    [SerializeField] private bool _appearsInBestiary = true;

    public string CharacterName { get => _characterName; set => _characterName = value; }
    public Sprite DefaultCharacterPortrait { get => _defaultCharacterPortrait; set => _defaultCharacterPortrait = value; }
    public RandomizedAudio CharacterVoice { get => _characterVoice; set => _characterVoice = value; }
    public Portrait[] Portraits { get => _portraits; set => _portraits = value; }
    public string CharacterBio { get => _characterBio; set => _characterBio = value; }
    public int CharacterHealth { get => _characterHealth; set => _characterHealth = value; }
    public int AttackPower { get => _attackPower; set => _attackPower = value; }
    //public Enums.InteractType[] InteractionTypes { get => _interactionTypes; set => _interactionTypes = value; }
    //public Enums.CharacterType[] CharacterTypes { get => _characterTypes; set => _characterTypes = value; }
    public bool AppearsInBestiary { get => _appearsInBestiary; set => _appearsInBestiary = value; }

    /// <summary>
    /// gets a portrait of the character from its associated ID. if no match is
    /// found, the default portrait is returned.
    /// </summary>
    /// <param name="id">the ID to find the portrait by.</param>
    /// <returns>the portrait associated with the ID.</returns>
    public Sprite GetPortraitByID(string id)
    {
        foreach (Portrait p in _portraits)
        {
            if (p.PortraitID.Equals(id))
            {
                return p.PortraitImage;
            }
        }
        Debug.Log("no portrait matching ID found, returning default");
        return _defaultCharacterPortrait;
    }

    /*public string FormattedTagString()
    {
        string output = "Tags: ";
        foreach (Enums.CharacterType t in _characterTypes)
            output = output + t + "; ";
        return output;
    }*/
}
