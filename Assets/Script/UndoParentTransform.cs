using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoParentTransform : MonoBehaviour
{
    [SerializeField]
    Transform toUndo;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {

        // Vector3 localScale = this.transform.localScale;
        this.transform.localPosition = toUndo.InverseTransformPoint(Vector3.zero);
        this.transform.localRotation = Quaternion.Inverse(toUndo.localRotation);
    }
}
