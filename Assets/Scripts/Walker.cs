using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public bool isActive = false;
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
        RaycastHit rayForward = CastRayForward();
        RaycastHit rayDown = CastRayDown();
        
        if (!CanMoveForward(rayForward))
        {
            ChangeDirection(rayForward);
        }
        
        CheckForRedirect(rayDown);

        if (isActive)
        {
            TakeStep();
        }
    }

    private void TakeStep()
    {
        if (animator != null)
        {
            animator.SetTrigger(Move);
        }
        transform.Translate(currentDirection * speed * SpeedScale, Space.World);
    }

    private void CheckForRedirect(RaycastHit hit)
    {
        if (hit.collider != null && Vector3.Distance(transform.position, hit.collider.gameObject.GetComponent<Renderer>().bounds.center) < 0.5f)
        {
            Redirector redirector = hit.collider.gameObject.GetComponent<Redirector>();
            if (redirector.direction != Redirector.Direction.None)
            {
                Vector3 newDirection;
                switch (redirector.direction)
                {
                    case Redirector.Direction.Up:
                        currentDirection = Vector3.forward;
                        transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(Vector3.forward, transform.up));
                        break;
                    case Redirector.Direction.Down:
                        currentDirection = -1 * Vector3.forward;
                        transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-1 * Vector3.forward, transform.up));
                        break;
                    case Redirector.Direction.Left:
                        currentDirection = -1 * Vector3.right;
                        transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(-1 * Vector3.right, transform.up));
                        break;
                    case Redirector.Direction.Right:
                        currentDirection = Vector3.right;
                        transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(Vector3.right, transform.up));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void ChangeDirection(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == Layers.Obstacle)
        {
            currentDirection = Vector3.Cross(transform.TransformDirection(Vector3.up), currentDirection);
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(currentDirection, transform.up));
        }
        else if(hit.collider.gameObject.layer == Layers.Floor)
        {
            //Physics.gravity = currentDirection;
            currentDirection = Vector3.Cross(currentDirection, transform.TransformDirection(Vector3.right));
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(currentDirection, -1 * transform.forward));
        }
    }

    private RaycastHit CastRayForward()
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
    
    private RaycastHit CastRayDown()
    {
        RaycastHit hit;
        var position = transform.position;
        
        Ray ray = new Ray(position, -1 * transform.up);
        float lookDistance = 0.3f;
        bool isDetected = Physics.Raycast(ray, out hit, lookDistance);
        
        if (isDetected)
        {
            Debug.DrawRay(position, -1 * transform.up * lookDistance, Color.red, Time.deltaTime);
        }
        else
        {
            Debug.DrawRay(position, -1 * transform.up * lookDistance, Color.white, Time.deltaTime);
        }
        
        return hit;
    }
   
    private bool CanMoveForward(RaycastHit hit)
    {
        
        var o = hit.collider != null ? hit.collider.gameObject : null;
        return !(hit.collider != null && (o.layer == Layers.Obstacle || o.layer == Layers.Floor));
    }

    public void OnStartClicked()
    {
        isActive = true;
    }
}
