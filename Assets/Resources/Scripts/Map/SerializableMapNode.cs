using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableBase
{
    public string Owner;//标识所属方

    public void SetOwner(string owner)//设置所属方
    {
        Owner = owner;
    }
}

[Serializable]
public class SerializableAreaBase:SerializableBase
{
    public Area Area = Area.None;
}

[Serializable]
public class SerializableMapCard: SerializableBase
{
    public string CardID;
    public List<string> EquipmentsID = new List<string>();

    public SerializableMapCard()
    { }

    public SerializableMapCard(string card_id)
    {
        CardID = card_id;
    }

    public bool AddEquipments(string equipment_id)
    {
        if (CardID == "")
            return false;
        EquipmentsID.Add(equipment_id);
        return true;
    }
}

[Serializable]
public class SerializableMapNodes
{
    public List<string> PlayerCamps = new List<string>();
    public List<string> NeutralCamps = new List<string>();
    public List<SerializableMapNode> MapNodes = new List<SerializableMapNode>();

    public SerializableMapNodes()
    { }

    public void AddCamps(string player,string neutral)
    {
        PlayerCamps.Clear();
        NeutralCamps.Clear();
        PlayerCamps.AddRange(player.Split(' '));
        NeutralCamps.AddRange(neutral.Split(' '));
    }

    public void AddMapNode(List<GameObject> nodes)
    {
        foreach (GameObject obj in nodes)
        {
            MapNodes.Add(obj.GetComponent<MapNode>().SerializableMapNode);
        }
    }

    public SerializableMapNodes(List<GameObject> nodes)
    {
        AddMapNode(nodes);
    }

    public SerializableMapNodes(List<GameObject> nodes, string player, string neutral)
    {
        AddMapNode(nodes);
        AddCamps(player, neutral);
    }
}

[Serializable]
public class SerializableMapNode : SerializableAreaBase
{
    public Node Node;
    public SerializableMapCard Cards;
    public List<string> AmbushCardsID = new List<string>();

    public SerializableMapNode()
    { }

    public SerializableMapNode(int x, int y, string CardID = "")
    {
        Node = new Node(x, y);
        if (CardID == "")
            return;
        Cards = new SerializableMapCard(CardID);
    }

    public void AddAmbushCard(string ambush_id)
    {
        AmbushCardsID.Add(ambush_id);
    }

}


