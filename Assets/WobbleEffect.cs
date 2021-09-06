using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    private Vector2 startScale;
    private Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startScale = this.transform.localScale;
        startPos = this.transform.localPosition;   
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = startScale + new Vector2(0.02f * Mathf.Sin(Time.time), 0.01f * Mathf.Cos(Time.time));

        Vector3 rot = this.transform.localEulerAngles;
        rot.z = 2.5f * Mathf.Cos(Time.time / 2f);
        this.transform.localEulerAngles = rot;

        this.transform.localPosition = startPos + new Vector2(Screen.width / 96f * Mathf.Sin(Time.time * 1.2f), Screen.width / 96f * Mathf.Cos(Time.time / 1.8f));
    }
}
