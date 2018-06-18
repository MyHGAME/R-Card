using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对战阶段卡类
public class RCard : Base 
{
	public CardXML Card;//捆绑卡类信息
    public SkillBase Skill;//技能类
	public Area CardArea = Area.None;//区域枚举
    public string CardImagePath;//卡的图片
	public string MapNode;//地图的节点
    //让技能管理获取的委托，表示这张卡响应的动作，在卡牌上时
    //有出场（发动前要先出场），埋伏，在场上时，有攻击，其他技能效果在技能类处理
    public delegate void ResponseAction();
    public ResponseAction Action;
    public Dictionary<Area, ResponseAction> ActionTable = new Dictionary<Area, ResponseAction>();
	public Dictionary<string,bool> Ability = new Dictionary<string, bool>();
	public Dictionary<string,bool> State = new Dictionary<string, bool>();

	protected virtual void Init()
	{
		//委托事件加入，变量初始化
		//能力和状态
		Ability.Add (Config.config.Appearance,true);
		Ability.Add (Config.config.Flop, true);
		State.Add (Config.config.Appearance, false);
		State.Add (Config.config.Flop, false);
        ActionTable.Add(Area.Hand, AppearanceOrder);

	}
     
	public virtual void AppearanceOrder()
	{
		//出场命令 通知：xxx 出场 事件：出场
	}

	public virtual void Appearance()
	{
		//出场
	}

	public virtual void RationalizeParameters()
	{
		//参数合理化
	}

	public virtual void FlopOrder()
	{
		//翻牌命令 通知：xxx 翻牌 事件：翻牌
	}

	public virtual void Flop()
	{
		//回手牌
	}

	public virtual void ShowCard()
	{
		//显示卡牌
	}


	public virtual void Response()
	{
        //响应
        Action = ActionTable[CardArea];

    }

	// Use this for initialization
	void Start () 
	{
		
	}
	

}
