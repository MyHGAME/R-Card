using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CreatCard : MonoBehaviour {
	public string XMLName = "";

	// Use this for initialization
	void Start () {
       
	}

	Card AssignCardSerialization()
	{
		Card TempCard = new Card("C000000","英雄单位","双子猎杀者","LR");
		TempCard.AddAttributes(new CardAttributesSerialization ("30","5","{attack}","20","1","3"));
		TempCard.AddSkills(Card.Passive,
			new Skill("击破","","",
				"这张卡破坏一个对方单位卡时，可选择1距离的位置进行移动，然后可选择1距离的对方单位卡，对那个单位造成{hurt}穿透伤害")
		);
		TempCard.AddSkills (Card.Active,
			new Skill ("双子之力","-1 E","笨拙","直到回合结束前,攻击力或者防御力提高{attack}"),
			new Skill ("猎杀之力", "-1 E", "笨拙", "对2距离内的一个对方单位造成{hurt}穿透伤害")
		);
		TempCard.AddParameters(
			new Parameter("attack",new KeyValue("1","20"),new KeyValue("3","30"),new KeyValue("5","40")),
			new Parameter("hurt",new KeyValue("1","10"),new KeyValue("3","15"),new KeyValue("5","20"))
		);
		return TempCard;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
