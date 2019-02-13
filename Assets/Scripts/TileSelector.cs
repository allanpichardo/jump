using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileSelector : MonoBehaviour
{
    public Camera camera;
    public GameObject tilePanel;
    public List<Redirector.Direction> availableActions;
    private Stack<Redirector.Direction> directions;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private void Start()
    {
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

                if (redirector.direction == Redirector.Direction.None)
                {
                    redirector.direction = directions.Pop();
                }
                else
                {
                    directions.Push(redirector.direction);
                    redirector.direction = Redirector.Direction.None;
                }
            }
        }

        if (tilePanel != null)
        {
            foreach (var direction in directions)
            {
                
            }
        }

    }

    private GameObject GetArrowIcon(Redirector.Direction direction)
    {
        GameObject NewObj = new GameObject(); //Create the GameObject
        Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
        switch (direction)
        {
            case Redirector.Direction.Up:
                NewImage.sprite = upSprite;
                break;
            case Redirector.Direction.Down:
                NewImage.sprite = downSprite;
                break;
            case Redirector.Direction.Left:
                NewImage.sprite = leftSprite;
                break;
            case Redirector.Direction.Right:
                NewImage.sprite = rightSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return NewObj;
    }
}
