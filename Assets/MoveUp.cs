using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    private float startTime;
    private Vector3 startPosition;
    public bool moveStarted;
    public float moveTime;
    private float moveDist;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moveStarted) {
            transform.localPosition = new Vector3(startPosition.x, startPosition.y + moveDist * positionFunction(((Time.time - startTime) / moveTime)), startPosition.z);
        }
    }

    public float positionFunction(float time) {
        if (time >= 1)
        {
            moveStarted = false;
            return 1;
        }
        else if (time <= 0)
            return 0;
        else return -Mathf.Pow(time - 1f, 2f) + 1f;
    }

    public void moveUp(bool startGame) {
        if (!moveStarted)
        {
            moveStarted = true;
            startTime = Time.time;
            startPosition = transform.localPosition;
            if (startGame)
            {
                moveDist = GetComponent<RectTransform>().rect.height / 2 + GetComponent<RectTransform>().rect.height / 16;
                moveTime = 1;
            }
            else {
                moveTime = 1.25F;
                moveDist = (7 * (GetComponent<RectTransform>().rect.height / 8));
            }
        }
    }

    public void moveDown(bool endGame) {
        if (!moveStarted)
        {
            moveStarted = true;
            startTime = Time.time;
            startPosition = transform.localPosition;
            if (endGame)
            {
                moveDist = -(GetComponent<RectTransform>().rect.height / 2 + GetComponent<RectTransform>().rect.height / 16);
                moveTime = 1;
            }
            else
            {
                moveTime = 1.25F;
                moveDist = -(7 * (GetComponent<RectTransform>().rect.height / 8));
            }
        }
    }
}
