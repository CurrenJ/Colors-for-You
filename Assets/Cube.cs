using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float startTime;
    public float timeToReach;
    public bool infoSet;
    public Vector2 frustumDims;
    private Vector2 location;

    private float depthBounceTime = 0.5F;
    private float depthBounceDistance = 0.9F;
    public bool depthBounce;
    private float depthBounceStartTime;
    private float startDepth;

    void Start()
    {
        depthBounce = false;
        depthBounceStartTime = -depthBounceTime;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (infoSet) {
    //    //    transform.localPosition = getPosition();

    //    //    if (transform.position.y + transform.lossyScale.y / 2 < Camera.main.transform.position.y - frustumDims.y / 2)
    //    //    {
    //    //        Destroy(gameObject);
    //    //    }

    //    //    if (depthBounce) {
    //    //        doDepthBounce();
    //    //    }
    //    //}

    //}

    public float depthEasingFunction(float time) {
        if (time >= 1)
            return 0;
        else if (time <= 0)
            return 0;
        else return -4 * Mathf.Pow(time-0.5F, 2) + 1;
    }

    public float getDepth(bool moveBackwards) {
        if (!moveBackwards)
            return startDepth - depthEasingFunction((Time.time - depthBounceStartTime) / depthBounceTime) * depthBounceDistance;
        else return startDepth + depthEasingFunction((Time.time - depthBounceStartTime) / depthBounceTime) * depthBounceDistance;
    }

    public Vector3 getPosition() {
        float time = (Time.time - startTime) / timeToReach;
        float scalar = positionFunction(time);
        Vector3 newPosition = (endPosition - startPosition) * scalar + startPosition;
        return new Vector3(transform.position.x, newPosition.y, getDepth(false));
    }

    //returns a scalar from 0 to 1 given the time expressed as a 0 to 1 scalar
    public float positionFunction(float progress) {
        if (progress >= 1) //safeguard
            return 1;
        else if (progress <= 0) //safeguard
            return 0;
        else return -Mathf.Pow(progress - 1, 2) + 1; //position easing function
    }
    public void setCubeInfo(Vector3 startPosition, Vector3 endPosition, float timeToReach, Color color, Vector2 frustumDims, Vector2 location) {
        this.endPosition = endPosition;
        this.timeToReach = timeToReach;
        this.startPosition = startPosition;
        startTime = Time.time;
        infoSet = true;
        GetComponent<Renderer>().material.color = color;
        this.frustumDims = frustumDims;
        this.location = location;
        startDepth = startPosition.z;
    }

    public void doDepthBounce() {
        if (Time.time - depthBounceStartTime >= depthBounceTime)
        {
            depthBounce = false;
            depthBounceStartTime = Time.time;
        }
    }
}
