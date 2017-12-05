using UnityEngine;
using System.Collections.Generic;
using Pipes;
using System;

public class PipeNexus: MonoBehaviour {

    Dictionary<string, PipeHub> nexus = new Dictionary<string, PipeHub>();

    private static PipeNexus _instance;
    public static PipeNexus instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject();
                PipeNexus pn = go.AddComponent<PipeNexus>();
                pn.Awake();
                _instance = pn;
            }
            if (applicationIsQuitting)
            {
                return null;
            }
            return _instance;
        }
    }
    GameObject hubs;
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (_instance == null)
        {

            //if not, set instance to this
            _instance = this;

            Initialize();

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);
        }
        //If instance already exists and it's not this:
        else if (_instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
       
    }
    protected void Initialize()
    {

        hubs = new GameObject("LOCAL HUBS");
        hubs.transform.parent = this.transform;
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    public void AddOutput(string channel, Pipe pipe)
    {
        PipeHub ph;
        if (!nexus.ContainsKey(channel))
        {
            GameObject go =  new GameObject();
            ph = go.AddComponent<PipeHub>();
            ph.channel = channel;
            go.transform.parent = hubs.transform;
            nexus[channel] = ph;
        }
        else
        {
            ph = nexus[channel];
        }

        ph.PlugOutput(pipe);
        pipe.PlugInput(ph);
    }
    public void RemoveOutput(string channel, Pipe pipe)
    {
        if (nexus.ContainsKey(channel))
        {
            nexus[channel].UnplugOutput(pipe);
        }
    }
    public void AddInput(string channel, Pipe pipe)
    {
        PipeHub ph;
        if (!nexus.ContainsKey(channel))
        {
            GameObject go = new GameObject();
            ph = go.AddComponent<PipeHub>();
            ph.channel = channel;
            go.transform.parent = hubs.transform;
            nexus[channel] = ph;
        }
        else
        {
            ph = nexus[channel];
        }

        ph.PlugInput(pipe);
        pipe.PlugOutput(ph);
    }
    public void RemoveInput(string channel, Pipe pipe)
    {
        if (nexus.ContainsKey(channel))
        {
            nexus[channel].UnplugInput(pipe);
        }
    }
}

public class PipeHub : AutoPipe
{
    public string channel;
    [SerializeField]
    object value;

    public override string pipeName
    {
        get
        {
            return "Hub: " + channel;
        }
    }

    public override object GetValue()
    {
        return value;
    }

    object prev = 0;
    protected override bool Evaluate()
    {
       
        value = GetFirstInput();
        bool change = prev != value;
        if (change)
        {
            prev = value;
        }
        return change;
    }

    protected override void InitializePipe()
    {
        _type = Pipes.Type.INOUT;
        
    }
    protected override void InitializeConnections()
    {
        if (parent != null)
        {
            _direction = Direction.DOWNWARDS;
        }
        else
        {
            _direction = Direction.UPWARDS;
        }
    }

}