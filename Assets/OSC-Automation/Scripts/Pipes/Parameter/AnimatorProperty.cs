using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

using Pipes;

public class AnimatorProperty : AutoPipe
{
    [System.Serializable]
    public enum PropertyName
    {
        Speed
    }
    [Header("Property Info")]
    [SerializeField]
    [Tooltip("This is the parameter to set")]
    PropertyName property;

    [Header("Property Value")]
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
            return "Anim.property [" +property +"="+value+"]";
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

            switch (property)
            {
                case (PropertyName.Speed):
                    animator.speed =  (float)value;
                    break;
                    /* here others can be added*/
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
    [MenuItem("GameObject/Parameters/Animator/Property", false, 10)]
    static void CreateContextMenu(MenuCommand menuCommand)
    {
        if (CheckMenu())
        {
            AnimatorProperty p = AutoPipe.CreateGameObjectWithComponent<AnimatorProperty>(menuCommand);
            p.animator = (menuCommand.context as GameObject).GetComponent<Animator>();
        }
    }
    [MenuItem("GameObject/Parameters/Animator/Property", true)]
    static bool CheckMenu()
    {
        return (Selection.activeTransform.gameObject!=null && Selection.activeTransform.gameObject.GetComponent<Animator>() != null);
    }

#endif
    
}
