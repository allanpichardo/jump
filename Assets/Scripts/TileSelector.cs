using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    public Camera camera;
    public List<Redirector.Direction> availableActions;
    private Stack<Redirector.Direction> directions;
    private List<Transform> portals;

    private void Start()
    {
        portals = new List<Transform>();
        directions = new Stack<Redirector.Direction>(availableActions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider != null && hit.collider.gameObject.layer == Layers.Floor)
            {
                Redirector redirector = hit.collider.gameObject.GetComponent<Redirector>();

                if (redirector.direction == Redirector.Direction.None && directions.Count > 0)
                {
                    redirector.direction = directions.Pop();
                    portals.Add(redirector.transform);
                }
                else
                {
                    directions.Push(redirector.direction);
                    redirector.direction = Redirector.Direction.None;
                    portals.Remove(redirector.transform);
                }
            }
        }
    }

    public Transform GetPortalOutletFrom(Transform start)
    {
        foreach (var p in portals)
        {
            if (!p.Equals(start))
            {
                return p.transform;
            }
        }

        return start;
    }

    public int GetActivePortalCount()
    {
        return portals.Count;
    }

    public void ClearPortals()
    {
        foreach (var p in portals)
        {
            Redirector redirector = p.GetComponent<Redirector>();
            directions.Push(redirector.direction);
            redirector.direction = Redirector.Direction.None;
            portals.Remove(redirector.transform);
        }
    }
}
