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
    public Material PortalMaterial;
    
    public enum Direction
    {
        Up, Down, Left, Right, Portal, None
    }

    public Direction direction = Direction.None;

    private Material defaultMaterial;
    private MeshRenderer meshRenderer;
    
    private const int CellWidth = 1;
    private const int CellHeight = 1;
    private float textureX = 0;
    private float textureY = 0;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
    }

    void FixedUpdate()
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
            case Direction.Portal:
                meshRenderer.material = PortalMaterial;
                AnimatePortal();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private void AnimatePortal()
    {
        textureX = (float) ((textureX + 0.25) % CellWidth);
        textureY = (float) ((textureY + 0.25) % CellHeight);
        meshRenderer.material.mainTextureOffset = new Vector2(textureX, textureY);
    }
}
