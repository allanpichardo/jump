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

    public override bool Equals(object obj)
    {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType())) 
        {
            return false;
        }
        else { 
            TeleportPoint p = (TeleportPoint) obj;
            return p.transform.Equals(p.transform) && p.redirector.direction == this.redirector.direction;
        } 
    }
}