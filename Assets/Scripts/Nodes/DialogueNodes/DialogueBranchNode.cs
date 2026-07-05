using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#49467a")]
//doesn't inherit from LinkedNode like the others due to having multiple "nextNodes"
public class DialogueBranchNode : Node {
	[Input] [SerializeField] private Node _lastNode;
	[Output(dynamicPortList = true)] [SerializeField] private Node[] _responses;

	public Node[] nextNodes;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		nextNodes = new Node[_responses.Length];
		for(int i = 0; i < _responses.Length; i++)
        {
			//gets each node in the _responses input and adds them to nextNodes
			NodePort nextPort = GetOutputPort("_responses " + i).Connection;
			if (nextPort != null)
			{
				nextNodes[i] = nextPort.node as Node;
			}
		}
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		if (port.fieldName == "_lastNode")
			return GetInputValue<Node>("_lastNode", this);
		return null; // Replace this
	}
}