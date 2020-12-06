using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private float startTime;
    private bool fadeStarted;
    public float fadeTime;
    public bool outFade;
    public bool startFaded;
    // Start is called before the first frame update
    void Start()
    {
        if (startFaded) {
            Color color = GetComponent<Image>().color;
            color.a = 0;
            GetComponent<Image>().color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeStarted) {
            Color color = GetComponent<Image>().color;
            if(outFade)
                color.a = 1 - (Time.time - startTime) / fadeTime;
            else color.a = (Time.time - startTime) / fadeTime;
            GetComponent<Image>().color = color;
        }
    }

    public void fadeOut() {
        startTime = Time.time;
        fadeStarted = true;
        outFade = true;
    }

    public void fadeIn() {
        startTime = Time.time;
        fadeStarted = true;
        outFade = false;
    }
}
