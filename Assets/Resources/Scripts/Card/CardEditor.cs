using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CardEditor : MonoBehaviour {
    public GameObject Card;
    public CardXML CardXML;
    public int Columns = 2;
    public float MaxOffsetY = 30f;
    public Dictionary<string, string> FundamentalElements = new Dictionary<string, string>()
    {
        {"卡牌编号",""},
        {"卡牌类型",""},
        {"卡牌称号",""},
        {"卡牌名字",""},
    };
    public Dictionary<string,string> AttritubesElements = new Dictionary<string, string>()
    {
        {"最大生命",""},
        {"最大能量","" },
        {"攻击力","" },
        {"防御力","" },
        {"攻击距离","" },
        {"移动距离","" },

    };
    public Dictionary<string, List<Skill>> Skills = new Dictionary<string, List<Skill>>()
    {
        {"被动技能",new List<Skill>()},
        {"反制技能",new List<Skill>()},
        {"主动技能",new List<Skill>()},
    };

    public List<string> Skill = new List<string>()
    {
        {"技能名字"},
        {"技能花费"},
        {"技能程度"},
        {"技能内容"},
    };
    private Dictionary<string, string> ElementName = new Dictionary<string, string>()
    {
        {"卡牌编号","ID"},
        {"卡牌类型","Type"},
        {"卡牌称号","Title"},
        {"卡牌名字","Name"},
        {"最大生命","MaxLife"},
        {"最大能量","MaxEnergy"},
        {"攻击力","Attack"},
        {"防御力","Defence"},
        {"攻击距离","AttackDistance"},
        {"移动距离","MovingDistance"},
        {"被动技能","PassiveSkills"},
        {"反制技能","CouterSkills"},
        {"主动技能","ActiveSkills"},
        {"技能名字","Name"},
        {"技能花费","Cost"},
        {"技能程度","Degree"},
        {"技能内容","Content"},
    };
    public Vector2 scrollPosition;
    public float ViewHeight;
    // Use this for initialization
    void Start () {
        Card = (GameObject)Instantiate(Config.config.Prefabs["Card"]);

        
    }

    

    void ElementsUITemplate(Dictionary<string,string> item,float startPosY = 0,int columns = 1)
    {
        string[] keys = GetKeys(item);
        float OffsetY = startPosY;
        int Num = 0;
        foreach (string key in keys)
        {
            int result = Num % columns;
            GUI.Label(new Rect(result * 160, OffsetY, 60, 30), key);
            item[key] = GUI.TextField(new Rect(60 + result * 160, OffsetY, 100, 30), item[key]);
            if (result == columns - 1) OffsetY += MaxOffsetY;
            Num += 1;
        }
    }

    void SkillUITemplate(Dictionary<string, List<Skill>> item, float startPosY = 0)
    {
        string[] keys = GetKeys(item);
        //开始滚动视图  
        scrollPosition = GUI.BeginScrollView(new Rect(0, startPosY, 200, 200), scrollPosition, new Rect(0, 0, Screen.width, item.Count * 30 + ViewHeight),true, true);
        float OffsetY = 0;
        foreach (string key in keys)
        {
            if (GUI.Button(new Rect(0, OffsetY, 100, 30), "+"+key))
            {
                item[key].Add(new Skill());
                ViewHeight += Skill.Count * 30;
            }
            OffsetY += 30;
            List<Skill> DeleteSkill = new List<Skill>();
            foreach (Skill skill in item[key])
            {
                Type type = typeof(Skill);
                foreach (string temp in Skill)
                {
                    if (temp == "技能名字")
                    {
                        if (GUI.Button(new Rect(0, OffsetY, 70, 30), "-" + temp))
                        {
                            DeleteSkill.Add(skill);
                        }
                    }
                    else
                    {
                        GUI.Label(new Rect(0, OffsetY, 60, 30), temp);
                    }
                    string temptext = GUI.TextField(new Rect(70, OffsetY, 200, 30), (string)type.GetField(ElementName[temp]).GetValue(skill));
                    type.GetField(ElementName[temp]).SetValue(skill, temptext);
                    OffsetY += 30;
                }
            }
            foreach (Skill delete in DeleteSkill)
            {
                item[key].Remove(delete);
                ViewHeight -= Skill.Count * 30;
            }
        }
        //结束滚动视图  
        GUI.EndScrollView();
    }
   

    private void OnGUI()
    {
        ElementsUITemplate(FundamentalElements,0, Columns);
        ElementsUITemplate(AttritubesElements,MaxOffsetY*FundamentalElements.Count / Columns,Columns);
        SkillUITemplate(Skills, MaxOffsetY * (FundamentalElements.Count + AttritubesElements.Count) / Columns);
        if (GUI.Button(new Rect(Screen.width-100, 0, 100, 30), "保存卡牌"))
        {
            SaveCard();
        }
        if (GUI.Button(new Rect(Screen.width - 100, 40, 100, 30), "载入卡牌"))
        {
            LoadCard();
            string[] keys = GetKeys(Skills);
            ViewHeight = 0;
            foreach (string key in keys)
            {
                ViewHeight += Skills[key].Count * Skill.Count * 30;
            }
        }

    }

    string[] GetKeys(IDictionary item)
    {
        string[] keys = new string[item.Count];
        item.Keys.CopyTo(keys, 0);
        return keys;
    }

    void GetData<T>(IDictionary item, T instance)
    {
        Type type = typeof(T);
        string[] ItemKeys = GetKeys(item);
        for (int i = 0; i < ItemKeys.Length; i++)
        {
            item[ItemKeys[i]] = type.GetField(ElementName[ItemKeys[i]]).GetValue(instance);
        }
    }

    void SetData<T>(IDictionary item, T instance)
    {
        Type type = typeof(T);
        string[] ItemKeys = GetKeys(item);
        for (int i = 0; i < ItemKeys.Length; i++)
        {
            type.GetField(ElementName[ItemKeys[i]]).SetValue(instance, item[ItemKeys[i]]);
        }

    }

    void LoadAllData(Card card)
    {
        GetData<Card>(FundamentalElements, card);
        GetData<CardAttributesSerialization>(AttritubesElements, card.Attributes);
        GetData<Card>(Skills, card);
        
    }

    public void LoadCard()
    {
        CardXML = new CardXML(FundamentalElements["卡牌编号"]);
        CardXML.ReadXML();
        LoadAllData(CardXML.Card);
        Card.GetComponent<SpriteRenderer>().sprite = Config.config.GetCardSprite(CardXML.Card.ID);
    }

    

    public void SaveCard()
    {
        Card card = new Card();
        if (CardXML != null)
        {
            card = CardXML.Card;
        }
        CardXML = new CardXML(FundamentalElements["卡牌编号"]);
        SetData<Card>(FundamentalElements, card);
        SetData<CardAttributesSerialization>(AttritubesElements, card.Attributes);
        SetData<Card>(Skills, card);
        CardXML.WriteXML(card);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
