using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManagerXML : XMLOperator<SerializableGroupManager> {
    public SerializableGroupManager GroupManager
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

    public GroupManagerXML(string name):base(name)
    { }
}
