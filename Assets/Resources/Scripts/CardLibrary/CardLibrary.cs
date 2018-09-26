using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance;
    public CardLibraryXML CardLibraryXML;
	// Use this for initialization
	void Awake () {
        Instance = this;
        CardLibraryXML = new CardLibraryXML("CardLibrary");
        CardLibraryXML.ReadXML();
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  
   

   /* public Card GetCard()
    {
 
    }*/
}
