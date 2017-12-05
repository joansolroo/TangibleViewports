using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtQuad : MonoBehaviour
{

    Camera theCamera;
    public GameObject targetQuad;
    Vector3 cameraPosition;


    Matrix4x4 viewMatrix;
    Matrix4x4 projectionMatrix = new Matrix4x4();

    // Use this for initialization
    void Start()
    {
        theCamera = GetComponent<Camera>();
        cameraPosition = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        cameraPosition = transform.position;
        Matrix4x4 inverseTargetTransform = targetQuad.transform.worldToLocalMatrix;
        Vector3 relativePosition = inverseTargetTransform.MultiplyPoint(cameraPosition);

        #region COMPUTE VIEW
        Vector3 properPosition;
        properPosition = cameraPosition;

        Vector3 scale =  targetQuad.transform.lossyScale;
        viewMatrix = Matrix4x4.TRS(properPosition, Quaternion.identity, scale).inverse;
        viewMatrix.SetRow(2, -viewMatrix.GetRow(2));
        //viewMatrix[2, 2] = -viewMatrix[2, 2];
        //viewMatrix[2, 3] = -viewMatrix[2, 3];
        
        #endregion
        #region COMPUTE PROJECTION

        float relativeToDistance = relativePosition.z;
        float near = theCamera.nearClipPlane;
        float far = theCamera.farClipPlane ;
        float left = (-0.5f - relativePosition.x);
        float right = (0.5f - relativePosition.x);
        float top = (-0.5f - relativePosition.y);
        float bottom = (0.5f - relativePosition.y);

      //  float n = near - relativePosition.z;
      //  float f = far - relativePosition.z;
        
        projectionMatrix[0, 0] = -2 * relativeToDistance / (right - left);
        projectionMatrix[0, 2] = (left + right) / (right - left);
        projectionMatrix[1, 1] = 2 * relativeToDistance / (top - bottom);
        projectionMatrix[1, 2] = -(bottom + top) / (top - bottom);
        projectionMatrix[2, 2] = -(far + near) / (far - near);
        projectionMatrix[2, 3] = -2 * far * near / (far - near);
        projectionMatrix[3, 2] = -1;

        /*
        projectionMatrix[0, 0] = 2 * relativeToDistance / (right - left);
        projectionMatrix[0, 2] = -(left + right) / (right - left);
        projectionMatrix[1, 1] = projectionMatrix[0, 0];//2 * near / (top - bottom);
        projectionMatrix[1, 2] = -(bottom + top) / (top - bottom);
        projectionMatrix[2, 2] = (far + near) / (far - near);
        projectionMatrix[2, 3] = -far * near / (far - near);
        projectionMatrix[3, 2] = 1;
        projectionMatrix = projectionMatrix.transpose;
        Matrix4x4Utils.DXToOGL.Projection(projectionMatrix);
        */

        #endregion

        theCamera.projectionMatrix = projectionMatrix;
        theCamera.worldToCameraMatrix = viewMatrix;
    }
}
