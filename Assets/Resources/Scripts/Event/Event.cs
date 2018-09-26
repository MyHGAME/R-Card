using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTemlate
{
    public int EventNumber;
    public delegate IEnumerator TheEvent();
    public TheEvent theEvent;
    public Dictionary<string, string> Record;

    public EventTemlate()
    {
        Record.Add("发动者","");
        Record.Add("行为", "");
        Record.Add("接受者", "");
    }

}

public class Event : MonoBehaviour {
    public static Event Instance;
    public delegate bool ConditionsTemlate();
    public delegate void temlate();
    public Dictionary<string, List<EventTemlate>> Events = new Dictionary<string, List<EventTemlate>>();
    public List<EventTemlate> PastEvents = new List<EventTemlate>();
    public List<EventTemlate> AtomicEvents = new List<EventTemlate>();
    public List<ConditionsTemlate> ConditionsLoop = new List<ConditionsTemlate>();
    public List<temlate> PassiveCouterLoop = new List<temlate>();
    public EventTemlate CurrentEvent;
    public int EventsNumber = 0;
    public int PhaseEventCount = 0;
    public float IntervalTime = 1.0f;
    public float CurrentTime = 0;
    //锁住事件，一次只执行一次事件，执行事件时，只有解锁了才能执行其他事件
    public bool EventLock = false;
    //行动锁，当前回合有事件时，锁住，不能切换回合，
    //玩家不能释放技能和攻击，可以选择，查看
    public bool ActionLock = false;

    public Dictionary<string, string> CurrentEventRecord
    {
        get
        {
            return CurrentEvent.Record;
        }
    }

    public Dictionary<string, string> PastEventRecord
    {
        get
        {
            return PastEvents[PastEvents.Count - 1].Record;
        }
    }

    void Awake()
    {
        Instance = this;
        
    }

    // Use this for initialization
	void Start () {
        Init();
	}

    void Init()
    {
        CurrentTime = IntervalTime;
        for (int i = 0; i < Config.config.Phase.Length; i++)
        {
           
            Events.Add(Config.config.Phase[i], new List<EventTemlate>());
        }
        EventsNumber = 0;
        IntervalTime = Config.config.MaxEventIntervalTime;
        CurrentTime = 0;
    }

    void DetectLoop()
    {
        for (int i = 0; i < ConditionsLoop.Count; i++)
        {
            ConditionsLoop[i]();
        }
        for (int i = 0; i < PassiveCouterLoop.Count; i++)
        {
            PassiveCouterLoop[i]();
        }
    }

    

    void EventLoop()
    {
        if (PhaseEventCount == 0) 
        { 
            CurrentEvent = null;
           
            return; 
        }
        if (!EventLock)
        {
            CurrentEvent = Events[Phase.phase.GetPhase][PhaseEventCount - 1];
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                CurrentTime = IntervalTime;
                //委托执行，用协程的方式可以做异步处理，当事件需要等待时，执行等待事件
                //等待事件执行一次endevent函数，目的将缓存的小事件释放到事件列表
                //缓存小事件的原因，方便插入，在一个大事件里，有许多带有记录的小事件，
                //带记录是为了服务被动效果和反击效果，小事件的插入需要一个接着一个而且必须在大事件的后面
                //所以用缓存小事件，当结束事件时，释放出小事件
                StartCoroutine(CurrentEvent.theEvent());
                Events[Phase.phase.GetPhase].Remove(CurrentEvent);
                //执行到这个就代表可以成功执行了，就可以成过去事件，过去事件是成功事件
                PastEvents.Add(CurrentEvent);
                EventLock = true;
            }
        }   
    }

    public void EndEvent()
    {
        ReleaseAtomicEvents();
        EventLock = false;
       
    }

    //被动事件add，判断好发生条件，若在某个效果发生后，就在past事件里
    public void InsertEvent(EventTemlate temp ,bool couter = false)
    {
        Events[Phase.phase.GetPhase].Add(temp);
        temp.EventNumber = ++EventsNumber;
        //插入了事件，重置时间
        CurrentTime = IntervalTime;
        //是反击效果，解锁，刷新事件
        if (couter) EventLock = false;
       
    }

    public void InsertAtomicEvent(EventTemlate temp)
    {
        AtomicEvents.Add(temp);
    }

    void ReleaseAtomicEvents()
    {
        for (int i = AtomicEvents.Count - 1; i >= 0; i--)
        {
            InsertEvent(AtomicEvents[i]);
        }
        AtomicEvents.Clear();

    }

	// Update is called once per frame
	void Update () {
        if (Phase.phase.GetPhase == "") return;
        PhaseEventCount = Events[Phase.phase.GetPhase].Count;
        ActionLock = !((PhaseEventCount == 0) || EventLock);
        DetectLoop();
        EventLoop();
    }
}
