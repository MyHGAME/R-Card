using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardGroupType
{
    Main = 0,
    Replace = 1,
}

[Serializable]
public class SerializableCardGroup
{
    public string CardGroupName;
    public List<string> MainCardIDList = new List<string>();
    public List<string> ReplaceCardIDList = new List<string>();
    private Dictionary<CardGroupType, List<string>> CardGroupList = new Dictionary<CardGroupType, List<string>>() 
    {
        {CardGroupType.Main, new List<string>()},
        {CardGroupType.Replace,new List<string>()},

    };
    public SerializableCardGroup()
    {
        CardGroupList[CardGroupType.Main] = MainCardIDList;
        CardGroupList[CardGroupType.Replace] = ReplaceCardIDList;
    }

    //CardArea = 0
    public bool AddCard(string ID, CardGroupType cardgroup = CardGroupType.Main)
    {
        if (CardGroupList[cardgroup].Count >= Config.config.CardGroupMaxCount[cardgroup]) return false;
        CardGroupList[cardgroup].Add(ID); return true;
    }

    public void SetCardGroupName(string name)
    {
        CardGroupName = name;
    }


    public bool AddCard(Card card, CardGroupType cardgroup = CardGroupType.Main)
    {
        return AddCard(card.ID, cardgroup);
    }
    public bool RemoveCard(string ID, CardGroupType cardgroup = CardGroupType.Main)
    {
        if (!CardGroupList[cardgroup].Contains(ID)) return false;
        CardGroupList[cardgroup].Remove(ID); return true;
    }

    public bool RemoveCard(Card card, CardGroupType cardgroup = CardGroupType.Main)
    {
        return RemoveCard(card.ID, cardgroup);
    }
}

