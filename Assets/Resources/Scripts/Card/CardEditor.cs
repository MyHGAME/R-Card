using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardEditor : MonoBehaviour {
    public GameObject Card;
    public string CardID;
    public CardXML CardXML;
    public Dictionary<string, string> FundamentalElements = new Dictionary<string, string>()
    {
        {"卡牌编号",""},
        {"卡牌类型",""},
        {"卡牌称号",""},
        {"卡牌名字",""},
    };
    private Dictionary<string, string> ElementName = new Dictionary<string, string>()
    {
        {"卡牌编号","ID"},
        {"卡牌类型","Type"},
        {"卡牌称号","Title"},
        {"卡牌名字","Name"},
    };
    // Use this for initialization
    void Start () {
        Card = (GameObject)Instantiate(Config.config.Prefabs["Card"]);
	}

    void FundamentalElementsUI()
    {
        List<string> temp = new List<string>();
        temp.AddRange(FundamentalElements.Keys);
        int TempY = 0;
        foreach (string key in temp)
        {
            GUI.Label(new Rect(0, TempY, 60, 30), key);
            FundamentalElements[key] = GUI.TextField(new Rect(60, TempY, 100, 30), FundamentalElements[key]);
            TempY += 30;
        }
    }

    private void OnGUI()
    {
        FundamentalElementsUI();
        if (GUI.Button(new Rect(Screen.width-100, 0, 100, 30), "保存卡牌"))
        {
            SaveCard();
        }
        if (GUI.Button(new Rect(Screen.width-100, 40, 100, 30), "载入卡牌"))
        {
            LoadCard();
        }

    }

    void LoadUI(Card card)
    {

        string[] keys = new string[FundamentalElements.Count];
        FundamentalElements.Keys.CopyTo(keys,0);
        Type type = typeof(Card);
        for (int i = 0;i < keys.Length; i++)
        {
            FundamentalElements[keys[i]] = (string)type.GetField(ElementName[keys[i]]).GetValue(card);
        }

        
    }

    public void LoadCard()
    {
        CardXML = new CardXML(FundamentalElements["卡牌编号"]);
        CardXML.ReadXML();
        LoadUI(CardXML.Card);
        Card.GetComponent<SpriteRenderer>().sprite = Config.config.GetCardSprite(CardXML.Card.ID);
    }

    

    public void SaveCard()
    {
        string[] keys = new string[FundamentalElements.Count];
        FundamentalElements.Keys.CopyTo(keys, 0);
        Type type = typeof(Card);
        for (int i = 0; i < keys.Length; i++)
        {
           type.GetField(ElementName[keys[i]]).SetValue(CardXML.Card,FundamentalElements[keys[i]]);
        }
        Card card = CardXML.Card;
        CardXML = new CardXML(FundamentalElements["卡牌编号"]);
        CardXML.WriteXML(card);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
