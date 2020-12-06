using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaling : MonoBehaviour
{
    public GameObject arrowDown;
    public GameObject colorArrow;
    public GameObject colorRangeListParent;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log((Screen.height / 2) - (Screen.height / 16));
        float y = (Screen.height / 2) - (Screen.height / 16);
        colorArrow.transform.localPosition = new Vector3(arrowDown.transform.localPosition.x, y, arrowDown.transform.localPosition.z);
        colorArrow.GetComponent<Bob>().setOrigin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
