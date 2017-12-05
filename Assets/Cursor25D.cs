using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor25D : MonoBehaviour {

    Camera theCamera;
    public GameObject cursorSar;
    public GameObject cursorScreen;
    Vector3 cursorPosition;
    Vector3 cursorPositionPrevious;
    RaycastHit hit;
    bool touchingSomething = false;
    public GameObject screen;

    [SerializeField]
    float brushSize = 0.1f;

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
            if (hit.transform.gameObject != screen.gameObject)
            {
                cursorSar.transform.position = cursorPosition;
                cursorSar.SetActive(true);
            }
            else
            {
                cursorSar.SetActive(true);
            }

            cursorScreen.transform.position = new Vector3(cursorPosition.x, cursorPosition.y, screen.transform.position.z);
        }
        if(touchingSomething && Input.GetMouseButton(0))
        {
            if (Vector3.Distance(cursorPosition, cursorPositionPrevious) > brushSize/2)
            {
                if (hit.transform.gameObject != screen.gameObject)
                {
                    GameObject paint = Instantiate<GameObject>(cursorSar);
                    paint.transform.parent = hit.transform;
                    paint.transform.position = cursorPosition;
                    cursorPositionPrevious = cursorPosition;

                    paint.transform.localScale = new Vector3(brushSize / paint.transform.lossyScale.x,
                        brushSize / paint.transform.lossyScale.y,
                        brushSize / paint.transform.lossyScale.z);
                }
                else{
                    GameObject paint = Instantiate<GameObject>(cursorScreen);
                    paint.transform.parent = hit.transform;
                    paint.transform.position = cursorPosition;
                    cursorPositionPrevious = cursorPosition;

                    paint.transform.localScale = new Vector3(brushSize / paint.transform.lossyScale.x,
                        brushSize / paint.transform.lossyScale.y,
                        brushSize / paint.transform.lossyScale.z);
                }
            }
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
