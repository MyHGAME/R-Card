using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldArea : CardList {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AcquireRCard(MapManager.mapManager.GetOwnerAreaScript<CardGroup>().DistributionRCard(this.gameObject));
        }
	}

   
}
