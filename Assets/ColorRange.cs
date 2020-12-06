using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorRange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float hoverTime;
    public float transitionStartTime;
    public float normalHeight;
    public float heightChange;
    public bool expand;
    private ColorRangeManager colorRangeManager;
    public Color from;
    public Color to;

    // Start is called before the first frame update
    void Start()
    {
        normalHeight = GetComponentInChildren<RectTransform>().rect.height;
        transitionStartTime -= hoverTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (expand)
        {
            float time = (Time.time - transitionStartTime) / hoverTime;
            setChildrenHeights(normalHeight + positionFunction(time) * heightChange);
        }
        else {
            float time = (Time.time - transitionStartTime) / hoverTime;
            setChildrenHeights(normalHeight + (1 - positionFunction(time)) * heightChange);
        }
    }

    private void setChildrenHeights(float height) {
        //Debug.Log("setting height... " + height);
        foreach (RectTransform child in GetComponentsInChildren<RectTransform>()) {
            SetSize(child, new Vector2(child.rect.width, height));
        }
    }
    public static void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }


    public void onHoverEnter() {
        expand = true;
        float time = (Time.time - transitionStartTime) / hoverTime;
        float currentHeightChange = (GetComponent<RectTransform>().rect.height - normalHeight) / heightChange;
        transitionStartTime = Time.time - hoverTime * (reversePositionFunction(currentHeightChange, expand)); ;
    }

    public void onHoverExit() {
        expand = false;
        float time = (Time.time - transitionStartTime) / hoverTime;
        float currentHeightChange = (GetComponent<RectTransform>().rect.height - normalHeight) / heightChange;
        transitionStartTime = Time.time - hoverTime * (reversePositionFunction(currentHeightChange, expand)); ;
    }

    public void setColors(Color from, Color to) {
        this.from = from;
        this.to = to;
        GetComponentInChildren<GradientUpdater>().setColors(from, to);
    }

    public float positionFunction(float time) {
        if (time >= 1)
            return 1;
        if (time <= 0)
            return 0;
        else return -Mathf.Pow(time - 1, 2) + 1;
    }

    public float reversePositionFunction(float heightChange, bool expand) {
        if (expand)
        {
            if (heightChange >= 1)
                return 1;
            else if (heightChange <= 0)
                return 0;
            else
            {
                return -Mathf.Sqrt(-heightChange + 1) + 1;
            }
        }
        else {
            if (heightChange >= 1)
                return 0;
            else if (heightChange <= 0)
                return 1;
            else
            {
                return -Mathf.Sqrt(heightChange) + 1;
            }
        }
    }

    public Color getColorFrom() {
        return from;
    }

    public Color getColorTo()
    {
        return to;
    }

    public void clicked() {
        colorRangeManager.selectRange(gameObject);
    }

    public void setColorRangeManager(ColorRangeManager cRM) {
        colorRangeManager = cRM;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked();
    }
}
