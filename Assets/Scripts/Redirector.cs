using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Redirector : MonoBehaviour
{
    public Material UpMaterial;
    public Material DownMaterial;
    public Material LeftMaterial;
    public Material RightMaterial;
    
    public enum Direction
    {
        Up, Down, Left, Right, None
    }

    public Direction direction = Direction.None;

    private Material defaultMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }

    void Update()
    {
        switch (direction)
        {
            case Direction.Up:
                meshRenderer.material = UpMaterial;
                break;
            case Direction.Down:
                meshRenderer.material = DownMaterial;
                break;
            case Direction.Left:
                meshRenderer.material = LeftMaterial;
                break;
            case Direction.Right:
                meshRenderer.material = RightMaterial;
                break;
            case Direction.None:
                meshRenderer.material = defaultMaterial;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
    
}
