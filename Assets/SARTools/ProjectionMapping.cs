using UnityEngine;
using System.Collections;

using System;
using System.IO;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ProjectionMapping : MonoBehaviour
{

    Camera theCamera;


    public string path = "data\\projectorCalibration.mat";
    public Matrix4x4 projection;
    public Matrix4x4 view;

    public Matrix4x4 initialView;
    public Matrix4x4 initialProjection;

    public bool calibrateOnStartup = true;
    public bool updateCameraCalibration = true;
    public bool UpdateView = true;
    public bool UpdateProjection = true;

    void Start()
    {
        updateCameraCalibration = calibrateOnStartup;
        theCamera = GetComponent<Camera>();
        view = theCamera.worldToCameraMatrix;
        projection = theCamera.projectionMatrix;
        initialView = theCamera.worldToCameraMatrix;
        initialProjection = theCamera.projectionMatrix;
    }
    Matrix4x4 DXtoOGL(Matrix4x4 DX)
    {
        Matrix4x4 result = DX.transpose;

        return result;
    }

    private void UpdateCameraCalibration()
    {

        {
            float[,] result = new float[16, 2];
            string input = File.ReadAllText(path);
            {
                int x = 0, y = 0;
                
                foreach (var row in input.Split('\r'))
                {

                    x = 0;
                    foreach (var col in row.Trim().Split(' '))
                    {
                        string value = col.Trim();
                        
                        result[x, y] = Single.Parse(value);
                        //Console.WriteLine("{0} -> {1}", value, result[x, y]);
                        x++;
                        

                    }
                    y++;
                }
            }
            if (UpdateView)
            {
                Matrix4x4 originalView = new Matrix4x4();
                originalView.SetRow(0, new Vector4(result[0, 0], result[1, 0], result[2, 0], result[3, 0]));
                originalView.SetRow(1, new Vector4(result[4, 0], result[5, 0], result[6, 0], result[7, 0]));
                originalView.SetRow(2, new Vector4(result[8, 0], result[9, 0], result[10, 0], result[11, 0]));
                originalView.SetRow(3, new Vector4(result[12, 0], result[13, 0], result[14, 0], result[15, 0]));
                
                cameraPosition = Matrix4x4Utils.DXToOGL.View(originalView).inverse;

                view = cameraPosition.inverse;
            }

            if (UpdateProjection)
            {


                //  float n = near - relativePosition.z;
                //  float f = far - relativePosition.z;



                projection.SetRow(0, new Vector4(result[0, 1], result[1, 1], result[2, 1], result[3, 1]));
                projection.SetRow(1, new Vector4(result[4, 1], result[5, 1], result[6, 1], result[7, 1]));
                projection.SetRow(2, new Vector4(result[8, 1], result[9, 1], result[10, 1], result[11, 1]));
                projection.SetRow(3, new Vector4(result[12, 1], result[13, 1], result[14, 1], result[15, 1]));
                projection = Matrix4x4Utils.DXToOGL.Projection(projection);

                float near = theCamera.nearClipPlane;
                float far = theCamera.farClipPlane;

                projection[2, 2] = -(far + near) / (far - near);
                projection[2, 3] = -2 * far * near / (far - near);
                projection[3, 2] = -1;
                //Debug.Log("fov:" + Mathf.Atan(1.0f / projection.m00) * 2.0f * Mathf.Rad2Deg + ","+ Mathf.Atan(1.0f / projection.m11) * 2.0f * Mathf.Rad2Deg);

            }

        }
    }

    public bool reset = false;
    public Matrix4x4 cameraPosition;
    public bool DoUpdate = true;
    void LateUpdate()
    {
        if (updateCameraCalibration)
        {
            updateCameraCalibration = false;
            UpdateCameraCalibration();
        }
        if (reset)
        {
            reset = false;
            projection = initialProjection;
            theCamera.ResetProjectionMatrix();
            theCamera.ResetWorldToCameraMatrix();

        }
        if (DoUpdate)
        {
            if (UpdateView)
                Matrix4x4Utils.SetTransformFromMatrix(transform, ref cameraPosition);
            //theCamera.worldToCameraMatrix = view;
            //if (UpdateProjection)
                theCamera.projectionMatrix = projection;
        }
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UpdateCameraCalibration();
#endif
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(new Vector3(0, 0, -10), new Vector3(20, 10, 20));
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(new Vector3(0, 0, -10), new Vector3(20, 10, 20));
        Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 3);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
