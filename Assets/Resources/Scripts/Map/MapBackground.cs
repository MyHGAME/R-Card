using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//编辑模式下承载点击生成的地图节点的背景，用透明的格子来做
public class MapBackground : MonoBehaviour {
    public Node Node = new Node();
    public GameObject MapNode;

    //地图节点生成，在自己的上面生成地图节点
    public GameObject NodeGenerator(GameObject node_prefab)
    {
        if (MapNode == null)
        {
            GameObject node = (GameObject)Instantiate(node_prefab, this.transform.position, node_prefab.transform.rotation);
            MapNode mapNode = node.GetComponent<MapNode>();
            if (mapNode == null) mapNode = node.AddComponent<MapNode>();
            //mapNode.Node = this.Node;
            //赋值地图节点
            mapNode.SerializableMapNode.Node = this.Node;
            MapNode = node;
            return node;
        }
        else
        {
            return null;
        }
            
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
