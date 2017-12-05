using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoTransform : MonoBehaviour {

    [SerializeField]
    Transform transformToUndo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (transformToUndo)
        {
            this.transform.localPosition = transformToUndo.InverseTransformPoint(Vector3.zero);
            this.transform.localRotation = Quaternion.Inverse(transformToUndo.localRotation);
        }
	}
}
