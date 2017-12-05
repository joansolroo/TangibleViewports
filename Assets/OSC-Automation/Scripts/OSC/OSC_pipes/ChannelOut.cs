using UnityEngine;
using System.Collections;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace OSC
{
    public class ChannelOut: Pipes.AutoPipe
    {

        [Header("Setup")]
        protected OSC.Sender sender;
        public string address;

        [Header("Value")]
        public float value = 0;

        public override string pipeName
        {
            get
            {
                return "OSC - Send: " + address;
            }
        }

        protected override void InitializePipe()
        {
            _direction = Pipes.Direction.UPWARDS;
            _type = Pipes.Type.IN;
           
        }
        protected override void InitializeConnections()
        {
            sender = transform.parent.gameObject.GetComponent<Sender>();
        }
        float prev = float.MinValue;
        protected override bool Evaluate()
        {
            
            value = (float)GetFirstInput();
            bool change = prev != value;
            if (change)
            {
                sender.Send(address, value);
                prev = value;
            }
            return change;
        }

        public override object GetValue()
        {
            return value;
        }
#if UNITY_EDITOR
        // Add a menu item to create custom GameObjects.
        // Priority 1 ensures it is grouped with the other menu items of the same kind
        // and propagated to the hierarchy dropdown and hierarch context menus. 
        [MenuItem("GameObject/Parameters/Communication/OSC/Send", false, 10)]
        static void CreateContextMenu(MenuCommand menuCommand)
        {
            if (CheckMenu())
            {
                Pipes.AutoPipe.CreateGameObjectWithComponent<ChannelOut>(menuCommand);
            }
        }
        [MenuItem("GameObject/Parameters/Communication/OSC/Send", true)]
        static bool CheckMenu()
        {
            return (Selection.activeTransform.gameObject != null && Selection.activeTransform.gameObject.GetComponent<Connection>() != null);
        }
#endif 
    }
}