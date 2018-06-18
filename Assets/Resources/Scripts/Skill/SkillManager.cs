using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public RCard RCard;
    public delegate void CurrentSkill();
	// Use this for initialization
	void Start () {
		
	}

    public void GetCard(RCard card)
    {
        RCard = card;
        ShowSkill();
    }

    /*
     调用技能UI类，将这个效果的类传递过去，调用显示UI函数，
     根据委托列表的，若为空则不显示，
     若根据条件而且是自己的卡否则无法使用则不激活且不准使用
     */
    void ShowSkill()
    {

    }

	// Update is called once per frame
	void Update () {

    }
}
