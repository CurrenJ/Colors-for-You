using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }

    public void openURL() {
        Application.OpenURL("https://currenj.github.io/cfy/privacy.html");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Press position + " + eventData.pressPosition);
        //Debug.Log("End position + " + eventData.position);
        Vector3 unnormalizedDragVector = (eventData.position - eventData.pressPosition);
        Vector3 dragVectorDirection = unnormalizedDragVector.normalized;
        //Debug.Log("norm + " + dragVectorDirection);
        if (GetDragDirection(dragVectorDirection) == DraggedDirection.Up)
        {
            GetComponentInChildren<MenuController>().swipeUp(unnormalizedDragVector);
        }
        else if (GetDragDirection(dragVectorDirection) == DraggedDirection.Down) {
            GetComponentInChildren<MenuController>().swipeDown(unnormalizedDragVector);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
