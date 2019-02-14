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
    private bool isAHeld;
    private bool isDHeld;

    public float lookSpeed = 0.7f;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isMouseHeld = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isMouseHeld = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isAHeld = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            isAHeld = false;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDHeld = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            isDHeld = false;
        }

        AdjustCameraLook(Input.mousePosition);
        RotateCameraDirection();
    }
    
    public void RotateCameraDirection()
    {
        if (isAHeld || isDHeld)
        {
            float angle = isAHeld ? 90.0f : -90.0f;
            Vector3 parentCenter = transform.parent.TransformPoint(new Vector3(-1,0,1));
            transform.RotateAround(parentCenter, Vector3.up, angle * Time.deltaTime);
        }
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
