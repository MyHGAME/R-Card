using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCarry : CardList {
    public GameObject UpperCard
    {
        get 
        {
            return RCardGroup[RCardGroup.Count - 1];
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void RCardPosChange(GameObject card)
    {
        Vector3 pos = this.transform.position;
        pos.y += RCardGroupHeightIncrement;
        card.transform.position = pos;
    }
}
