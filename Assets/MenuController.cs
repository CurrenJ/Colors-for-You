using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuArrow1;
    public GameObject menuArrow2;
    public GameObject menuArrow2Image;
    public GameObject menuArrowBack;
    public GameObject menuPanel;
    public GameObject colorRangeManager;
    public bool colorRangeMenu;
    public bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Camera.main.GetComponent<GameController>().init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void swipeUp(Vector3 dragVector) {
        Debug.Log("DV: " + dragVector);
        if (!menuPanel.GetComponent<MoveUp>().moveStarted && !gameStarted && Mathf.Abs(dragVector.y) > Screen.height / 16) {
            if (!colorRangeMenu)
                startCubes();
            else {
                colorRangeMenu = false;
                menuArrow1.GetComponent<FadeOut>().fadeIn();
                menuPanel.GetComponent<MoveUp>().moveUp(false);
            }
        }
    }

    public void swipeDown(Vector3 dragVector) {
        if (!menuPanel.GetComponent<MoveUp>().moveStarted && !gameStarted)
        {
            if (!colorRangeMenu)
                colorRangePicker();
        }
    }

    public void startCubes() {
        menuArrow1.GetComponent<FadeOut>().fadeOut();
        menuArrow2.GetComponent<FadeOut>().fadeOut();
        menuArrow2Image.GetComponent<FadeOut>().fadeOut();
        menuArrowBack.GetComponent<FadeOut>().fadeIn();
        menuPanel.GetComponent<MoveUp>().moveUp(true);
        gameStarted = true;
        ColorRange range = colorRangeManager.GetComponent<ColorRangeManager>().selected;
        if(range != null)
            Camera.main.GetComponent<GameController>().setColorRange(range);
        Camera.main.GetComponent<GameController>().enabled = true;
        colorRangeMenu = false;
    }

    public void colorRangePicker() {
        colorRangeMenu = true;
        menuArrow1.GetComponent<FadeOut>().fadeOut();
        menuPanel.GetComponent<MoveUp>().moveDown(false);
    }

    public void endCubes()
    {
        menuPanel.GetComponent<MoveUp>().moveDown(true);
        menuArrow2.GetComponent<FadeOut>().fadeIn();
        menuArrow2Image.GetComponent<FadeOut>().fadeIn();
        menuArrow1.GetComponent<FadeOut>().fadeIn();
        menuArrowBack.GetComponent<FadeOut>().fadeOut();
        gameStarted = false;
        Camera.main.GetComponent<GameController>().enabled = false;
    }
}
