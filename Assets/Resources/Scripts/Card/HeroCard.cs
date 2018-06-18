using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCard : OrdinaryCard {

	// Use this for initialization
	void Start () {
		
	}


	public override void Flop ()
	{
		base.Flop ();
	}

	public void LevelUp()
	{
		this.PresentAttribute.Level += 1;
		HandingDynamicVariables ();
	}

	private void HandingDynamicVariables()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
