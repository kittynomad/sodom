using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogueNodeGraph : NodeGraph
{
    /// <summary>
    /// Iterates through NodeGraph to find a node of type IntroNode.
    /// Used to find the intro point for dialogue.
    /// </summary>
    /// <returns>Entry point of DialogueNodeGraph.</returns>
    public IntroNode findIntroNode()
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].GetType().ToString().Equals("IntroNode"))
                return nodes[i] as IntroNode;
        }
        return null;
    }
}
