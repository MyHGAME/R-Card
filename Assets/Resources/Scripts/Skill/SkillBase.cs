using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour {
    public Skill Skill;
    //技能图片根据卡的ID获取
    public delegate void temlate();
    public delegate bool ConditionsTemlate();
    public List<ConditionsTemlate> ActiveConditions;  //主动条件类顺便触发条件ui
    public List<temlate> ActiveContents;       //根据技能类来分配
    public List<temlate> ActiveSkills;  //主动效果，玩家发动 满足条件ui提示
    public List<temlate> PassiveSkills; //被动效果,按顺序插入到事件序列 满足条件直接执行
    public List<temlate> CouterSkills; //反制效果,插入到事件序列的前面 满足条件执行询问
    /*
    被动技能：
    void passive（）
    {
        if（条件）
        {
        发动（内容）；
        }

    }

    反制技能：
    void couter（）
    {
        if（条件）
        {
        询问发动（内容）；
        }
    }

    主动技能：
    技能发动：
条件判断：
条件回调
发动（内容）-----若发动被无效破坏了----条件回调也发动了

内容：
事件锁
实际内容
生命周期
解锁
    */
	// Use this for initialization
	void Start () {
        SendSkillDetection();
	}

    void SendSkillDetection()
    {
        //将被动和反击技能传到事件循环
    }
    // Update is called once per frame
    void Update () {
		
	}
}
