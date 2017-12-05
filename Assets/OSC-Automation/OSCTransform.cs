using OSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCTransform : MonoBehaviour
{

    public enum Mode
    {
        LOCAL, REMOTE, HYBRID
    }
    public Mode mode = Mode.LOCAL;

    public string localPath;
    public Transform root;
    public GameObject OSCStream;
    Sender sender;
    Receiver receiver;

    [SerializeField]
    bool recursive = false;
    // Use this for initialization
    
    void Start()
    {
        sender = OSCStream.GetComponent<Sender>();
        receiver = OSCStream.GetComponent<Receiver>();

        localPath = GetGameObjectPath(transform, root);

        receiver.AddChannel(localPath + ".transform", new float[10]);
        receiver.AddChannel(localPath + ".remoteManipulation", false);
        if (recursive)
        {
            for(int c = 0; c< transform.childCount; ++c)
            {
                Transform child = transform.GetChild(c);
                if (child.gameObject.GetComponent<OSCTransform>() == null)
                {
                    OSCTransform childOSCT = child.gameObject.AddComponent<OSCTransform>();
                    childOSCT.recursive = true;
                    childOSCT.root = root;
                    childOSCT.OSCStream = OSCStream;
                    childOSCT.mode = mode;
                }
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mode == Mode.REMOTE)
        {
            Read();
        }
        else if (mode == Mode.LOCAL)
        {
            if (transform.hasChanged)
            {
                Write();
                sender.Send(localPath + ".remoteManipulation", true);
            }
            else
            {
                sender.Send(localPath + ".remoteManipulation", false);
            }
        }
        else if (mode == Mode.HYBRID)
        {

            if (transform.hasChanged)
            {
                Write();
                if (GetRemoteChange())
                {
                    Debug.LogWarning("BOTH LOCAL AND REMOTE MODIFICATION");
                }
                sender.Send(localPath + ".remoteManipulation", true);
            }
            else //if (GetRemoteChange())
            {
                Read();
            }
        }
    }

    void Read()
    {
        float[] t = GetTransformArray();
        deSerializeTransform(t, this.transform);
    }
    
    void Write()
    {
        float[] _transform = serializeTransform(transform);
        sender.Send(localPath + ".transform", _transform);
    }
    
    float[] GetTransformArray()
    {
        object msgValue = receiver.GetValue(localPath + ".transform");

        if (msgValue != null)
        {
            if (msgValue.GetType() == typeof(object[]))
            {
                object[] array = (object[])msgValue;
                float[] output = new float[array.Length];
                for (int i = 0; i < array.Length; ++i)
                {
                    output[i] = (float)array[i];
                    //Debug.Log("-> array[" + (localPath + ".transform") + "]=" + msgValue);
                }
                return output;
            }
            else if (msgValue.GetType() == typeof(float[]))
            {
                return (float[])msgValue;
                //Debug.Log("-> array[" + (localPath + ".transform") + "]=" + msgValue);
            }
        }
        return new float[0];
    }
    bool GetRemoteChange()
    {
        object msgValue = receiver.GetValue(localPath + ".remoteManipulation");
        if (msgValue != null)
        {

            if (msgValue.GetType() == typeof(bool))
            {
                return (bool)msgValue;
            }
        }
        return false;
    }
    private static string GetGameObjectPath(Transform transform, Transform root)
    {
        string path = transform.name;
        while (transform.parent != null && (root == null || transform.parent != root))
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return "/" + path;
    }
    static bool deSerializeTransform(float[] serialization, Transform target)
    {
        if (serialization != null)
        {
            Vector3 pos = new Vector3(serialization[0], serialization[1], serialization[2]);
            target.localPosition = pos;
            Quaternion rot = new Quaternion(serialization[3], serialization[4], serialization[5], serialization[6]);
            target.localRotation = rot;
            Vector3 scale = new Vector3(serialization[7], serialization[8], serialization[9]);
            target.localScale = scale;

            return true;
        }
        return false;
    }
    static float[] serializeTransform(Transform source)
    {
        float[] serialized = new float[10];
        serialized[0] = source.localPosition.x;
        serialized[1] = source.localPosition.y;
        serialized[2] = source.localPosition.z;
        serialized[3] = source.localRotation.x;
        serialized[4] = source.localRotation.y;
        serialized[5] = source.localRotation.z;
        serialized[6] = source.localRotation.w;
        serialized[7] = source.localScale.x;
        serialized[8] = source.localScale.y;
        serialized[9] = source.localScale.z;
        return serialized;
    }
}
