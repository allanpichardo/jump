using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private Vector3 currentDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        currentDirection = transform.TransformDirection(Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        CanMoveForward();
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Changing direction");
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        currentDirection = Vector3.Cross(transform.TransformDirection(Vector3.up), currentDirection);
    }
   
    private bool CanMoveForward()
    {
        int layerMask = 1 << 10;

        var position = transform.position;
        Debug.DrawRay(position, currentDirection * 1000, Color.black, Time.deltaTime);
        return Physics.Raycast(position, currentDirection, 1.0f, layerMask);
    }
}
