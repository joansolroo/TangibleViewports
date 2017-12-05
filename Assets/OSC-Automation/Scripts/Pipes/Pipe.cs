using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace Pipes
{

    [System.Serializable]
    public enum Type
    {
        IN, INOUT, OUT
    }
    [System.Serializable]
    public enum Direction
    {
        UPWARDS, DOWNWARDS
    }

    public interface IdentifiedObject
    {
        int id
        {
            get;
        }
    }
    public interface Connector : IdentifiedObject
    {
        Type type
        {
            get;
        }

        Direction direction
        {
            get;
        }

        void PlugInput(Connector _in);
        void UnplugInput(Connector _in);

        void PlugOutput(Connector _out);
        void UnplugOutput(Connector _out);

        void InputChanged(Connector _in);

        object GetValue();
    }
    public abstract class Pipe : MonoBehaviour, Connector
    {

        protected static int MAX_ID;
        private int _id = ++MAX_ID;
        public int id
        {
            get
            {
                return _id;
            }
        }
        public abstract Type type
        {
            get;
        }

       // [SerializeField]
        protected Direction _direction;
        public Direction direction
        {
            get
            {
                return _direction;
            }
        }
        public enum UpdateMode
        {
            FIXED, ONINPUT
        }

        [SerializeField]
        [Tooltip("When the parameter is updated. FIXED update happens once a frame, while ONINPUT will update everytime the input changes")]
        protected UpdateMode updateMode = UpdateMode.FIXED;

        protected Dictionary<int, Connector> input = new Dictionary<int, Connector>();
        protected Dictionary<int, Connector> output = new Dictionary<int, Connector>();

        protected bool inputChanged = false;
        protected bool outputChanged = false;

        public void PlugInput(Connector _in)
        {
            input[_in.id] = _in;
        }
        public void UnplugInput(Connector _in)
        {
            input.Remove(_in.id);
        }
        public void PlugOutput(Connector _out)
        {
            output[_out.id] = _out;
        }
        public void UnplugOutput(Connector _out)
        {
            output.Remove(_out.id);
        }

        public void InputChanged(Connector _in)
        {
            inputChanged = true;
            if (updateMode == UpdateMode.ONINPUT)
            {
                EvaluateAndUpdate();
                NotifyOutput();
            }
        }
        protected void Update()
        {
            if (updateMode == UpdateMode.FIXED)
            {
                EvaluateAndUpdate();
            }
        }

        protected void UnplugAll()
        {
            foreach (KeyValuePair<int, Connector> pair in input)
            {
                pair.Value.UnplugOutput(this);
            }
            input.Clear();

            foreach (KeyValuePair<int, Connector> pair in output)
            {
                pair.Value.UnplugInput(this);
            }
            output.Clear();
        }

        protected object GetFirstInput()
        {
            // Debug.Log(input.Count);
            if (input.Count > 0)
            {
                foreach (KeyValuePair<int, Connector> pair in input)
                {
                    return pair.Value.GetValue();
                }
            }
            else
            {
                Debug.LogWarning(name + " has no input");
            }
            return null;

        }
        protected object GetFirstOutput()
        {
            // Debug.Log(input.Count);
            if (input.Count > 0)
            {
                foreach (KeyValuePair<int, Connector> pair in output)
                {
                    return pair.Value.GetValue();
                }
            }
            return null;

        }
        // Return true if the value changed
        protected abstract bool Evaluate();
        void EvaluateAndUpdate()
        {
            if (Evaluate())
            {
                NotifyOutput();
                outputChanged = true;
            }
        }

        void NotifyOutput()
        {
            foreach (KeyValuePair<int, Connector> pair in output)
            {
                pair.Value.InputChanged(this);
            }
        }

        public abstract object GetValue();
    }

}