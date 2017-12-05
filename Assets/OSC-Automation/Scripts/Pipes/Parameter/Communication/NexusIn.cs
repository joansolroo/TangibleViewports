using UnityEngine;
using System.Collections;
using System;

using Pipes;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class NexusIn : Pipes.AutoPipe
{
    [SerializeField]
    string address;
    public override string pipeName {
        get
        {
            return "Set: " + address;
        }
    }

    public override object GetValue()
    {
        return value;
    }
    [SerializeField]
    object value;
    
    protected override bool Evaluate()
    {
        value = GetFirstInput();
        return true; //TODO poll the value of FirstInput
    }
    protected override void InitializePipe()
    {
        _type = Pipes.Type.INOUT;
        
    }


    string previousAddress = null;

    protected override void InitializeConnections()
    {
        if (parent != null)
        {
            _direction = parent.direction;
        }
        else
        {
            _direction = Direction.UPWARDS;
        }
        if (previousAddress != null)
        {
            PipeNexus.instance.RemoveInput(previousAddress, this);
        }
        PipeNexus.instance.AddInput(address, this);
        previousAddress = address;
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarch context menus. 
    [MenuItem("GameObject/Parameters/Communication/Local/Send", false, 10)]
    static void CreateContextMenu(MenuCommand menuCommand)
    {
        if (CheckMenu())
        {
            AutoPipe.CreateGameObjectWithComponent<NexusIn>(menuCommand);
        }
    }
    [MenuItem("GameObject/Parameters/Communication/Local/Send", true)]
    static bool CheckMenu()
    {
        return (Selection.activeTransform.gameObject != null);
    }
#endif
    
}
