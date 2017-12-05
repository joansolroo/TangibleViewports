using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

namespace OSC
{
    public class Receiver : Communicator
    {

        // This method sets the communication (in this case, just receiving messages)
        // and setting the default values per channel
        override protected void InitCommunicator()
        {
            handler.SetAllMessageHandler(AllMessageHandler);
        }

        public void AllMessageHandler(OscMessage oscMessage)
        {

            var msgAddress = oscMessage.Address; //the message parameters
            object msgValue = null;
            if (oscMessage.Values != null && oscMessage.Values.Count > 0)
            {
                msgValue = oscMessage.Values[0]; //the message value
                if (oscMessage.Values.Count == 1)
                {
                    values[msgAddress] = msgValue == null ? 0 : msgValue;
                    //Debug.Log("-> obj[" + msgAddress + "]=" + msgValue);
                }
                else
                {
                    //Array message
                    object[] array = new object[oscMessage.Values.Count];
                    for (int m = 0; m < array.Length; ++m) {
                        array[m] = oscMessage.Values[m];
                    }
                    values[msgAddress] = array;
                    //Debug.Log("-> array[" + msgAddress + "]=" + array.Length);
                }
            }
            else
            {
                //Empty message
            }
            //var msgString = Osc.OscMessageToString(oscMessage); //the message and value combined
            //Debug.Log(msgString); //log the message and values coming from OSC
        }

        public void AddChannel(string _address, object defaultValue)
        {
            values[_address]= defaultValue;
        }
        /*
        void OnValidate()
        {
            gameObject.name = "Receiver p:" + gameObject.GetComponent<Connection>().ListenerPort;

        }*/
    }

}