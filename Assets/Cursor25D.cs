using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor25D : MonoBehaviour {

    Camera theCamera;
    public GameObject cursor;
    Vector3 cursorPosition;
    RaycastHit hit;
    bool touchingSomething = false;
    public GameObject screen;
    // Use this for initialization
    void Start () {
        theCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        theCamera.ScreenPointToRay(Input.mousePosition);

        touchingSomething = Physics.Raycast(theCamera.ScreenPointToRay(Input.mousePosition), out hit);
        if (touchingSomething)
        {
            cursorPosition = new Vector3(hit.point.x, hit.point.y, Mathf.Min(0,hit.point.z));
            cursor.transform.position = cursorPosition;
        }
    }
    private void OnDrawGizmos()
    {
        if (touchingSomething)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
            Gizmos.DrawWireSphere(hit.point, 1f);
            Gizmos.DrawLine(cursorPosition, new Vector3(hit.point.x, hit.point.y, screen.transform.position.z));
        }
    }
}
