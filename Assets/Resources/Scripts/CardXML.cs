using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

//可以序列化的字典类，继承了字典类和xml可序列化的接口，修改写入方法和读取方法
[Serializable]  
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable  
{  
	public SerializableDictionary() { }  
	public void WriteXml(XmlWriter write)       // Serializer  
	{  
		XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));  
		XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));  

		foreach (KeyValuePair<TKey, TValue> kv in this)  
		{   
			KeySerializer.Serialize(write, kv.Key);    
			ValueSerializer.Serialize(write, kv.Value);  
		}  
	}  
	public void ReadXml(XmlReader reader)       // Deserializer  
	{  
		reader.Read();  
		XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));  
		XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));  

		while (reader.NodeType != XmlNodeType.EndElement)  
		{   
			TKey tk = (TKey)KeySerializer.Deserialize(reader);  
			TValue vl = (TValue)ValueSerializer.Deserialize(reader);    
			this.Add(tk, vl);  
			reader.MoveToContent();  
		}  
		reader.ReadEndElement();  

	}  
	public XmlSchema GetSchema()  
	{  
		return null;  
	}  
}

[Serializable]
public class GenericSerialization
{
	public SerializableDictionary<string,SerializableDictionary<string,string>> Skill = new SerializableDictionary<string,SerializableDictionary<string,string>>();

	public void Add(string skill_name,string detail_type,string detail)
	{
		SerializableDictionary<string,string> temp = new SerializableDictionary<string, string> ();
		temp.Add (detail_type, detail);
		Skill.Add (skill_name, temp);
	}

	public void AddDetailType(string skill_name,string detail_type,string detail)
	{
		Skill [skill_name].Add (detail_type,detail); 
	}
}

//等级参数表的字典类，每一等级对应一个参数，若是每等级都有自己规律，例如lv*10+20可以用<等级x>x*10+20</等级x>
[Serializable]  
public class ParameterDictionary: Dictionary<string, string>, IXmlSerializable  
{  
	public static string  TheKeyHead = "等级";

	public ParameterDictionary() { }  
	public void WriteXml(XmlWriter write)       // Serializer  
	{  
		foreach (string  key in this.Keys)  
		{ 
			write.WriteElementString (TheKeyHead+key,this[key]);
		}  
	}  
	public void ReadXml(XmlReader reader)       // Deserializer  
	{  			
		reader.Read();
		while (reader.NodeType != XmlNodeType.EndElement)
		{
			string key = reader.Name.Replace(TheKeyHead,"");
			this[key] = reader.ReadElementString();//自动下一行,获取元素里的值
		}

	}  
	public XmlSchema GetSchema()  
	{  
		return null;  
	}  
} 

//用来序列化的属性类
[Serializable]
public class CardAttributesSerialization
{
	public string MaxLife,MaxEnergy;		//生命，能量
	public string Attack,Defence;		//攻击力，防御力
	public string AttackDistance,MovingDistance;		//攻击距离，移动距离

	public CardAttributesSerialization()
	{
        MaxLife = "";
        MaxEnergy = "";
        Attack = "";
        Defence = "";
        AttackDistance = "";
        MovingDistance = "";
    }

	public CardAttributesSerialization(string life,string energy,string attack,string defence,string AD,string MD)
	{
		MaxLife = life;
		MaxEnergy = energy;
		Attack = attack;
		Defence = defence;
		AttackDistance = AD;
		MovingDistance = MD;
	}
}
	
//序列化技能描述
[Serializable]
public class Skill
{
	public string Name;
	public string Cost;
	public string Degree;
	public string Content;

	public Skill()
	{
        Name = "";
        Cost = "";
        Degree = "";
        Content = "";
    }

	public Skill(string name,string cost,string degree,string content)
	{
		Name = name;
		Cost = cost;
		Degree = degree;
		Content = content;
	}


}

//方便捆绑键值对
[Serializable]
public class KeyValue
{
	public string Key;
	public string Value;

	public KeyValue()
	{
        Key = "";
        Value = "";
	}
	public KeyValue(string k,string v)
	{
		Key = k;
		Value = v;
	}
}

//动态参数序列化
[Serializable]
public class Parameter
{
	public string Name;
	public ParameterDictionary Level = new ParameterDictionary ();

	public Parameter()
	{
        Name = "";
    }

	public Parameter(string name,params KeyValue[] levels)
	{
		Name = name;
		for (int i = 0; i < levels.Length; i++)
		{
			Level.Add (levels [i].Key,levels[i].Value);
		}
	}

