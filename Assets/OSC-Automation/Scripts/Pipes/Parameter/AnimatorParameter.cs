using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

using Pipes;

public class AnimatorParameter : AutoPipe
{
    [System.Serializable]
    public enum ParameterType
    {
        Int, Float, Bool, Trigger
    }
    [Header("Parameter Info")]
    [SerializeField]
    [Tooltip("This is the parameter type, must match with the one on the animator")]
    ParameterType parameterType = ParameterType.Int;
    [SerializeField]
    [Tooltip("This is the parameter name, must match with the one on the animator")]
    string parameterName = "NAME";

    [Header("Parameter Value")]
    [SerializeField]
    float value = 0;
    float _previousValue = 0;
    [SerializeField]
    bool changed = false;
    Animator animator;

    public override string pipeName
    {
        get
        {
            return "Anim.parameter [" +parameterName +"="+value+"]";
        }
    }

    protected override bool Evaluate()
    {
        object obj = GetFirstInput();
        if (obj == null)
        {
            return false;
        }
        try {
            value = (float)obj;
        }
        catch(System.Exception e)
        {
            return false;
        }
        if (value == _previousValue)
        {
            changed = false;
        }
        else {
            changed = true;

            switch (parameterType)
            {
                case (ParameterType.Int):
                    animator.SetInteger(parameterName, (int)value);
                    break;
                case (ParameterType.Float):
                    animator.SetFloat(parameterName, value);
                    break;
                case (ParameterType.Bool):
                    animator.SetBool(parameterName, value > 0);
                    break;
                case (ParameterType.Trigger):
                    if (value > 0)
                    {
                        animator.SetTrigger(parameterName);
                    }
                    break;
            }
            _previousValue = value;

            UpdateName();
        }
        return changed;
    }

    protected override void InitializePipe()
    {
        _type = Type.IN;
        _direction = Direction.UPWARDS;
    }
    protected override void InitializeConnections()
    {
        animator = transform.parent.gameObject.GetComponent<Animator>();
    }

    public override object GetValue()
    {
        return value;
    }
#if UNITY_EDITOR
    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarch context menus. 
    [MenuItem("GameObject/Parameters/Animator/Parameter", false, 10)]
    static void CreateContextMenu(MenuCommand menuCommand)
    {
        if (CheckMenu())
        {
            AnimatorParameter p = AutoPipe.CreateGameObjectWithComponent<AnimatorParameter>(menuCommand);
            p.animator = (menuCommand.context as GameObject).GetComponent<Animator>();
        }
    }
    [MenuItem("GameObject/Parameters/Animator/Parameter", true)]
    static bool CheckMenu()
    {
        return (Selection.activeTransform.gameObject!=null && Selection.activeTransform.gameObject.GetComponent<Animator>() != null);
    }

#endif
}
