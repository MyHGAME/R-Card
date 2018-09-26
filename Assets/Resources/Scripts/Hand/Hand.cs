using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : CardList {

    public float CardPostionX = 0,CardPositionXIncrement = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.J))
        {
            AcquireRCard(MapManager.mapManager.GetOwnerAreaScript<CardGroup>().DistributionRCard(this.gameObject));
        }

	}

    public override void RCardPosChange(GameObject card)
    {
        card.transform.parent = this.transform;
        card.transform.localPosition = new Vector3(CardPostionX, 0, 0);
        CardPostionX -= CardPositionXIncrement;
        card.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
