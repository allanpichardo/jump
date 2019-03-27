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
    public Collider lookaheadPoint;

    private bool canChange;
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
        RaycastHit rayDown = CastRayDown();
        
        CheckForRedirect(rayDown);

        if (isActive)
        {
            TakeStep();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = true;
        }
    }

    private void TakeStep()
    {
        if (animator != null)
        {
            animator.SetTrigger(Move);
        }
        transform.Translate(currentDirection * speed * SpeedScale, Space.World);
        canChange = true;
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
                    case Redirector.Direction.InPortal:
                        Teleport(redirector.transform);
                        break;
                    case Redirector.Direction.None:
                        break;
                    case Redirector.Direction.OutPortal:
                        //tileSelector.ClosePortal(redirector);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void Teleport(Transform start)
    {
        TeleportPoint destination = tileSelector.GetPortalOutletFrom(start);
        if (destination != null && destination.redirector.direction == Redirector.Direction.OutPortal)
        {
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
                Vector3 floorBack = floor.TransformDirection(Vector3.back);
                
                if (dogForward == floorDown)
                {
                    Vector3 dir = dogForward == Vector3.down ? Vector3.left : Vector3.right;
                    currentDirection = floor.TransformDirection(dir);
                }else if (dogForward == floorUp)
                {
                    currentDirection = floor.TransformDirection(Vector3.right);
                }
                
                transform.rotation = Quaternion.LookRotation(currentDirection, floorUp);
            }
            tileSelector.ClosePortal(destination.redirector);
        }
    }

    public void ChangeDirection(Collider collider)
    {
        if (collider.gameObject.layer == Layers.Obstacle)
        {
            currentDirection = Vector3.Cross(transform.TransformDirection(Vector3.up), currentDirection);
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(currentDirection, transform.up));
        }
        else if(collider.gameObject.layer == Layers.Floor && canChange)
        {
            //currentDirection = Vector3.Cross(currentDirection, transform.TransformDirection(Vector3.right));
            currentDirection = transform.up;
            Vector3 ahead = transform.forward * (lookForwardDistance * 0.66f);
            transform.localPosition += ahead;
            transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(currentDirection, -1 * transform.forward));
            canChange = false;
        }
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

    public void OnStartClicked()
    {
        isActive = true;
    }

}
