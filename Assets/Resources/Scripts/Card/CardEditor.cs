using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEditor : MonoBehaviour {
    public GameObject Card;
    public string CardID;
    public CardXML CardXML; 
	// Use this for initialization
	void Start () {
        Card = (GameObject)Instantiate(Config.config.Prefabs["Card"]);
	}


    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 80, 30), "卡牌编号");
        CardID = GUI.TextField(new Rect(80, 0, 100, 30), CardID);
        if (GUI.Button(new Rect(0, 40, 100, 30), "保存卡牌"))
        {
            SaveCard();
        }
        if (GUI.Button(new Rect(0, 80, 100, 30), "载入卡牌"))
        {
            LoadCard();
        }

    }

    public void LoadCard()
    {
        CardXML = new CardXML(CardID);
        CardXML.ReadXML();
        print(CardXML.Card.ID);
        Card.GetComponent<SpriteRenderer>().sprite = Config.config.GetCardSprite(CardXML.Card.ID);
    }

    public void SaveCard()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
