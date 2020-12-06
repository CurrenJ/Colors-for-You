using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCycle : MonoBehaviour
{
    private Image image;
    public float hueCycleTime;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();   
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.HSVToRGB((Time.time % hueCycleTime) / hueCycleTime, 0.3F, 1);
    }
}
