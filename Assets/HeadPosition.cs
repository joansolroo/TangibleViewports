using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPosition : MonoBehaviour {

    public OSC.ChannelIn sourceX;
    public OSC.ChannelIn sourceY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(-sourceX.value, sourceY.value);
	}
}
