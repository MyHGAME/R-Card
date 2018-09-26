using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableGroupManager  {
    public string MainGroupName;
    public List<string> GroupNames = new List<string>();

    public SerializableGroupManager()
    {
 
    }

    public void SetMainGroupName(string name)
    {
        MainGroupName = name;
    }

    public void AddGroup(string GroupName)
    {
        GroupNames.Add(GroupName);
    }
}
