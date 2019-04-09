using System.Collections.Generic;
using UnityEngine;

public class WalkableGrid : MonoBehaviour
{
    public enum BendEdge
    {
        North, South, East, West, Top
    }

    public List<BendEdge> bendEdges;
    public int width = 9;
    public int height = 9;
    public GameObject oddTile;
    public GameObject evenTile;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        if (isValid())
        {
            GeneratePlane(Vector3.zero, Vector3.zero);
            for (int i = 0; i < bendEdges.Count; i++)
            {
                Vector3 rotation = getRotationForWall(i);
                Vector3 translation = getTranslationForWall(i);
                GeneratePlane(translation,rotation);
            }
        }
    }

    private Vector3 getRotationForWall(int i)
    {
        Vector3 rotation = Vector3.zero;
        
        switch (bendEdges[i])
        {
            case BendEdge.North:
                rotation.x = 90;
                break;
            case BendEdge.South:
                break;
            case BendEdge.East:
                break;
            case BendEdge.West:
                break;
            case BendEdge.Top:
                break;
        }

        return rotation;
    }

    private Vector3 getTranslationForWall(int i)
    {
        Vector3 vector = new Vector3();
        
        switch (bendEdges[i])
        {
            case BendEdge.North:
                vector.z = height;
                break;
            case BendEdge.South:
                vector.z = -height;
                break;
            case BendEdge.East:
                vector.x = width;
                break;
            case BendEdge.West:
                vector.x = -width;
                break;
            case BendEdge.Top:
                vector.y = height;
                break;
        }

        return vector;
    }

    private void GeneratePlane(Vector3 translation, Vector3 rotation)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject newTile = Instantiate((i + j) % 2 == 0 ? evenTile : oddTile, this.transform);
                newTile.transform.localPosition = new Vector3(j, 0, i) + translation;
                newTile.transform.Rotate(rotation);
            }
        }
    }

    private bool isValid()
    {
        return width > 0 && height > 0 && oddTile != null && evenTile != null;
    }
}
