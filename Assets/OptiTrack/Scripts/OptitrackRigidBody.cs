//======================================================================================================
// Copyright 2016, NaturalPoint Inc.
//======================================================================================================

using System;
using UnityEngine;


public class OptitrackRigidBody : MonoBehaviour
{
    public OptitrackStreamingClient StreamingClient;
    public Int32 RigidBodyId;

    [SerializeField]
    bool localToParent = false;

    void Start()
    {
        // If the user didn't explicitly associate a client, find a suitable default.
        if ( this.StreamingClient == null )
        {
            this.StreamingClient = OptitrackStreamingClient.FindDefaultClient();

            // If we still couldn't find one, disable this component.
            if ( this.StreamingClient == null )
            {
                Debug.LogError( GetType().FullName + ": Streaming client not set, and no " + typeof( OptitrackStreamingClient ).FullName + " components found in scene; disabling this component.", this );
                this.enabled = false;
                return;
            }
        }
    }

    Vector3[] markers;
    float[] markerSize;
    void Update()
    {
        OptitrackRigidBodyState rbState = StreamingClient.GetLatestRigidBodyState( RigidBodyId );

        if ( rbState != null )
        {
            this.transform.localPosition = rbState.Pose.Position;
            this.transform.localRotation = rbState.Pose.Orientation;

            if (localToParent)
            {
                this.transform.localPosition = transform.parent.InverseTransformPoint(this.transform.localPosition);
                this.transform.localRotation = Quaternion.Inverse(transform.parent.localRotation)* this.transform.localRotation;
            }
            if(markers==null || markers.Length != rbState.Markers.Count)
            {
                markers = new Vector3[rbState.Markers.Count];
                markerSize = new float[rbState.Markers.Count];
            }
            try
            {
                for (int m = 0; m < markers.Length; ++m)
                {
                    markers[m] = rbState.Markers[m].Position;
                    markerSize[m] = rbState.Markers[m].Size;
                }
            }
            catch (Exception e) { }
        }
    }

    private void OnDrawGizmos()
    {
        if (markers != null)
        {
            for(int m = 0; m< markers.Length;++m)
            {
                Gizmos.DrawWireSphere(transform.InverseTransformPoint(markers[m]), markerSize[m]);
            }
        }
    }
}
