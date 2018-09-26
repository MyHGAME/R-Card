using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour {
    //生成地图，载入地图xml信息后生成地图
    public static MapManager mapManager;
    public string MapName;
    private MapXML MapXML;
    public int HorizontalLength, VerticalLength,MaxY,MaxX,MinX,MinY;
   //地图信息，存储各种信息，手牌存有手牌的实例化物体
    public Dictionary<Area, List<GameObject>> Map = new Dictionary<Area, List<GameObject>>()
    {
        { Area.ActionField,new List<GameObject>()},
        { Area.CardGroup,new List<GameObject>()},
        { Area.UseCardArea,new List<GameObject>()},
        { Area.FlopArea,new List<GameObject>()},
        { Area.FoldArea,new List<GameObject>()},
        { Area.Hand,new List<GameObject>()},
        { Area.PlayerCardArea,new List<GameObject>()},
        { Area.None,new List<GameObject>()}
    };

    public List<GameObject> ActionFields
    {
        get
        {
            return Map[Area.ActionField];
        }
        set
        {
            Map[Area.ActionField] = value;
        }
    }

    public Vector3 GetCenter
    {
        get
        {
            return Config.config.GetCurrentPos((MaxX + MinX) / 2, (MaxY + MinY) / 2);
        }
    }

    private void Awake()
    {
        mapManager = this;
    }

    // Use this for initialization
    void Start () {
        LoadMap();
        AddAreaScript();
    }

    public void LoadMap()
    {
        MapXML = new MapXML(MapName);
        MapXML.ReadXML();
        GetMapInformation(MapXML.Map);
        NodeInstantiation();
    }

    void AddAreaScript()
    {
        GetOwnerArea(Area.CardGroup).AddComponent<CardGroup>().CreatRCardGroup();
        GetOwnerArea(Area.Hand).AddComponent<Hand>();
        GetOwnerArea(Area.FoldArea).AddComponent<FoldArea>();
        foreach (GameObject obj in GetOwnerAreaList(Area.UseCardArea)){ obj.AddComponent<UseCardArea>(); }
        foreach (GameObject obj in GetOwnerAreaList(Area.FlopArea)){obj.AddComponent<FlopArea>(); }
    }

    //获取活动区域的列表节点
    public int GetPos(int NodeX,int NodeY)
    {
        return (NodeX - MinX) * VerticalLength + (NodeY - MinY);
    }
   
    //获取区域信息
    public GameObject GetMapNode(int NodeX,int NodeY, Area area = Area.ActionField)
    {
        switch (area)
        {
            case Area.ActionField:
                return ActionFields[GetPos(NodeX, NodeY)];
            default:
                return null;
        }
    }

    public T GetOwnerAreaScript<T>()
    {
        return GetOwnerArea(Config.config.ScriptToArea[typeof(T)]).GetComponent<T>();
    }

    //生成地图节点,临时
    void NodeInstantiation()
    {
        for (int i = 0; i < HorizontalLength; i++)
        {
            for (int j = 0; j < VerticalLength; j++)
            {
                Map[Area.ActionField].Add(null);
            }
        }
        foreach (SerializableMapNode item in MapXML.Map.MapNodes)
        {
            GameObject obj = (GameObject)Instantiate(Config.config.Prefabs["MapNode"],
                   Config.config.GetCurrentPos(item.Node.x, item.Node.y),
                   Config.config.Prefabs["MapNode"].transform.rotation
                    );
            if (item.Area == Area.ActionField)
            {
                int pos = GetPos(item.Node.x, item.Node.y);
                Map[item.Area][pos] = obj;
            }
            else
            {
                Map[item.Area].Add(obj);
            }
            obj.GetComponent<MapNode>().SerializableMapNode = item;
            //临时
            obj.GetComponent<AreaBase>().Owner = item.Owner;
        }
        GameObject hand = GameObject.Find("Hand");
        hand.GetComponent<AreaBase>().Owner = hand.GetComponent<MapNode>().SerializableMapNode.Owner;
        Map[Area.Hand].Add(hand);

    }

    //获取地图信息，边界，长度等
    void GetMapInformation(SerializableMapNodes temp)
    {
        MaxX = temp.MapNodes[0].Node.x;
        MaxY = temp.MapNodes[0].Node.y;
        MinX = MaxX;
        MinY = MaxY;
        foreach (SerializableMapNode item in temp.MapNodes)
        {
            if (item.Area == Area.ActionField)
            {
                Comparing(item.Node.x,ref MaxX, '>');
                Comparing(item.Node.x, ref MinX, '<');
                Comparing(item.Node.y,ref MaxY, '>');
                Comparing(item.Node.y, ref MinY, '<');
            }
        }
        HorizontalLength = MaxX - MinX + 1;
        VerticalLength = MaxY - MinY + 1;
    }

    //若a比b大或者小，将a赋值给b
    void Comparing(int a, ref int b, char oper)
    {
        if (oper == '>')
        {
            if (a > b)
                b = a;
        }
        else if (oper == '<')
        {
            if (a < b)
                b = a;
        }


    }

    public GameObject GetOwnerArea(Area area)
    {
        GameObject result = null;
        foreach (GameObject obj in Map[area])
        {
            if (BattleManager.battleManager.Belong(obj))
            {
                result = obj;
                break;
            }
        }
        return result;
    }

    public List<GameObject> GetOwnerAreaList(Area area)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (GameObject obj in Map[area])
        {
            if (BattleManager.battleManager.Belong(obj))
            {
                result.Add(obj);
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
