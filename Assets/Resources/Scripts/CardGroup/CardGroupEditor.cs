using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroupEditor : MonoBehaviour {
    public static CardGroupEditor Instance;
    public List<string> CardIDs = new List<string>();
    public List<Card> Cards = new List<Card>();
    public string CardGroupName;
    public Vector2 ParameterScrollPosition;
    public bool Choice;
    public Card ChoiceCard;
    public float StartPosX = 300 , StartPosY = 150,SizeX = 200,SizeY = 200;
    public int MaxRows = 5;
    private CardGroup CardGroup;
    void Awake()
    {
        Instance = this;
       
    }


    void LoadCardGroup()
    {
        CardIDs = CardGroup.CardGroupXML.CardGroup.MainCardIDList;
        CardGroupName = CardGroup.CardGroupXML.CardGroup.CardGroupName;
        LoadCards();
    }

    public void AddCard(Card card)
    {
        AddCard(card.ID);
    }

    public void AddCard(string id)
    {
        CardIDs.Add(id);
        Cards.Add(new CardXML(id).ReadXML());
    }

    public void RemoveCard(string id)
    {
        CardIDs.Remove(id);
    }

    public void RemoveCard(Card card)
    {
        RemoveCard(card.ID);
        Cards.Remove(card);
    }

    void LoadCards()
    {
        List<Card> Temp = new List<Card>();
        foreach (string id in CardIDs)
        {
            Temp.Add(new CardXML(id).ReadXML());
        }
        Cards = Temp;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(StartPosX, StartPosY - 100, 70, 30), "卡组名字");
        CardGroupName = GUI.TextField(new Rect(StartPosX + 70, StartPosY - 100, 100, 30), CardGroupName);
        if (GUI.Button(new Rect(StartPosX, StartPosY - 60, 50, 50), "保存"))
        {
            if (!CardGroup.GroupManagerXML.GroupManager.GroupNames.Contains(CardGroupName))
            {
                CardGroup.GroupManagerXML.GroupManager.AddGroup(CardGroupName);
                CardGroup.GroupManagerXML.WriteXML();
            }
            CardGroup.CardGroupXML = new CardGroupXML(CardGroupName);
            SerializableCardGroup TempCardGroup = new SerializableCardGroup();
            TempCardGroup.SetCardGroupName(CardGroupName);
            TempCardGroup.MainCardIDList = CardIDs;
            CardGroup.CardGroupXML.WriteXML(TempCardGroup);
        }
        if (GUI.Button(new Rect(StartPosX + 100 , StartPosY-60, 50, 50), "载入"))
        {
            if (CardGroup.LoadCardGroup(CardGroupName))
            {
                LoadCardGroup();
                CardLibraryEditor.Instance.EditState();
            }
        }
        if (GUI.Button(new Rect(StartPosX + 200, StartPosY - 120, 100, 50), "设置主卡组"))
        {
            CardGroup.SetMainCardGroup(CardGroupName);
        }
        if (Choice && GUI.Button(new Rect(StartPosX + 200, StartPosY - 60, 50, 50), "移除"))
        {
            RemoveCard(ChoiceCard);
            Choice = false;
            CardLibraryEditor.Instance.AddCardInCards(ChoiceCard);
        }

        ListView(StartPosX, StartPosY);
    }

    void ListView(float startPosX, float startPosY)
    {
        ParameterScrollPosition = GUI.BeginScrollView(new Rect(startPosX, startPosY, SizeX, SizeY), ParameterScrollPosition,
            new Rect(0, 0, 300, 500), true, true);
        float CardPosX = 0, CardPosY = 0;
        int Rows = 0;
        foreach (Card card in Cards)
        {
            if (GUI.Button(new Rect(CardPosX, CardPosY, 50, 50), card.GetCurrentName()))
            {
                ChoiceCard = card;
                Choice = true;
                CardContentUI.Instace.SetCard(card);
            }

            if (Rows >= MaxRows - 1) { CardPosY += 50; CardPosX = 0; Rows = 0; }
            else { Rows++; CardPosX += 50; }

        }
        GUI.EndScrollView();
    }


	// Use this for initialization
	void Start () {
        CardGroup = this.GetComponent<CardGroup>();
        LoadCardGroup();
        CardLibraryEditor.Instance.EditState();
      
	}
	
	// Update is called once per frame
	void Update () {
     
	}
}
