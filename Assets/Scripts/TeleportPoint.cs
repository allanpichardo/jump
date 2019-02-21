using UnityEngine;

public class TeleportPoint
{
    public TeleportPoint(Vector3 position, Bounds bounds, Transform transform, Redirector redirector)
    {
        this.position = position;
        this.bounds = bounds;
        this.transform = transform;
        this.redirector = redirector;
    }

    public Transform transform;
    public Vector3 position;
    public Bounds bounds;
    public Redirector redirector;
}