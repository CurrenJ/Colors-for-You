using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientUpdater : MonoBehaviour
{
    public Shader gradentMaterialShader;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setColors(Color from, Color to) {
        Material gradientMaterial = new Material(gradentMaterialShader);
        gradientMaterial.SetColor("_Color", from); // the left color
        gradientMaterial.SetColor("_Color2", to); // the right color
        GetComponent<Image>().material = gradientMaterial;
    }
}
