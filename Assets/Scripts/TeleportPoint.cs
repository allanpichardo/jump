using UnityEngine;

public class TeleportPoint
{
    public TeleportPoint(Vector3 position, Bounds bounds, Transform transform)
    {
        this.position = position;
        this.bounds = bounds;
        this.transform = transform;
    }

    public Transform transform;
    public Vector3 position;
    public Bounds bounds;
}