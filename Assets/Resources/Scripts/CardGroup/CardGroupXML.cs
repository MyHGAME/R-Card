using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroupXML : XMLOperator<SerializableCardGroup>
{

    public SerializableCardGroup CardGroup
    {
        get
        {
            return Item;
        }
        set
        {
            Item = value;
        }
    }

     public CardGroupXML(string name): base(name)
    {

    }
}
