using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

/*
 * ChoiceNode is basically just used to define each choice in a dialogue branch
 * ChoiceNode itself doesn't do much, besides storing the label for the button
 * the player presses for the dialogue choice
 * However it also allows for more modular choices (i.e. ItemChoiceNode, which
 * inherits from ChoiceNode but requires a specific item to be obtained to be
 * choosable)
 */

[NodeTint("#46677a")]
public class ChoiceNode : LinkedNode {
	//the text to appear on the button for this choice
	[SerializeField] private string _choiceLabel;

    public string ChoiceLabel { get => _choiceLabel; set => _choiceLabel = value; }

	public virtual bool IsSelectable()
    {
		Debug.Log("choice node");
		return true; // regular choice node is always selectable
    }

    // Use this for initialization
    protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this

	}
}