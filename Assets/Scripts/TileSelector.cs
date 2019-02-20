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
    private List<TeleportPoint> portals;
    private List<Redirector> activeRedirectors;

    private void Start()
    {
        portals = new List<TeleportPoint>();
        activeRedirectors = new List<Redirector>();
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
                TeleportPoint tp = new TeleportPoint(hit.point, hit.collider.bounds, hit.collider.gameObject.transform);

                if (redirector.direction == Redirector.Direction.None && directions.Count > 0)
                {
                    redirector.direction = directions.Pop();
                    portals.Add(tp);
                    activeRedirectors.Add(redirector);
                }
                else
                {
                    directions.Push(redirector.direction);
                    redirector.direction = Redirector.Direction.None;
                    portals.Remove(tp);
                    activeRedirectors.Remove(redirector);
                }
            }
        }
    }

    public TeleportPoint GetPortalOutletFrom(Transform start)
    {
        foreach (var p in portals)
        {
            if (!p.transform.Equals(start))
            {
                return p;
            }
        }

        return null;
    }

    public int GetActivePortalCount()
    {
        return activeRedirectors.Count;
    }

    public void ClearPortals()
    {
        foreach (var redirector in activeRedirectors)
        {
            directions.Push(redirector.direction);
            redirector.direction = Redirector.Direction.None;
        }
        portals.Clear();
        activeRedirectors.Clear();
    }
}