    public string GetParmeterDetail()
    {
        string detail = "";
        string level = "(等级";
        foreach (string key in Level.Keys)
        {
            level += key + "/";
            detail += Level[key] + "/";
        }
        level = level.Remove(level.Length -1);
        level += ")";
        detail = detail .Remove(detail.Length -1);
        detail += level;
        return detail;
    }
}

//卡类
[Serializable]
public class Card
{
    public string ID;		//卡牌id
	public string Type;		//卡牌类型
	public string Title,Name;		//卡牌称号，名字
	public CardAttributesSerialization Attributes;		//字符类型的卡牌属性类
	public List<Skill> PassiveSkills = new List<Skill>();		//被动技能
    public List<Skill> CouterSkills = new List<Skill>();        //反制技能
	public List<Skill> ActiveSkills = new List<Skill>();		//主动技能
	public List<Parameter> ParameterList = new List<Parameter>();
	public static string Passive = "PassiveSkills";//为了区分主动技能
	public static string Active = "ActiveSkills";

	public Card()
	{
        ID = "";
        Type = "";
        Title = "";
        Name = "";
        Attributes = new CardAttributesSerialization();
    }

	public Card(string id,string type,string title,string name = "")
	{
		ID = id;
		Type = type;
		Title = title;
		Name = name;
	}

	public void AddSkills(string skill_type,params Skill[] skills)
	{
		for (int i = 0; i < skills.Length; i++) 
		{
			if(skill_type == Passive)
				PassiveSkills.Add (skills [i]);
			else if(skill_type == Active)
				ActiveSkills.Add(skills[i]);
		}
	}
	public void AddParameters(params Parameter[] parameter)
	{
		for (int i = 0; i < parameter.Length; i++)
		{
			ParameterList.Add (parameter [i]);
		}
	}

	public void AddAttributes(CardAttributesSerialization attribute)
	{
		Attributes = attribute;
	}

    public string GetCurrentDetail(string detail)
    {
        List<string> ParmeterNames = GetParmeterName(detail);
        foreach (Parameter parmeter in this.ParameterList)
        {
            foreach(string name in ParmeterNames)
            {
                if (name == parmeter.Name )
                {
                    
                   detail = detail.Replace(name, parmeter.GetParmeterDetail());
                } 
            }
       }
       detail = detail.Replace("{", "");
       detail = detail.Replace("}", "");
       return detail;
    }

    private List<string> GetParmeterName(string detail)
    {
        List<string> ParmeterNames =new List<string>();//一个描述最多十个参数
        for(int i = 0;i < detail.Length;i++)
        {
            if (detail[i] == '{')//需要config配置用什么符号
            {
                int j = i + 1;
                string temp = "";
                while (detail[j] != '}')
                {
                    temp += detail[j];
                   
                    j++;
                }
               
                ParmeterNames.Add(temp);
                i = j;
            }
        }
        return ParmeterNames;
    }

    public string GetCurrentName()
    {
        return Title + " " + Name;
    }
}
//指定好xml文件名称，可以序列化
public class XMLOperator<T>
{
    public string XMLName;
    private string FileName;
    protected T Item;

    public XMLOperator()
    {
    }

    public XMLOperator(string name)
    {
        SetName(name);
    }

    public  void SetName(string name)
    {
        XMLName = name;
        FileName = Config.config.GetXMLFilePath(typeof(T), XMLName);
    }

    public void ReadXML(string name)
    {
        SetName(name);
        ReadXML();
    }

    public T ReadXML()
    {
       
        //未进行错误判断,需要对xml读写操作封装，只需要指定好类，就可以序列化相应的类
        FileStream fs = new FileStream(FileName, FileMode.Open);
        XmlSerializer serialize = new XmlSerializer(typeof(T));
        T Temp = (T)serialize.Deserialize(fs);
        Item = Temp;
        fs.Close();
        return Temp;
    }

    public void WriteXML(T Temp)
    {
        FileStream fs = new FileStream(FileName, FileMode.Create);
        XmlSerializer serialize = new XmlSerializer(typeof(T));
        serialize.Serialize(fs, Temp);
        Item = Temp;
        fs.Close();
    }

    public void WriteXML()
    {
        WriteXML(this.Item);
    }
}

//cardxml，rcard类读取这个来标识卡类
public class CardXML : XMLOperator<Card>
{
    public Card Card
    {
        get
        {
            return Item;
        }
        set
        {
            Item = value;
        }
    }

    public CardXML(string name) : base(name)
    {

    }



}
