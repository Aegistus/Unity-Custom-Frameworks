using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityNode : Node
{
    protected GameObject gameObject;
    protected Transform transform;

    public UnityNode(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
    }
}
