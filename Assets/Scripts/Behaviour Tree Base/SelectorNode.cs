using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : UnityNode
{
    private List<UnityNode> childNodes = new List<UnityNode>();

    public SelectorNode(GameObject gameObject, List<UnityNode> childNodes) : base(gameObject)
    {
        this.childNodes = childNodes;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i < childNodes.Count; i++)
        {
            switch (childNodes[i].Evaluate())
            {
                case NodeState.Running:
                    break;
                case NodeState.Failure:
                    break;
                case NodeState.Success:
                    _nodeState = NodeState.Success;
                    return NodeState.Success;
                default:
                    break;
            }
        }
        _nodeState = NodeState.Failure;
        return _nodeState;
    }
}
