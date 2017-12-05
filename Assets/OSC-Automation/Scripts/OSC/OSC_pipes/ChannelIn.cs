using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace OSC
{
    public class ChannelIn : Pipes.AutoPipe
    {

        [Header("Setup")]
        protected OSC.Receiver receiver;
        public string address;
        public bool isTrigger = false;
        [Header("Value")]
        public float defaultValue = 0;
        public float value = 0;

        public override string pipeName
        {
            get
            {
                return "OSC - Read: " + address;
            }
        }

        void LateUpdate()
        {
            if (isTrigger)
            {
                value = defaultValue;
            }
        }

        public float GetOutput()
        {
            if (receiver != null)
            {
                object msgValue = receiver.GetValue(address);
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
            }
            return defaultValue;
        }

        protected override void InitializePipe()
        {
            _direction = Pipes.Direction.DOWNWARDS;
            _type = Pipes.Type.OUT;

        }
        protected override void InitializeConnections()
        {
            receiver = transform.parent.gameObject.GetComponent<Receiver>();
            receiver.AddChannel(address, defaultValue);
        }
        float prev = 0;
        protected override bool Evaluate()
        {
            object obj = receiver.GetValue(address);
            if (obj == null)
            {
                return false;
            }
            value = (float)obj;

            bool change = prev != value;
            if (change)
            {
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
        [MenuItem("GameObject/Parameters/Communication/OSC/Read", false, 10)]
        static void CreateContextMenu(MenuCommand menuCommand)
        {
            if (CheckMenu())
            {
                Pipes.AutoPipe.CreateGameObjectWithComponent<ChannelIn>(menuCommand);
            }
        }
        [MenuItem("GameObject/Parameters/Communication/OSC/Read", true)]
        static bool CheckMenu()
        {
            return (Selection.activeTransform.gameObject != null && Selection.activeTransform.gameObject.GetComponent<Connection>()!=null);
        }
#endif
    }

}