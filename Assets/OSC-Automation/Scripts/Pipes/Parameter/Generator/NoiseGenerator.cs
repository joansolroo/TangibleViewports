using UnityEngine;
using System.Collections;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

using Pipes;

public class NoiseGenerator : Generator
{

    [Header("Input Range")]
    [SerializeField]
    float minimum = 0;
    [SerializeField]
    float maximum = 1;

    [Header("Output")]
    [SerializeField]
    float value = 0;
    public override string pipeName
    {
        get
        {
            return "Random [" + minimum + "," + maximum + "]";
        }
    }
    public override object GetValue()
    {
        return value;
    }
    protected override bool Evaluate()
    {
        value = UnityEngine.Random.Range(minimum, maximum);
        return true;
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarch context menus. 
    [MenuItem("GameObject/Parameters/Generators/Noise", false, 10)]
    static void CreateContextMenu(MenuCommand menuCommand)
    {
        if (CheckMenu())
        {
            AutoPipe.CreateGameObjectWithComponent<NoiseGenerator>(menuCommand);
        }
    }
    [MenuItem("GameObject/Parameters/Generators/Noise", true)]
    static bool CheckMenu()
    {
        return (Selection.activeTransform.gameObject != null);
    }
#endif
    
}
