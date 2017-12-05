using UnityEngine;
using System.Collections.Generic;

namespace OSC
{
    public abstract class Communicator : MonoBehaviour
    {

        protected Dictionary<string, object> values = new Dictionary<string, object>();
        protected Osc handler;

        // Use this for initialization
        void Start()
        {
            handler = gameObject.GetComponent<Osc>();
        }

        bool initialized = false;
        protected void Update()
        {
            if (!initialized)
            {
                initialized = true;
                InitCommunicator();
            }
        }

        protected abstract void InitCommunicator();

        public object GetValue(string msgAddress)
        {
            return values[msgAddress];
        }

        public float GetFloat(string msgAddress)
        {
            object msgValue = values[msgAddress];
            if (msgValue != null)
            {
                if (msgValue.GetType() == typeof(int))
                {
                    return (int)msgValue;
                }
                else if (msgValue.GetType() == typeof(float))
                {
                    return (float)msgValue;
                }
            }
            return 0;
        }
    }

}