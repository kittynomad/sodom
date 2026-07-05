/*****************************************************************************
// File Name : DialogueManager.cs
// Author : Pierce
// Creation Date : 7/5/2026
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using TMPro;
using XNode;

public class DialogueManager : MonoBehaviour
{
    private Node nextNode;

    [SerializeField] private TextMeshProUGUI _dialogueBody;
    
    [SerializeField] private GameObject _dialogueBox;

    /// <summary>
    /// initializes a conversation from an outside source.
    /// </summary>
    /// <param name="branchDialogue">The current node of dialogue.</param>
    /// <param name="NPC">The GameObject which initiates the conversation.</param>
    /// <param name="willAutoAdvance">Determines if dialogue will advance automatically.</param>
    public void StartDialogue(LinkedNode branchDialogue, GameObject NPC, bool willAutoAdvance)
    {
        //displays input node if it's a dialogueNode
        if (branchDialogue is IntroNode)
            nextNode = branchDialogue.NextNode;
        //displays input node's next node if it isn't dialogue
        else
            nextNode = branchDialogue;
    }

    public void AdvanceDialogue()
    {
        StopAllCoroutines();
    }
}
