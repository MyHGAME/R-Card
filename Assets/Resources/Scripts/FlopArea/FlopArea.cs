using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlopArea : CardCarry {
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AcquireRCard(MapManager.mapManager.GetOwnerAreaScript<CardGroup>().DistributionRCard(this.gameObject));

        }
	}
        

}
