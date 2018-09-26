using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Base {
    //临时
    public static BattleManager battleManager;
    private Dictionary<string, string> playerMapping = new Dictionary<string, string>();
    public Dictionary<string, string> PlayerMapping
    {
        get 
        {
            return playerMapping;
        }
    }
    // Use this for initialization
    void Awake()
    {
        battleManager = this;
        Owner = "b";//设置玩家属于哪个区域
    }

    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void SetOwner(string playername)
    {
        Owner = playerMapping[playername];
    }

    void CreatOwnerMapping(List<string> Players)
    {
        playerMapping.Clear();
        char start = 'a';
        foreach (string player in Players)
        {
            playerMapping.Add(player, start.ToString());
            start ++ ;
        }
    }

	public bool Belong(GameObject judged)//查看所属方,通过映射，现在未设置映射，因为地图节点不要用abc代替，需要映射
	{
        if (judged == null) return false;
        Base temp = judged.GetComponent<Base>();
		return temp.Owner == Owner; 
	}
    public bool Belong(string judged)
    {
        return Owner == playerMapping[judged];
    }
}
