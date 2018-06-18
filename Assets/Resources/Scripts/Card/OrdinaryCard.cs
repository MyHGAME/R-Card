using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinaryCard : UnitCard {

	protected override void Init ()
	{
		base.Init ();
		Ability.Add (Config.config.Attack,true);
		Ability.Add (Config.config.Move, true);
        ActionTable.Add(Area.ActionField, AttackOrder);
	}
	// Use this for initialization
	void Start () {
		
	}

	public override void Flop ()
	{
		if (!Ability [Config.config.Appearance])
			base.Flop ();
		else 
		{
			
		}
	}

	public void AttackOrder()
	{
		
	}

	public void SelectAttackAim()
	{
		
	}

	public void ReadyAttackAim()
	{
		
	}

	public void AttakAim()
	{
		
	}

	public void HurnOrder()
	{
		
	}

	public void HurnAim()
	{
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
