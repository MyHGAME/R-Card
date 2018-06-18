using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Base {
    //临时
    public static BattleManager battleManager;
    // Use this for initialization

    void Awake()
    {
        battleManager = this;
        Owner = "b";
    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Belong(GameObject judged)//查看所属方
	{
		Base temp = judged.GetComponent<Base> ();
		if (temp == null) return false;
		return temp.Owner == Owner ? true : false; 
	}
    public bool Belong(string judged)
    {
        return Owner == judged ? true : false;
    }
}
