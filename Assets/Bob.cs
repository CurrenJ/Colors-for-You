using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    private Vector3 startPosition;
    private float startTime;
    private bool startPosSet;

    public float bobTime;
    private float bobDepth;
    // Start is called before the first frame update
    void Start()
    {
        if(!startPosSet)
            startPosition = transform.localPosition;
        startTime = Time.time;
        bobDepth = GetComponent<RectTransform>().rect.height / 10;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + bobDepth * positionFunction((Time.time - startTime) % bobTime / bobTime);
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }

    public void setOrigin(Vector3 pos) {
        startPosition = pos;
        startPosSet = true;
    }

    public void setOrigin() {
        setOrigin(transform.position);
    }

    public float positionFunction(float time) {
        if (time >= 1 || time <= 0)
            return 0;
        else return Mathf.Sin(2 * Mathf.PI * time);
    }
}
