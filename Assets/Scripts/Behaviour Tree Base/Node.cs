using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    protected NodeState _nodeState;
    public NodeState NodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}

public enum NodeState
{
    Running, Failure, Success
}
