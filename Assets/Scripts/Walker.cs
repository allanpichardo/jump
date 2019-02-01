using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 currentDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        currentDirection = transform.TransformDirection(Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMoveForward())
        {
            ChangeDirection();
        }
        TakeStep();
    }

    private void TakeStep()
    {
        transform.Translate(currentDirection * Time.deltaTime * speed, Space.World);
    }

    private void ChangeDirection()
    {
        currentDirection = Vector3.Cross(transform.TransformDirection(Vector3.up), currentDirection);
    }
   
    private bool CanMoveForward()
    {
        int layerMask = 1 << 10;

        var position = transform.position;
        Debug.DrawRay(position, currentDirection * 0.5f, Color.yellow, Time.deltaTime);
        return !Physics.Raycast(position, currentDirection, 0.5f, layerMask);
    }
}
