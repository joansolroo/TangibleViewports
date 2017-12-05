using UnityEngine;
using System.Collections;


namespace OSC {
    public class Connection : MonoBehaviour
    {
        [Header("Connection setup")]
        [SerializeField]
        public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
        [SerializeField]
        public int SendToPort = 9000; //the port you will be sending from
        [SerializeField]
        public int ListenerPort = 8000; //the port you will be listening on
                                           // Use this for initialization

        public static Connection instance;
        protected Osc handler;

        void Start()
        {
            UDPPacketIO udp = GetComponent<UDPPacketIO>();
            udp.init(RemoteIP, SendToPort, ListenerPort);
            handler = GetComponent<Osc>();
            handler.init(udp);
        }
        protected void OnDisable()
        {
            if (handler != null)
            {
                handler.Cancel();
            }

            // speed up finalization
            handler = null;
            System.GC.Collect();
        }


        void OnValidate()
        {
            gameObject.name = "OSC Connection";
            instance = this;
        }
    }


}
