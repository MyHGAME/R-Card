using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node
{
    public int x;
    public int y;
    public Node()
    {}

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


public class MapNode : AreaBase {
    public SerializableMapNode serializableMapNode = new SerializableMapNode();

    public Node Node
    {
        get
        {
            return serializableMapNode.Node;
        }
    }

    public SerializableMapNode SerializableMapNode
    {
        get
        {
            return serializableMapNode;
        }

        set
        {
            serializableMapNode = value;
            Area = serializableMapNode.Area;
        }
    }

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
