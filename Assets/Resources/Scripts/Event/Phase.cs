using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour {
    public static Phase phase;
    public int RoundNumber = 0;
    public int PhaseNumber = -1;
    public List<string> PhaseLoop = new List<string>();
    public string CurrentPhase;
    public string[] PlayerOrder;
    public string GetPlayer
    {
        get
        {
            return PlayerOrder[PhaseNumber / Config.config.Phase.Length];
        }
    }

    public string GetPhase
    {
        get
        {
            return CurrentPhase.Replace(GetPlayer, "");
        }
    }

    void Awake()
    {
        phase = this;
    }
    public void CreatePhaseLoop(string[] playerOrder)
    {
        PhaseLoop.Clear();
        PlayerOrder = playerOrder;
        for (int i = 0; i < playerOrder.Length; i++)
        {
            for(int j = 0;j < Config.config.Phase.Length;j++)
                PhaseLoop.Add(playerOrder[i] + Config.config.Phase[j]);
        }
        RoundNumber = 0;
        PhaseNumber = -1;
        CurrentPhase = "";
    }

    public void NextPhase()
    {
        if (PhaseLoop.Count == 0) return;
        PhaseNumber++;
        PhaseNumber %= PhaseLoop.Count;
        if (PhaseNumber == 0) RoundNumber++;
        CurrentPhase = PhaseLoop[PhaseNumber];
        print(CurrentPhase);
    }

    IEnumerator AutoPhase()
    {
        while (true)
        {
            foreach (string phase in Config.config.AutoPhase)
            {
               
                if (Event.Instance.PhaseEventCount == 0&&phase == GetPhase)
                {
                    yield return new WaitForSeconds(1);
                    NextPhase();
                }
               
            }
            yield return 0; 
        }
       
    }

    
    // Use this for initialization
    void Start () {
		CreatePhaseLoop(new string[]{"a","b"});
        StartCoroutine(AutoPhase());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextPhase();
           
        }
	}
}
