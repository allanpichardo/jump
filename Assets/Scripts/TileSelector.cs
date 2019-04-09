using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    public Camera camera;
    public Text portalsText;
    public Transform startingPosition;
    public Material positiveHighlightMaterial;
    public Material negativeHighlightMaterial;
    public float portalDistance = 10.0f;
    public List<Redirector.Direction> availableActions;
    public AudioClip portalOpenSound;
    public AudioSource audioSource;
    
    private Stack<Redirector.Direction> directions;
    private Dictionary<TeleportPoint, TeleportPoint> portals;
    private Stack<TeleportPoint> lastPoint;
    private GameObject lastHighlightedTile;
    private int maxAttempts;
    public LineRenderer lineRenderer;

    private void Start()
    {
        portals = new Dictionary<TeleportPoint, TeleportPoint>();
        lastPoint = new Stack<TeleportPoint>();
        directions = new Stack<Redirector.Direction>(availableActions);
        maxAttempts = availableActions.Count;
    }

    private bool IsDistanceValid(GameObject tile)
    {
        if (portals.Count == availableActions.Count) return false;

        Vector3 fromPosition = lastPoint.Count > 0 ? lastPoint.Peek().position : startingPosition.position;
        return Vector3.Distance(tile.transform.position, fromPosition) <= portalDistance;
    }

    // Update is called once per frame
    void Update()
    {
        DrawRangeGrid();

        lineRenderer.positionCount = 0;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
            
        DrawLines(hit.point);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider != null && hit.collider.gameObject.layer == Layers.Floor)
            {
                GameObject tile = hit.collider.gameObject;
                Redirector redirector = tile.GetComponent<Redirector>();

                if (IsDistanceValid(tile))
                {
                    if(maxAttempts > 0) redirector.SetHighlight(true);
                    
                    if (Input.GetMouseButtonDown(0) && maxAttempts > 0)
                    {
                        TeleportPoint tp = new TeleportPoint(hit.point, hit.collider.bounds,
                            hit.collider.gameObject.transform, redirector);

                        if (redirector.direction == Redirector.Direction.None && directions.Count > 0)
                        {
                            redirector.direction = directions.Pop();
                            if (redirector.direction == Redirector.Direction.InPortal)
                            {
                                portals.Add(tp, null);
                                lastPoint.Push(tp);
                            }
                            else if (redirector.direction == Redirector.Direction.OutPortal)
                            {
                                portals[lastPoint.Peek()] = tp;
                                lastPoint.Push(tp);
                            }

                            if (audioSource && portalOpenSound)
                            {
                                audioSource.PlayOneShot(portalOpenSound);
                            }

                            maxAttempts--;
                        }
                        else
                        {
                            if (tp.Equals(lastPoint.Peek()))
                            {
                                if (redirector.direction == Redirector.Direction.InPortal)
                                {
                                    portals.Remove(lastPoint.Peek());
                                }else if (redirector.direction == Redirector.Direction.OutPortal)
                                {
                                    var item = portals.First(kvp => kvp.Value.Equals(tp));
                                    portals[item.Key] = null;
                                }
                                directions.Push(redirector.direction);
                                redirector.direction = Redirector.Direction.None;
                                lastPoint.Pop();

                                maxAttempts++;
                            }
                        }
                    }
                }
                else
                {
                    if(maxAttempts > 0) redirector.SetHighlight(false);
                }
            }
        }

        if (portalsText)
        {
            int j = Mathf.CeilToInt(maxAttempts / 2.0f);
            string portals = j == 1 ? "JUMP" : "JUMPS";
            portalsText.text = j + " " + portals;
            portalsText.color = maxAttempts > 0 ? Color.white : Color.red;
        }
    }

    private void DrawRangeGrid()
    {
        Redirector[] redirectors = FindObjectsOfType<Redirector>();

        foreach(Redirector redirector in redirectors)
        {
            if(redirector.direction == Redirector.Direction.None)
            {
                redirector.SetReachable(IsDistanceValid(redirector.gameObject));
            }
        }
    }

    private void DrawLines(Vector3 mouseWorldPos)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (var kvp in portals)
        {
            positions.Add(kvp.Key.position);
            if (kvp.Value != null)
            {
                positions.Add(kvp.Value.position);
            }
            else
            {
                positions.Add(mouseWorldPos);
            }
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void ClosePortal(Redirector redirector)
    {
        var item = portals.First(kvp => kvp.Value.redirector.Equals(redirector));
        if (item.Key != null)
        {
            directions.Push(item.Value.redirector.direction);
            item.Value.redirector.direction = Redirector.Direction.None;
            lastPoint.Pop();
        
            directions.Push(item.Key.redirector.direction);
            item.Key.redirector.direction = Redirector.Direction.None;
            lastPoint.Pop();
        
            portals.Remove(item.Key);
        }
    }

    public TeleportPoint GetPortalOutletFrom(Transform start)
    {
        foreach (TeleportPoint point in portals.Keys)
        {
            if (point.transform.Equals(start))
            {
                return portals[point];
            }
        }

        return null;
    }
}
