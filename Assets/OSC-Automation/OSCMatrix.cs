using OSC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCMatrix : MonoBehaviour
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
    byte[] matrix;
    Texture2D tex;
    Color32[] colors;
    // Use this for initialization
    [SerializeField]
    int width = 320;
    [SerializeField]
    int height = 320;
    int size;
    void Start()
    {
        sender = OSCStream.GetComponent<Sender>();
        receiver = OSCStream.GetComponent<Receiver>();

        localPath = GetGameObjectPath(transform, root);
        size = width * height;
        matrix = new byte[size];
        receiver.AddChannel(localPath + ".matrix", matrix);
        receiver.AddChannel(localPath + ".remoteManipulation", false);

        tex = new Texture2D(width, height);
        colors = new Color32[size];
        GetComponent<Renderer>().material.mainTexture = tex;
    }
    float lastTime = 0;
    bool update = false;
    private void Update()
    {
        if (Time.time > lastTime + 1)
        {
            lastTime = Time.time;
            if (mode == Mode.LOCAL)
            {
                for (int idx = 0; idx < matrix.Length; idx++)
                {
                    matrix[idx] = (byte)Random.Range(0,256);
                }
            }
            update = true;
        }
        else
        {
            update = false;
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
            if (update)
            {
                Write();
                sender.Send(localPath + ".remoteManipulation", true);
            }
            else
            {
                sender.Send(localPath + ".remoteManipulation", false);
            }
        }

        if (update)
        {
            int idx = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    byte value = matrix[idx];
                    colors[idx++] = new Color32(value, value, value,1);
                }
            }
            tex.SetPixels32(colors);
            tex.Apply(false);
        }
    }

    void Read()
    {
        matrix = GetMatrixArray();

    }

    void Write()
    {
        sender.Send(localPath + ".matrix", matrix);
    }

    byte[] GetMatrixArray()
    {
        object msgValue = receiver.GetValue(localPath + ".matrix");

        if (msgValue != null)
        {
            if (msgValue.GetType() == typeof(object[]))
            {
                object[] array = (object[])msgValue;
                byte[] output = new byte[array.Length];
                for (int i = 0; i < array.Length; ++i)
                {
                    output[i] = (byte)array[i];
                 //   Debug.Log("-> array[" + (localPath + ".transform") + "]=" + msgValue);
                }
                return output;
            }
            else if (msgValue.GetType() == typeof(byte[]))
            {
               // Debug.Log("-> array[" + (localPath + ".transform") + "]=" + msgValue);
                return (byte[])msgValue;
                
            }
        }
       // Debug.Log("-> array[" + (localPath + ".transform") + "]=???");
        return new byte[0];
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
}
