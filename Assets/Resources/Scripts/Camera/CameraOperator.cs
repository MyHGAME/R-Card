using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//整体都需要改
public class CameraOperator : MonoBehaviour {
    public bool EditMode = false;//是否编辑模式
    public bool PlayerModeInital = true;//玩家模式的镜头初始化
    public float MouseSensitivity = 4f;//灵敏度
    public float MouseWheelSensitivity = 10f;
    public float KeySensitivity = 2.0f;
    public GameObject PlayerArea;//对准玩家区域
    public float InitialHeight = 20f,MaxHeight = 40f,MinHeight = 0f;//初始值
    public Vector3 InitialRotation = new Vector3(45, 0, 0);
    private Vector3 PlayerAreaPos;
    
    void Start()
    {
        PlayerModeInitialization();
    }

    void PlayerModeInitialization()
    {
        //临时
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("MapNode");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].GetComponent<MapNode>().Area == Area.PlayerCardArea)
            {
                if (BattleManager.battleManager.Belong(gameObjects[i]))
                {
                    PlayerArea = gameObjects[i];
                    break;
                }
            }
        }

        if (!EditMode && PlayerArea != null)
        {
            PlayerAreaPos = PlayerArea.transform.position;
            this.transform.position = new Vector3(PlayerAreaPos.x, InitialHeight, PlayerAreaPos.z);

            this.transform.LookAt((MapManager.mapManager.GetCenter));
            InitialRotation.y = this.transform.localEulerAngles.y;
            InitialRotation.z = this.transform.localEulerAngles.z;
            this.transform.localEulerAngles = InitialRotation;
        }
    }

    void Update()
    {
        if (EditMode)
        {
            Edit();
            PlayerModeInital = EditMode;
        }
        else
        {
            if (PlayerModeInital)
            {
                PlayerModeInitialization();
                PlayerModeInital = false;
            }
            Play();
        }
        
    }

    void AdjustDistance()
    {
        this.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity);
    }

    void AdjustPosition()
    {
        transform.Translate(-Input.GetAxis("Mouse X") * MouseSensitivity, -Input.GetAxis("Mouse Y") * MouseSensitivity, Input.GetAxis("Mouse Y") * MouseSensitivity);
    }

    void AdjustView(float XAxis = 0f,float YAxis = 0f)
    {
        //局部坐标和全局坐标
        transform.Rotate(XAxis , 0, 0, Space.Self);
        transform.Rotate(0, YAxis , 0, Space.World);
        //transform.localEulerAngles = new Vector3(-Input.GetAxis("Mouse Y") * MouseSensitivity+ transform.localEulerAngles.x, transform.localEulerAngles.y + Input.GetAxis("Mouse X") * MouseSensitivity, 0);
    }

    void Edit()
    {
        //滚轮实现镜头缩进和拉远  
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            AdjustDistance();
        }
        //按着鼠标右键实现视角转动  
        if (Input.GetMouseButton(1))
        {
            AdjustView(-Input.GetAxis("Mouse Y") * MouseSensitivity, Input.GetAxis("Mouse X") * MouseSensitivity);

        }
        //按住滚轮调整摄像头位置
        if (Input.GetMouseButton(2))
        {
            AdjustPosition();
        }
    }

    //限制距离移动，视角调整
    void Play()
    {
        //滚轮实现镜头缩进和拉远  
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            AdjustDistance();
        }
        //键盘按钮←/a和→/d实现视角水平移动，键盘按钮↑/w和↓/s实现纵向移动 
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * KeySensitivity, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            //需要修改，固定y轴，不改变
            float y = transform.position.y;
            transform.Translate(Vector3.forward*Input.GetAxis("Vertical") * KeySensitivity);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 1, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -1, 0, Space.World);
        }
        
    }
}
