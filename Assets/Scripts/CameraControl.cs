using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{
    
    private Vector3 lastMousePos = Vector3.zero;
    private bool isMouseHeld;

    public float lookSpeed = 0.7f;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            RotateCameraDirection(true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RotateCameraDirection(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //transform.localEulerAngles.x = 67.5;
        }
        
        AdjustCameraLook(Input.mousePosition);
    }
    
    public void RotateCameraDirection(bool toRight)
    {
        float angle = toRight ? 90.0f : -90.0f;
        Vector3 parentCenter = transform.parent.TransformPoint(new Vector3(-1,0,1));
        transform.RotateAround(parentCenter, Vector3.up, angle);
    }

    public void AdjustCameraLook(Vector3 position)
    {
        Vector3 mouseDelta = (position - lastMousePos);

        if (isMouseHeld)
        {
            transform.Rotate(new Vector3(mouseDelta.y * lookSpeed, 0, 0), Space.Self);
        }

        lastMousePos = position;
    }
    
}
