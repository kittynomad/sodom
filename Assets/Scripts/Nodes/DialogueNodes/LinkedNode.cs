using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

/* 
 * LinkedNode isn't meant to be used directly
 * instead, it builds on the existing Node class with references to a
 * next and previous node, and then is inherited by most dialogue-related
 * nodes
 * This reduces code reuse and allows pulling of next/last node without knowing
 * which type of node any given node is
 */
public class LinkedNode : Node {
	[Output] [SerializeField] private Node _lastNode;
	[Input] [SerializeField] private Node _nextNode;

	private Node nextNode;

    public Node NextNode { get => nextNode; set => nextNode = value; }
    public Node LastNode { get => _lastNode; set => _lastNode = value; }

    // Use this for initialization
    protected override void Init() {
		base.Init();
		//Gets the nextNode port
		NodePort nextPort = GetInputPort("_nextNode").Connection;
		if (nextPort != null) //sets nextNode as nextPort's connection
		{
			NextNode = nextPort.node as Node;
		}
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public virtual void OnCall()
    {

    }
}