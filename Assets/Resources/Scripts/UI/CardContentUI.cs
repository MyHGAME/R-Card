using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardContentUI : MonoBehaviour {
    public static CardContentUI Instace;
    public Card Card = new Card();
    public List<RCard> RCards;
    public RCard RCard;
    public bool UseGUI = true;
    public float SizeScale = 0.4f;
    public Vector2 StartPos,ContentScrollPosition;
    private Vector2 size = new Vector2(342,455);
    public Vector2 Size
    {
        get
        {
            return size * SizeScale;
        }
    }

    void Awake()
    {
        Instace = this;
    }

    // Use this for initialization
    void Start()
    {
        //SetCard(new CardXML("C000001").ReadXML());
    }

    private void OnGUI()
    {
        if (!UseGUI)
            return;
        if (Card.ID != "")
        {
            ShowCardImage(StartPos, Size, Config.config.GetCardSprite(Card.ID).texture);
            ShowCardContent(new Vector2(StartPos.x, StartPos.y + Size.y));
        }

    }

    void ShowCardContent(Vector2 startPos)
    {
        ContentScrollPosition = GUI.BeginScrollView(new Rect(startPos, new Vector2(280, 200)), ContentScrollPosition, new Rect(0, 0, Screen.width, 1000),true,true);
        GUI.Label(new Rect(0,0,300,25),Card.Title + " " +Card.Name);
        GUI.Label(new Rect(0, 25, 300, 25), Card.Type);
        float height = 50f;
        //显示属性，可以用卡牌编辑的属性显示方法，要做到模块分离，ui和逻辑分开，只需调用简单的接口，就能显示ui
        if (Config.config.GetCardType(Card.Type) == "单位")
        {
            if (RCard != null)
            {
               // RCard.gameObject.GetComponent<UnitCard>().PresentAttribute.MaxEnergy
            }
            else 
            {
                GUI.Label(new Rect(0, height, 200, 25), "生命" + Card.GetCurrentDetail(Card.Attributes.MaxLife));
                GUI.Label(new Rect(150, height, 200, 25), "能量 " + Card.GetCurrentDetail(Card.Attributes.MaxEnergy));
                height += 25;
                GUI.Label(new Rect(0, height, 200, 25), "攻击 " + Card.GetCurrentDetail(Card.Attributes.Attack));
                GUI.Label(new Rect(150, height, 200, 25), "防御 " + Card.GetCurrentDetail(Card.Attributes.Defence));
                height += 25;
                GUI.Label(new Rect(0, height, 200, 25), "攻击距离 " + Card.GetCurrentDetail(Card.Attributes.AttackDistance));
                GUI.Label(new Rect(150, height, 200, 25), "移动距离 " + Card.GetCurrentDetail(Card.Attributes.MovingDistance));
                height += 25;
            }
        }
        height += 25;
        if (Card.PassiveSkills.Count > 0)
        {
            GUI.Label(new Rect(0, height, 150, 25), "被动技能");
            height += 25;
        }
        foreach (Skill skill in Card.PassiveSkills)
        {
            GUI.Label(new Rect(0, height, 200, 25), skill.Name);
            height += 25;
            GUI.Label(new Rect(0, height, 300, 100), Card.GetCurrentDetail(skill.Content));
            height += 100;
        }
        if (Card.CouterSkills.Count > 0)
        {
            GUI.Label(new Rect(0, height, 150, 25), "反制技能");
            height += 25;
        }
        foreach(Skill skill in Card.CouterSkills)
        {
            GUI.Label(new Rect(0, height, 200, 25), skill.Name + "(" + skill.Cost + " " + skill.Degree + ")");
            height += 25;
            GUI.Label(new Rect(0, height, 300, 100), Card.GetCurrentDetail(skill.Content));
            height += 100;
        }
        if (Card.ActiveSkills.Count > 0)
        {
            GUI.Label(new Rect(0, height, 150, 25), "主动技能");
            height += 25;
        }
        foreach (Skill skill in Card.ActiveSkills)
        {
            GUI.Label(new Rect(0, height,200, 25), skill.Name + "(" + skill.Cost +" "+ skill.Degree+")");
            height += 25;
            GUI.Label(new Rect(0, height, 300, 100), Card.GetCurrentDetail(skill.Content));
            height += 100;
        }
        GUI.EndScrollView();
    }

    void ShowCardImage(Vector2 startPos, Vector2 texture_size, Texture texture)
    {
        if (texture == null)
            return;
        GUI.DrawTexture(new Rect(startPos, texture_size), texture);
    }

    public void SetRCard(RCard rCard)
    {
        RCard = rCard;
        SetCard(RCard.CardXML.Card);
    }

    public void SetCard(Card card)
    {
        Card = card;
    }

    public void SetCard(string ID)
    {
        SetCard(new CardXML(ID).ReadXML());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
