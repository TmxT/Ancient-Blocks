using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerPos;

    public Vector3 offset;

    public bool Freeze { protected get; set; }

    [Range(2, 5)]
    public float smooth;
   
    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (!Freeze)
        {
            Vector3 fixCamPos = playerPos.position + offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position, fixCamPos, smooth * Time.fixedDeltaTime);
            transform.position = smoothFollow;
        }
    }
}
