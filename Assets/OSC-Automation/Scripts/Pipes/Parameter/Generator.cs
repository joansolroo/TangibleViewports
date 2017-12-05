using UnityEngine;
using System.Collections;

using Pipes;

public abstract class Generator : AutoPipe {

    public abstract override string pipeName
    {
        get;
    }

    protected abstract override bool Evaluate();

    protected override void InitializePipe()
    {
        _type = Type.OUT;

    }
    protected override void InitializeConnections()
    {
        if (parent != null)
        {
            _direction = Direction.UPWARDS;
        }
        else
        {
            _direction = Direction.DOWNWARDS;
        }
    }

}
