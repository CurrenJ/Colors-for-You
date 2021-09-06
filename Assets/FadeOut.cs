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
    Image imageComponent;
    TMPro.TextMeshProUGUI tmpComponent;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Image>(out imageComponent);
        TryGetComponent<TMPro.TextMeshProUGUI>(out tmpComponent);

        if (startFaded) {
            Color color = GetCurrentColor();
            color.a = 0;
            SetCurrentColor(color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeStarted) {
            Color color = GetCurrentColor();
            if(outFade)
                color.a = 1 - (Time.time - startTime) / fadeTime;
            else color.a = (Time.time - startTime) / fadeTime;
            SetCurrentColor(color);
        }
    }

    private Color GetCurrentColor(){
        if(imageComponent != null)
            return imageComponent.color;
        else if(tmpComponent != null)
            return tmpComponent.color;

        return Color.clear;
    }

    private void SetCurrentColor(Color color){
        if(imageComponent != null)
            imageComponent.color = color;
        if(tmpComponent != null)
            tmpComponent.color = color;
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
