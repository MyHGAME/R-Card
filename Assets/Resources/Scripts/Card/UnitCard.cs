using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Attribute
{
	public int MaxLife,MaxEnergy;
	public int Level;
	public int Life,Energy;
	public int Attack,Defence;
	public int AttackDistance,MovingDistance;
}

public class UnitCard : RCard {
	public Attribute PresentAttribute;
	public Attribute OriginalAttribute;
    public List<GameObject> Equipments,Traps;
	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		
	}
		
}
