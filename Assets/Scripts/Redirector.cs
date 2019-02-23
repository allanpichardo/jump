using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Redirector : MonoBehaviour
{
    public Material InPortalMaterial;
    public Material OutPortalMaterial;
    public Material positiveMaterial;
    public Material negativeMaterial;
    
    public enum Direction
    {
        InPortal, OutPortal, None
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

    private void Update()
    {
        if (direction != Direction.None)
        {
            AnimatePortal();
        }
    }

    void FixedUpdate()
    {
        switch (direction)
        {
            case Direction.None:
                meshRenderer.material = defaultMaterial;
                break;
            case Direction.InPortal:
                meshRenderer.material = InPortalMaterial;
                break;
            case Direction.OutPortal:
                meshRenderer.material = OutPortalMaterial;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public void SetHighlight(bool isPositive)
    {
        ClearHighlight();
        
        if (isPositive)
        {
            meshRenderer.material = positiveMaterial;
        }
        else
        {
            meshRenderer.material = negativeMaterial;
        }
    }

    private void OnMouseExit()
    {
        ClearHighlight();
    }

    public void ClearHighlight()
    {
        meshRenderer.material = defaultMaterial;
    }

    private void AnimatePortal()
    {
        textureX = (float) ((textureX + 0.25) % CellWidth);
        textureY = (float) ((textureY + 0.25) % CellHeight);
        meshRenderer.material.mainTextureOffset = new Vector2(textureX, textureY);
    }
}
