using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardGroup : CardList {
   // public static CardGroup Instance; 
    public CardGroupXML CardGroupXML;
    public GroupManagerXML GroupManagerXML;
    public Vector3 RCardGroupRotation = new Vector3(270, 0, 0);
    void Awake()
    {
      //  Instance = this;
        GroupManagerXML = new GroupManagerXML("CardGroupManager");
        GroupManagerXML.ReadXML();
        CardGroupXML = new CardGroupXML(GroupManagerXML.GroupManager.MainGroupName);
        CardGroupXML.ReadXML();
    }
    public bool LoadCardGroup(string name)
    {
        if (GroupManagerXML.GroupManager.GroupNames.Contains(name))
        {
            CardGroupXML = new CardGroupXML(name);
            CardGroupXML.ReadXML();
            return true;
        }
        return false;
    }


    public void CreatRCardGroup()
    {
        Vector3 StartPos = this.gameObject.transform.position;
        foreach(string id in CardGroupXML.CardGroup.MainCardIDList)
        {
            GameObject temp = (GameObject)Instantiate(Config.config.Prefabs["Card"], StartPos,Quaternion.identity);
            temp.AddComponent<RCard>().CardXML = new CardXML(id);
            temp.GetComponent<RCard>().CardXML.ReadXML();
            temp.GetComponent<RCard>().CardArea = Area.CardGroup;
            temp.transform.localEulerAngles = RCardGroupRotation;
            StartPos.y += RCardGroupHeightIncrement;
            RCardGroup.Add(temp);
        }
        Shuffle();
    }

    

    public void SetMainCardGroup(string name)
    {
        GroupManagerXML.GroupManager.SetMainGroupName(name);
        GroupManagerXML.WriteXML();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AcquireRCard(MapManager.mapManager.GetOwnerAreaScript<FoldArea>().DistributionRCard(this.gameObject));
        }
	}

    //位置1表示卡组的顶端，表示count-1的位置,失去卡牌
    public GameObject DistributionRCard(GameObject AimArea,int pos = 1,bool shuffle = false)
    {
       GameObject TheRCard = base.DistributionRCard(AimArea, pos);
        if (shuffle) Shuffle();
        return TheRCard;
    }

    //获得卡牌
    public void AcquireRCard(GameObject card, int pos = 1, bool shuffle = false)
    {
        base.AcquireRCard(card, pos);
        if (shuffle) Shuffle();
    }

  

    //洗牌,需要优化
    public void Shuffle()
    { 
       for (int i = 0; i < RCardGroup.Count; i++)
       {
            int index = Random.Range(0,RCardGroup.Count);
            GameObject obj = RCardGroup[i];
            Vector3 pos = obj.transform.position;
            RCardGroup[i].transform.position = RCardGroup[index].transform.position;
            RCardGroup[index].transform.position = pos;
            RCardGroup[i] = RCardGroup[index];
            RCardGroup[index] = obj;
        }
    }
}
