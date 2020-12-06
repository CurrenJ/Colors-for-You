using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRangeManager : MonoBehaviour
{
    public GameObject colorRangePrefab;
    public List<GameObject> colorRanges;
    public GameObject listParent;
    public float gap;
    public ColorRange selected;
    public GameObject noneButton;

    // Start is called before the first frame update
    void Start()
    {
        colorRanges = new List<GameObject>();
        addColorRange("96cdff", "dbbadd");
        addColorRange("cbdfbd", "f19c79");
        addColorRange("cc2936", "eee5e9");
        addColorRange("e5e8b6", "586994");
        addColorRange("e2f89c", "3f7cac");
        addColorRange("f39c6b", "ff3864");
        addColorRange("688e26", "faa613");
        addColorRange("550527", "f44708");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectRange(GameObject colorRange) {
        if(selected != null)
            selected.transform.localScale = new Vector3(1, 1, 1);
        colorRange.transform.localScale = new Vector3(1.15F, 1.15F, 1F);
        noneButton.transform.localScale = new Vector3(1F, 1F, 1F);
        selected = colorRange.GetComponent<ColorRange>();

    }

    public void selectRange() {
        Debug.Log("none.");
        if (selected != null)
            selected.transform.localScale = new Vector3(1, 1, 1);
        noneButton.transform.localScale = new Vector3(1.25F, 1.25F, 1F);
        selected = null;
    }

    public void addColorRange(Color from, Color to) {
        GameObject colorRange = Instantiate(colorRangePrefab, listParent.transform);
        colorRanges.Add(colorRange);
        colorRange.GetComponent<ColorRange>().setColorRangeManager(GetComponent<ColorRangeManager>());
        colorRange.GetComponent<ColorRange>().setColors(from, to);
        if (colorRanges.Count > 1) {
            ColorRange component = colorRanges[colorRanges.Count - 2].GetComponent<ColorRange>();
            colorRange.transform.localPosition = new Vector3(colorRange.transform.localPosition.x, colorRange.transform.localPosition.y - (colorRanges.Count-1) * (component.GetComponent<RectTransform>().rect.height + component.heightChange / 2 + gap), colorRange.transform.localPosition.z);
        }
    }

    public void addColorRange(string hexFrom, string hexTo) {
        Color from;
        Color to;

        if (!hexFrom.Substring(0, 1).Equals("#"))
            ColorUtility.TryParseHtmlString("#" + hexFrom, out from);
        else ColorUtility.TryParseHtmlString(hexFrom, out from);

        if(!hexTo.Substring(0, 1).Equals("#"))
            ColorUtility.TryParseHtmlString("#" + hexTo, out to);
        else ColorUtility.TryParseHtmlString(hexTo, out to);

        addColorRange(from, to);
    }
}
