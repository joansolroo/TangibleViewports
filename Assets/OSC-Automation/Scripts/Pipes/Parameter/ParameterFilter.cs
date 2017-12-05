using UnityEngine;
using System.Collections.Generic;

using Pipes;


#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public abstract class ParameterFilter : AutoPipe
{
    [SerializeField]
    protected object value;

    protected override void InitializePipe()
    {
        _type = Pipes.Type.INOUT;

    }
    protected override void InitializeConnections()
    {
        if (parent != null)
        {
            _direction = parent.direction;
        }
    }

    public override object GetValue()
    {
        return value;
    }

    object prev = 0;
    protected override bool Evaluate()
    {
        object inValue = GetFirstInput();
        object v = Evaluate(inValue);
        value = v;
        bool change = value != prev;
        prev = v;
        return change;
    }
    protected abstract object Evaluate(object inValue);
}
