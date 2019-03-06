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
    public float introLookMultiplier = 0.5f;
    public Transform centralPoint;
    public Transform player;
    public Transform goal;
    private Vector3 lastMousePos = Vector3.zero;
    private bool isMouseHeld;
    private bool isAHeld;
    private bool isDHeld;
    private bool isWHeld;
    private bool isSHeld;
    private bool isGameReady;
    private Quaternion rotationEnd;
    private float demoAngle;

    public float lookSpeed = 0.7f;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
        if (centralPoint != null)
        {
            var position = centralPoint.position;
            transform.Translate(new Vector3(position.x, position.y + 10,position.z - 5));
            
            Vector3 lookHeading = player.position - transform.position;
            lookHeading = lookHeading / lookHeading.magnitude;
            
            transform.LookAt(goal.position);
            demoAngle = Quaternion.Angle(transform.rotation, rotationEnd);
            rotationEnd = Quaternion.LookRotation(lookHeading, Vector3.up);
        }
    }

    private void InitializeGameCamera()
    {
        if (!isGameReady && transform.rotation != rotationEnd)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationEnd, demoAngle * Time.deltaTime * lookSpeed * introLookMultiplier);
            isGameReady = false;
        }
        else
        {
            isGameReady = true;
        }
    }

    void Update()
    {
        InitializeGameCamera();

        if (isGameReady)
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
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWHeld = true;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                isWHeld = false;
            }
        
            if (Input.GetKeyDown(KeyCode.S))
            {
                isSHeld = true;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                isSHeld = false;
            }

            AdjustCameraLook(Input.mousePosition);
            RotateCameraDirection();
        }
    }
    
    public void RotateCameraDirection()
    {
        if (isAHeld || isDHeld)
        {
            float angle = isAHeld ? -90.0f : 90.0f;
            Vector3 parentCenter = centralPoint != null ? centralPoint.position : transform.parent.TransformPoint(new Vector3(-1,0,1));
            transform.RotateAround(parentCenter, Vector3.up, angle * Time.deltaTime);
        }

        if (isWHeld || isSHeld)
        {
            float angle = isWHeld ? -90.0f : 90.0f;
            transform.Rotate(new Vector3(angle * Time.deltaTime * lookSpeed, 0, 0), Space.Self);
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
