using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyTransform : MonoBehaviour {

    Vector3 pos;
    Vector3 rot;
    Vector3 scale;

    [SerializeField]
    [Range(0, 1)]
    float range;
	// Use this for initialization
	void Start () {
        pos = this.transform.localPosition;
        rot = this.transform.localRotation.eulerAngles;
        scale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = pos + Vector3.one*(Random.Range(-1,1)*range);
        this.transform.localRotation = Quaternion.Euler(rot + Vector3.one * (Random.Range(-1, 1)*360*range));
        this.transform.localScale = scale + Vector3.one * (Random.Range(-1, 1)*range);
    }
}
