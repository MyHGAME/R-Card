using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//地图编辑
public class MapEditor : MonoBehaviour {

    public string MapXMLPath,PlayerCamps,NeutralCamps;
    public List<GameObject> BackgroundNodes = new List<GameObject>();
    public List<GameObject> MapNodes = new List<GameObject>();
    public RaycastHit hit;
    private GameObject Background, MapNodePrefab;
    // Use this for initialization
    void Start () {
        Init();
        BackgroundGenerator();
	}

    void Init()
    {
        Background = Config.config.Prefabs["Background"];
        MapNodePrefab = Config.config.Prefabs["MapNode"];
    }

    //地图节点承载背景生成
    void BackgroundGenerator()
    {
        if (Background == null)
            return;
        for (int i = 0;i < Config.config.MapMaxX;i++)
        {
            for (int j = 0;j < Config.config.MapMaxY;j++)
            {
                GameObject background = Instantiate(
                    Background,
                    new Vector3(Config.config.HorizontalInterval*i,0,Config.config.VerticalInterval*j),
                    Background.transform.rotation);
                background.GetComponent<MapBackground>().Node = new Node(i,j);
                BackgroundNodes.Add(background);
            }
        }
    }

    //删除空的节点
    void CheckMapNodes()
    {
        List<GameObject> NullObjList = new List<GameObject>();
        foreach (GameObject obj in MapNodes)
        {
            if (obj == null)
                NullObjList.Add(obj);
        }
        foreach (GameObject obj in NullObjList)
        {
            MapNodes.Remove(obj);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 80, 30), "地图名称");
        GUI.Label(new Rect(0, 40, 80, 30), "玩家阵营");
        GUI.Label(new Rect(0, 80, 80, 30), "中立阵营");
        MapXMLPath = GUI.TextField(new Rect(80, 0, 100, 30),MapXMLPath);
        PlayerCamps = GUI.TextField(new Rect(80, 40, 100, 30), PlayerCamps);
        NeutralCamps = GUI.TextField(new Rect(80, 80, 100, 30), NeutralCamps);
        if (GUI.Button(new Rect(0, 120, 100, 30), "保存MapXML"))
        {
            SaveMap();
        }
        if (GUI.Button(new Rect(0, 160, 100, 30), "载入MapXML"))
        {
            LoadMap();
        }
        if (GUI.Button(new Rect(0, 200, 100, 30), "重置场景"))
        {
            ResetMap();
        }
    }

    //保存成xml
    void SaveMap()
    {
        if (MapNodes.Count != 0)
        {
            MapXML MapXML = new MapXML(MapXMLPath);
            SerializableMapNodes serializableMapNodes = new SerializableMapNodes(MapNodes, PlayerCamps, NeutralCamps);
            MapXML.WriteXML(serializableMapNodes);
        }
        else
        {
            Debug.Log("没有地图节点");
        }
    }

    void ResetMap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //加载地图
    void LoadMap()
    {

        MapXML MapXML = new MapXML(MapXMLPath);
        MapXML.ReadXML();
        PlayerCamps = LoadCamps(MapXML.Map.PlayerCamps);
        NeutralCamps = LoadCamps(MapXML.Map.NeutralCamps);
        foreach (SerializableMapNode map_node in MapXML.Map.MapNodes)
        {
            int index = map_node.Node.x * Config.config.MapMaxY + map_node.Node.y;
            GameObject obj = BackgroundNodes[index].GetComponent<MapBackground>().NodeGenerator(MapNodePrefab);
            MapNode obj_node = obj.GetComponent<MapNode>();
            obj_node.SerializableMapNode = map_node;
            MapNodes.Add(obj);                                                                                          
        }
    }

    string LoadCamps(List<string> camps)
    {
        string temp = "";
        foreach (string camp in camps)
        {
            temp += camp+' ';
        }
        return temp;
    }

    void RayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100,1<<Config.config.ActionLayer.value))
        {
            if (hit.collider.tag == "Background")
            {
                if (Input.GetMouseButton(0))
                {
                  GameObject TempNode = hit.collider.gameObject.GetComponent<MapBackground>().NodeGenerator(MapNodePrefab);
                  if (TempNode != null)
                  {
                        MapNodes.Add(TempNode);
                  }
                }
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        RayHit();
        CheckMapNodes();
    }
}
