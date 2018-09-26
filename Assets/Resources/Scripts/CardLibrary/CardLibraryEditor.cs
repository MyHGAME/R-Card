using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibraryEditor : MonoBehaviour {
    public static CardLibraryEditor Instance;
    public List<Card> Cards = new List<Card>();
    public Vector2 ParameterScrollPosition;
    public float StartPosX = 700, StartPosY = 150,SizeX = 200,SizeY = 200;
    public Card ChoiceCard;
    public SerializableCardGroup EditCardGroup = new SerializableCardGroup();
    public bool CardGroupEditState = false;
    public int MaxRows = 5;
    private List<string> CardIDs = new List<string>();
    private bool Choice = false;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

	}
    
    public void EditState()
    {
        CardGroupEditState = true;
        LoadCardLibrary();
    }

    void LoadCardLibrary()
    {
        CardIDs = CardLibrary.Instance.CardLibraryXML.CardLibrary.CardIDs;
        LoadCards();
    }
	
	// Update is called once per frame
	void Update () {
      
      
        
	}

    public void AddCardInCards(Card card)
    {
        Cards.Add(card);
    }

    public void RemoveCardInCards(Card card)
    {
        Cards.Remove(card);
    }

    void OnGUI()
    {
        if (Choice&&GUI.Button(new Rect(StartPosX + 100, StartPosY -100, 50, 50), "加入"))
        {
            RemoveCardInCards(ChoiceCard);
            Choice = false;
            CardGroupEditor.Instance.AddCard(ChoiceCard);
        }

        ListView(StartPosX, StartPosY);
    }

    void ListView(float startPosX, float startPosY)
    {
        ParameterScrollPosition = GUI.BeginScrollView(new Rect(startPosX, startPosY, SizeX, SizeY), ParameterScrollPosition,
            new Rect(0, 0, 300,500 ), true, true);
        float CardPosX = 0,CardPosY = 0;
        int Rows = 0;
        foreach(Card card in Cards)
        {
            if (GUI.Button(new Rect(CardPosX, CardPosY, 50, 50), card.GetCurrentName()))
            {
                ChoiceCard = card;
                Choice = true;
                CardContentUI.Instace.SetCard(card);
            }
            
            if (Rows >= MaxRows -1) { CardPosY += 50; CardPosX = 0; Rows = 0; }
            else { Rows++; CardPosX += 50; }
           
        }
        GUI.EndScrollView();
    }

    void LoadCards()
    {
        List<Card> Temp = new List<Card>();
        List<string> TempID = new List<string>();
        if (CardGroupEditState)
        {
            TempID.AddRange(CardGroupEditor.Instance.CardIDs);
        }
        foreach(string id in CardIDs)
        {
            if (CardGroupEditState && TempID.Contains(id)) 
            {
                TempID.Remove(id);
                continue;
            }
            Temp.Add(new CardXML(id).ReadXML());
        }
 
        Cards = Temp;
    }
}
