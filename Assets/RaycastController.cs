using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                Raycast(Input.GetTouch(i).position);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
            Raycast(Input.mousePosition);
        }
    }

    public void Raycast(Vector2 touchPosition) {
        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        // Create a particle if hit
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.GetComponent<Cube>() != null)
                hit.transform.GetComponent<Cube>().doDepthBounce();
        }
    }
}
