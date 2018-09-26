using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Area{
	None,
	ActionField,//场上活动区
	CardGroup,//卡组
	Hand,//手牌
	FlopArea,//翻牌区
    UseCardArea,//出牌区
    FoldArea,//弃牌区
    PlayerCardArea,//玩家卡片区域

}

//配置类，可以序列化
[Serializable]
public class Config : MonoBehaviour {
	public static Config config;
    public string LoadPath;
    public string PrefabPath = "Prefabs/";
    public string SpritePath = "Sprite/";
    public string CardPath = "Card/";
    public string MapPath = "Map/";
    public string CardLibraryPath = "CardLibrary/";
    public string CardGroupPath = "CardGroup/";
    public string CardGroupManagerPath = "CardGroupManager/";
    public string XMLPath = "XML/";
    public string XMLFormatName = ".xml";
    public string ImageFormatName = ".png";
	public string Appearance = "出场";
	public string Flop = "翻牌";
	public string Attack = "攻击";
	public string Move = "移动";
    public string[] Phase = { "开始阶段","翻牌阶段","主要阶段","战斗阶段","调整阶段","结束阶段" };
    public string[] AutoPhase = { "开始阶段", "结束阶段" };
    public float MaxEventIntervalTime = 1.0f;
    public Dictionary<string,string> CardTypeTable = new Dictionary<string, string> ();
    public Dictionary<Type, string> XMLPaths = new Dictionary<Type, string>();
    public Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>()
    {
        {"Card",null},
        {"MapNode",null},
        {"Background" ,null},
        
    };
    public Dictionary<Type, Area> ScriptToArea = new Dictionary<Type, Area>() 
    {
        {typeof(CardGroup),Area.CardGroup},
        {typeof(Hand),Area.Hand},
        {typeof(FoldArea),Area.FoldArea},
        {typeof(FlopArea),Area.FlopArea},
        {typeof(UseCardArea),Area.UseCardArea},
    };
    public Dictionary<CardGroupType, int> CardGroupMaxCount = new Dictionary<CardGroupType, int>()
    {
        {CardGroupType.Main,70},
        {CardGroupType.Replace,15},
    };
    //方法1，地图生成，最大地图边界，用来生成地图边界，限制50x50，用空的格子，用来生成地图节点
    //地图节点包括区域说明，
    public int MapMaxX = 50, MapMaxY = 50;
    public float HorizontalInterval = 4.15f, VerticalInterval = 5.2f;
    public LayerMask ActionLayer;

    void Init()
	{
        config = this;
        LoadPath = Application.dataPath + "/";

        //普通单位，不能升级，可以移动攻击，生命值等于低于0，送去弃牌区
        //英雄单位，可以升级
        //辅助单位，不能升级，不能攻击，不能移动，翻开时，回到手牌，可以在自己场上任意合理位置出场
        //普通技能，在自己的主要阶段和调整阶段使用，翻开回到手牌，发动完毕送去弃牌区
        //装备技能，在自己主要阶段和调整阶段使用，翻开回到手牌，出场时选择一个装备的单位叠在那个单位下面
        //那个单位不在场,装备效果消失，送去弃牌区
        //反击技能，满足条件可发动，发动完毕送去弃牌区
        //陷阱卡需要触发，出场时埋伏在地图节点上，当单位卡出现在那个埋伏的节点时,，触发陷阱卡
        //普通陷阱触发后，发动完毕送去弃牌区
        //固定陷阱触发后，会一直跟随那个单位卡，单位卡不在场上，那个陷阱送去弃牌区，效果消失。否则效果一直存在
        //辅助物，例如血包，有陷阱卡一般触发功能，单位碰了会产生正面效果，一般是游戏内生成，没有这种卡类
        CardTypeTable.Add("玩家", "玩家");
        CardTypeTable.Add("普通单位", "单位");
		CardTypeTable.Add("英雄单位", "单位");
		CardTypeTable.Add("辅助单位", "单位");
		CardTypeTable.Add("普通技能", "技能");
		CardTypeTable.Add("装备技能", "技能");
		CardTypeTable.Add("反击技能", "技能");
        CardTypeTable.Add("物品", "触发");
        CardTypeTable.Add("陷阱", "触发");
        CardTypeTable.Add("普通陷阱", "陷阱");
        CardTypeTable.Add("固定陷阱", "陷阱");
        CardTypeTable.Add("普通物品", "物品");
        CardTypeTable.Add("固体物品", "物品");
        XMLPaths.Add(typeof(Card), CardPath+XMLPath);
        XMLPaths.Add(typeof(SerializableMapNodes), MapPath + XMLPath);
        XMLPaths.Add(typeof(SerializableCardLibrary), CardLibraryPath + XMLPath);
        XMLPaths.Add(typeof(SerializableCardGroup), CardGroupPath + XMLPath);
        XMLPaths.Add(typeof(SerializableGroupManager), CardGroupManagerPath + XMLPath);
        ActionLayer.value = LayerMask.NameToLayer("Action");

       // StartCoroutine(LoadPrefab());
        LoadPrefab();
    }

    public string GetCardType(string type)
    {
        if (CardTypeTable.ContainsKey(type))
        {
            return CardTypeTable[type];
        }
        else
        {
            return "";
        }
    }

  

    void LoadPrefab()
    {
        List<string> ls = new List<string>();
        ls.AddRange(Prefabs.Keys);
        foreach (string key in ls)
        {   
            Prefabs[key] = (GameObject)Resources.Load(PrefabPath + key);
        }
    }

   

    public Vector3 GetCurrentPos(int x,int y)
    {
        return new Vector3(x * HorizontalInterval, 0, y * VerticalInterval);
    }

    public string GetXMLFilePath(Type xml_type,string xml_name)
    {
        string File = LoadPath + XMLPaths[xml_type] + xml_name + XMLFormatName;
        return File;
    }

    public string GetCardSpritePath(string CardID)
    {
        return SpritePath + CardPath + CardID;
    }

    public Sprite GetCardBackSprite()
    {
        return Resources.Load<Sprite>(SpritePath + "CardBack/CardBack");
    }

    public Sprite GetCardSprite(string CardID)
    {
        Sprite sprite = Resources.Load<Sprite>(GetCardSpritePath(CardID));
        return sprite != null ? sprite : Resources.Load<Sprite>(GetCardSpritePath("C000000"));
    }

	void Awake()
	{
        Init();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
   
	}
}
