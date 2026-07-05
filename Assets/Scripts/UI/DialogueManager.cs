/*****************************************************************************
// File Name : DialogueManager.cs
// Author : Pierce
// Creation Date : 7/5/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using XNode;
using System.Collections;
using NaughtyAttributes;

public class DialogueManager : MonoBehaviour
{
    

    //dialogue settings
    [SerializeField] private float _chatSpeed = 0f;
    [SerializeField] private bool _autoAdvance = false;

    //references
    [SerializeField] private TextMeshProUGUI _dialogueBody;
    [SerializeField] private TextMeshProUGUI _speakerLabel;
    [SerializeField] private Image _speakerPortrait;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _playerResponses;
    [SerializeField] private GameObject[] _responseButtons;
    [SerializeField] private AudioSource _voiceClipSource;

    [SerializeField] private DialogueNodeGraph _testDialogueTree;

    private Node nextNode;
    private DialogueNode currentNode;
    private GameObject treeSource;
    private string currentDialogue;
    private bool isTyping;
    private bool isOpen = false;

    /// <summary>
    /// initializes a conversation from an outside source.
    /// </summary>
    /// <param name="branchDialogue">The current node of dialogue.</param>
    /// <param name="NPC">The GameObject which initiates the conversation.</param>
    /// <param name="willAutoAdvance">Determines if dialogue will advance automatically.</param>
    public void StartDialogue(LinkedNode branchDialogue, GameObject NPC, bool willAutoAdvance)
    {
        isOpen = true;
        FindAnyObjectByType<PlayerInput>().actions.FindActionMap("UI").Enable();
        FindAnyObjectByType<PlayerInput>().actions.FindActionMap("Player").Disable();
        _dialogueBox.SetActive(true);
        _playerResponses.SetActive(false);

        print("Dialogue started");
        //displays input node if it's a dialogueNode
        if (branchDialogue is IntroNode)
            nextNode = branchDialogue.NextNode;
        //displays input node's next node if it isn't dialogue
        else
            nextNode = branchDialogue;

        AdvanceDialogue();
    }

    [Button]
    public void AdvanceDialogue()
    {
        StopAllCoroutines();

        if(nextNode == null)
        {
            EndDialogue();
            return;
        }

        switch(nextNode.GetType().ToString())
        {
            case "IntroNode":
                break;
            case "DialogueNode":
                currentNode = nextNode as DialogueNode;
                currentNode.OnCall();
                break;
            case "DialogueBranchNode":
                SetUpDialogueChoices(nextNode as DialogueBranchNode);
                return;

        }

        SingleDialogue dialogue = null;
        try
        {
            dialogue = currentNode.Dialogue;
        }
        catch
        {
            print("node is of no known type");
            return;
        }


        //pull current dialogue text out of the SingleDialogue data
        currentDialogue = dialogue.sentences;

        //pull current talker's name out of the SingleDialogue's TalkerData
        string nameTag = dialogue.TalkerData.CharacterName;

        //pull current talker's portrait out of the SingleDialogue's TalkerData
        Sprite talkIMG = dialogue.TalkerData.GetPortraitByID(dialogue.PortraitID);
        AudioClip[] voice = dialogue.TalkerData.CharacterVoice.clips;

        //print dialogue text (for debug mostly)
        print(currentDialogue);

        //Stop all coroutines to prevent complications
        StopAllCoroutines();
        //Start coroutine to type out dialogue text
        StartCoroutine(TypeSentence(currentDialogue, voice));
        //update ui elements
        _speakerLabel.text = nameTag;
        _speakerPortrait.sprite = talkIMG;
        //_speakerPortrait.SetNativeSize(); //just in case any portraits have different dimensions

        //prepare next node for next call of DisplayNextSentence
        nextNode = currentNode.NextNode;
    }

    /// <summary>
    /// Makes a sentence in dialogue appear one letter at a time, as well as play the associated voice clip(s).
    /// </summary>
    /// <param name="sentence">The string to be displayed as a sentence.</param>
    /// <param name="voice">An array of AudioClips to be used as the voice.</param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence, AudioClip[] voice)
    {
        isTyping = true;

        _dialogueBody.maxVisibleCharacters = 0;
        _dialogueBody.text = sentence;
        char[] sentenceCharArray = sentence.ToCharArray();

        for (int i = 0; i < sentenceCharArray.Length; i++)
        {
            char letter = sentenceCharArray[i];

            _dialogueBody.maxVisibleCharacters++;

            //clip isn't played for specific chars or when no voice is available
            if (voice != null && letter != " "[0] && letter != ","[0] && letter != "'"[0])
            {
                int randomVChoice = UnityEngine.Random.Range(0, voice.Length);
                _voiceClipSource.clip = voice[randomVChoice];
                _voiceClipSource.Play();
            }

            //wait pre-specified time until printing the next letter
            yield return new WaitForSeconds(_chatSpeed);
        }

        isTyping = false;

        if (_autoAdvance) //go straight to next sentence if autoAdvance is on
        {
            AdvanceDialogue();
        }
        //_buttonPrompt.enabled = true;
    }

    void SetUpDialogueChoices(DialogueBranchNode branchNode)
    {
        StopAllCoroutines();
        //first check if there are any choices set up
        if (branchNode.nextNodes.Length > 0)
        {
            //show choice buttons
            _playerResponses.SetActive(true);

            //enable or disable scrollBar if needed
            //_scrollbar.interactable =
                //branchNode.nextNodes.Length >= minOptionsForScroll;

            for (int i = 0; i < _responseButtons.Length; i++)
            {
                //sets buttons to each choice for this chunk of dialogue
                if (i < branchNode.nextNodes.Length)
                {
                    ChoiceNode temp = branchNode.nextNodes[i] as ChoiceNode;
                    _responseButtons[i].SetActive(true);

                    //make button "locked" if corresponding choice is labeled as not selectable
                    if (!temp.IsSelectable())
                    {
                        _responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text
                                = "LOCKED";
                        _responseButtons[i].GetComponent<Button>().interactable = false;
                        continue;
                    }

                    //sets button text and stored node to choice's data
                    _responseButtons[i].GetComponent<Button>().interactable = true;
                    _responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = temp.ChoiceLabel;
                    _responseButtons[i].GetComponent<DialogueGiver>().DialogueToGive = temp.NextNode as DialogueNode;
                    _responseButtons[i].GetComponent<DialogueGiver>().Npc = treeSource;
                }
                //hides any extra buttons
                else
                {
                    _responseButtons[i].SetActive(false);
                }
            }

        }
    }

    public void EndDialogue()
    {
        _dialogueBox.SetActive(false);
        FindAnyObjectByType<PlayerInput>().actions.FindActionMap("UI").Disable();
        FindAnyObjectByType<PlayerInput>().actions.FindActionMap("Player").Enable();
        isOpen = false;
    }

    [Button]
    public void TestDialogue()
    {
        StartDialogue(_testDialogueTree.findIntroNode(), gameObject, false);
    }

    public void OnContinue()
    {
        if (isOpen && !_autoAdvance)
        {
            if (isTyping) //allows showing all dialogue instantly
            {
                isTyping = false;
                StopAllCoroutines();

                _dialogueBody.maxVisibleCharacters = currentDialogue.ToCharArray().Length;
                //_buttonPrompt.enabled = true;
            }
            else
                AdvanceDialogue();
        }
    }
}
