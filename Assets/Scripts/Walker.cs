using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public GameObject level;
    public bool isActive = false;
    public float speed = 1.0f;
    public float lookForwardDistance = 0.5f;
    public float lookDownDistance = 0.5f;
    public float redirectorDistance = 0.5f;

    private TileSelector tileSelector;
    private Vector3 currentDirection;
    private const float SpeedScale = 0.1f;
    private Animator animator;
    private Collider collider;
    private static readonly int Move = Animator.StringToHash("move");

    // Start is called before the first frame update
    void Start()
    {
        tileSelector = level.GetComponent<TileSelector>();
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
        if (hit.collider != null && hit.collider.gameObject.layer == Layers.Floor)
        {
            Bounds floorBounds = hit.collider.bounds;
            Redirector redirector = hit.collider.gameObject.GetComponent<Redirector>();
            if (redirector.direction != Redirector.Direction.None && Vector3.Distance(floorBounds.center, transform.position) < redirectorDistance)
            {
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
                    case Redirector.Direction.Portal:
                        Teleport(redirector.transform);
                        break;
                    case Redirector.Direction.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void Teleport(Transform start)
    {
        if (tileSelector.GetActivePortalCount() == 2)
        {
            TeleportPoint destination = tileSelector.GetPortalOutletFrom(start);
            Bounds b = destination.bounds;
            Vector3 maxUp = Vector3.Scale(b.extents, destination.transform.up);
            transform.position = destination.position + maxUp;
            Transform floor = destination.transform.parent.parent.parent;
            
            if (!start.parent.parent.parent.rotation.Equals(floor.rotation))
            {
                Vector3 dogForward = transform.TransformDirection(Vector3.forward);
                Vector3 floorDown = floor.TransformDirection(Vector3.down);
                Vector3 floorForward = floor.TransformDirection(Vector3.forward);
                Vector3 floorUp = floor.TransformDirection(Vector3.up);
                if (dogForward == floorDown)
                {
                    Vector3 dir = dogForward == Vector3.down ? Vector3.left : Vector3.right;
                    currentDirection = floor.TransformDirection(dir);
                    transform.Rotate(Vector3.right, -90.0f);
                }else if (dogForward == floorForward)
                {
                    transform.Rotate(Vector3.forward, -90.0f);
                }else if (dogForward == floorForward * -1)
                {
                    transform.Rotate(Vector3.forward, 90.0f);
                }else if (dogForward == floorUp)
                {
                    currentDirection = floor.TransformDirection(Vector3.right);
                    transform.Rotate(Vector3.right, 90.0f);
                }
            }
            //transform.LookAt(currentDirection);
            tileSelector.ClearPortals();
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
            currentDirection = Vector3.Cross(currentDirection, transform.TransformDirection(Vector3.right));
            Vector3 ahead = transform.forward * (lookForwardDistance / 2);
            //ahead = transform.TransformVector(ahead) + transform.position;
            transform.localPosition += ahead;
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(currentDirection, -1 * transform.forward));
            
        }
    }

    private RaycastHit CastRayForward()
    {
        RaycastHit hit;
        var position = transform.position;
        
        Ray ray = new Ray(position, currentDirection);
        bool isDetected = Physics.Raycast(ray, out hit, lookForwardDistance);
        
        if (isDetected)
        {
            Debug.DrawRay(position, currentDirection * lookForwardDistance, Color.red, Time.deltaTime);
        }
        else
        {
            Debug.DrawRay(position, currentDirection * lookForwardDistance, Color.white, Time.deltaTime);
        }
        
        return hit;
    }
    
    private RaycastHit CastRayDown()
    {
        RaycastHit hit;
        var position = transform.position;
        
        Ray ray = new Ray(position, -1 * transform.up);
        bool isDetected = Physics.Raycast(ray, out hit, lookDownDistance);
        
        if (isDetected)
        {
            Debug.DrawRay(position, -1 * transform.up * lookDownDistance, Color.red, Time.deltaTime);
        }
        else
        {
            Debug.DrawRay(position, -1 * transform.up * lookDownDistance, Color.white, Time.deltaTime);
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
