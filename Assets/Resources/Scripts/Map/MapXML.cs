using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MapXML:XMLOperator<SerializableMapNodes>
{ 
    public SerializableMapNodes Map
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

    public MapXML(string name) : base(name)
    {

    }
}
