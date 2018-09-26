using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibraryXML : XMLOperator<SerializableCardLibrary> {

    public SerializableCardLibrary CardLibrary 
    {
        get
        {
            return Item;
        }
        
    }
    
     public CardLibraryXML(string name): base(name)
    {

    }
}
