using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableCardLibrary{
    public List<string> CardIDs = new List<string>();

    public SerializableCardLibrary()
    {}

    public void AddCard(string ID)
    {
        CardIDs.Add(ID);
        
    }

    public void AddCard(Card Card)
    {
        AddCard(Card.ID);
    }

    public void RemoveCard(string ID)
    {
        CardIDs.Remove(ID);
    }

    public void RemoveCard(Card Card)
    {
        RemoveCard(Card.ID);
    }
}
