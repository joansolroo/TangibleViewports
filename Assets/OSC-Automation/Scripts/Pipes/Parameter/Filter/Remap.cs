using UnityEngine;
using System.Collections;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif
using Pipes;
public class Remap : ParameterFilter {


    [Header("Input Range")]
    [SerializeField]
    float inMinimum = 0;
    [SerializeField]
    float inMaximum = 1;

    [Header("Output Range")]
    [SerializeField]
    float outMinimum = -1;
    [SerializeField]
    float outMaximum = 1;

    public override string pipeName
    {
        get
        {
            return "Remap[" + inMinimum +","+inMaximum+"]->["+ outMinimum+","+outMaximum+"]";
        }
    }

    protected override object Evaluate(object inValue)
    {
        if (inValue == null)
        {
            return 0;
        }
        value = (float)inValue;

        return Mathf.Lerp(outMinimum, outMaximum, Mathf.InverseLerp(inMinimum, inMaximum, (float)inValue));
    }

    public override object GetValue()
    {
        return value;
    }
#if UNITY_EDITOR    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarch context menus. 
    [MenuItem("GameObject/Parameters/Filters/Remap", false, 10)]
    static void CreateContextMenu(MenuCommand menuCommand)
    {
        if (CheckMenu())
        {
            Remap p = AutoPipe.CreateGameObjectWithComponent<Remap>(menuCommand);
        }
    }
    [MenuItem("GameObject/Parameters/Filters/Remap", true)]
    static bool CheckMenu()
    {
        return (Selection.activeTransform.gameObject != null && Selection.activeTransform.gameObject.GetComponent<Connector>() != null);
    }
#endif
    
}
