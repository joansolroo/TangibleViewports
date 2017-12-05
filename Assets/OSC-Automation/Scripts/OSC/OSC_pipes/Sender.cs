using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OSC
{

    // Simple OSC test communication script
    public class Sender : Communicator
    {
        public void Send(string address, object message)
        {
            if(!values.ContainsKey(address) || values[address] != message)
            {
                values[address] = message;

                OscMessage oscM = new OscMessage();
                oscM.Address = address;
                oscM.Values = new ArrayList();
                oscM.Values.Add(message);
                handler.Send(oscM);
            }
        }
        public void Send<T>(string address, T[] message)
        {
            if (!values.ContainsKey(address) || values[address] != message)
            {
                values[address] = message;

                OscMessage oscM = new OscMessage();
                oscM.Address = address;
                oscM.Values = new ArrayList();
                foreach (object o in message)
                {
                    oscM.Values.Add(o);
                }
                handler.Send(oscM);
            }
        }
        // Start is called just before any of the Update methods is called the first time.
        protected override void InitCommunicator()
        {
            //handler.SetAddressHandler("/hand1", Example);
        }
        /*
        public static void Example(OscMessage m)
        {
            Debug.Log("--------------> OSC example message received: (" + m + ")");
        }
        */
        /*
        void OnValidate()
        {
            gameObject.name = "Sender   p:" + gameObject.GetComponent<Connection>().SendToPort;

        }*/
    }

}