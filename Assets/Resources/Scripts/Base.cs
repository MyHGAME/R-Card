using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Base : MonoBehaviour {
	private string owner;//标识所属方

    public string Owner
    {
        get
        {
            return owner;
        }

        set
        {
            owner = value;
        }
    }

    public void SetOwner(string owner)//设置所属方
	{
		Owner = owner;
	}
		
}
