using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AreaBase:Base  {
    public Area area = Area.None;

    public Area Area
    {
        get
        {
            return area;
        }

        set
        {
            area = value;
        }
    }
}
