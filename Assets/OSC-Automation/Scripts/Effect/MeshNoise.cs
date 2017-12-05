using UnityEngine;
using System.Collections;
using System;

namespace OSC
{ 
    public class MeshNoise : MonoBehaviour
    {

        Mesh mesh;

        Vector3[] _vertices;

        [SerializeField]
        float scale = 0.1f;
        protected void Start()
        {
            mesh = transform.parent.gameObject.GetComponent<MeshFilter>().mesh;
            _vertices = mesh.vertices;
        }

        void Update()
        {
            {
                Vector3[] vertices = mesh.vertices;
                Vector3[] normals = mesh.normals;
                int i = 0;
                while (i < vertices.Length)
                {
                    vertices[i] = _vertices[i] + normals[i] * scale * UnityEngine.Random.Range(-0.5f, 0.5f);
                    i++;
                }
                mesh.vertices = vertices;
            }
        }
        /*override protected string GetName()
        {
            return "⇖ Effect: Mesh noise";
        }*/
    }

    
}