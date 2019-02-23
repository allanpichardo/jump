using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadDetector : MonoBehaviour
{
    private Walker walker;
    
    private void Start()
    {
        walker = GetComponentInParent<Walker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Goal)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            if (walker != null)
            {
                walker.ChangeDirection(other);
            }
        }
    }
}
