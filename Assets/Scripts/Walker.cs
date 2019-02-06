using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float speed = 1.0f;
    public float lookDistance = 0.5f;
    private Vector3 currentDirection;
    private const float SpeedScale = 0.1f;
    private Animator animator;
    private Collider collider;
    private static readonly int Move = Animator.StringToHash("move");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentDirection = transform.TransformDirection(Vector3.forward);
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit = CastRay();
        
        if (!CanMoveForward(hit))
        {
            ChangeDirection(hit);
        }
        TakeStep();
    }

    private void TakeStep()
    {
        if (animator != null)
        {
            animator.SetTrigger(Move);
        }
        transform.Translate(currentDirection * speed * SpeedScale, Space.World);
    }

    private void ChangeDirection(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == Layers.Obstacle)
        {
            currentDirection = Vector3.Cross(transform.TransformDirection(Vector3.up), currentDirection);
            transform.Rotate(transform.up, 90.0f);
        }
        else if(hit.collider.gameObject.layer == Layers.Floor)
        {
            currentDirection = Vector3.Cross(currentDirection, transform.TransformDirection(Vector3.right));
            transform.Rotate(transform.right, -90.0f);
        }
    }

    private RaycastHit CastRay()
    {
        RaycastHit hit;
        var position = transform.position;
        
        Ray ray = new Ray(position, currentDirection);
        bool isDetected = Physics.Raycast(ray, out hit, lookDistance);
        
        if (isDetected)
        {
            Debug.DrawRay(position, currentDirection * lookDistance, Color.red, Time.deltaTime);
        }
        else
        {
            Debug.DrawRay(position, currentDirection * lookDistance, Color.white, Time.deltaTime);
        }
        
        return hit;
    }
   
    private bool CanMoveForward(RaycastHit hit)
    {
        var o = hit.collider != null ? hit.collider.gameObject : null;
        return !(hit.collider != null && (o.layer == Layers.Obstacle || o.layer == Layers.Floor));
    }
}
