using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    GamePlayManager gamePlayManager;
    float pressTime = 0.0f;
    Vector2 pressStartPos = Vector2.zero;
    bool isSwipe;
    float minSwipeDistance = 50f;
    float maxSwipeTime = 1.5f;
    public GameObject stopPanel;

    void Start () {
        gamePlayManager = GamePlayManager.Instance();
	}
	
	void Update () {
        if(Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            StopVisual();
        }
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        pressTime = Time.time;
                        pressStartPos = touch.position;
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;
                    case TouchPhase.Ended:
                        float fingerTime = Time.time - pressTime;
                        float fingerDir = (touch.position - pressStartPos).magnitude;
                        if (isSwipe && fingerTime < maxSwipeTime && fingerDir > minSwipeDistance)
                        {
                            Vector2 dir = touch.position - pressStartPos;
                            Vector2 swipType = Vector2.zero;
                            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                            {
                                swipType = Vector2.right * Mathf.Sign(dir.x);
                            }
                            else
                                swipType = Vector2.up * Mathf.Sign(dir.y);
                            if (swipType.x != 0)
                            {
                                if (swipType.x > 0)
                                {
                                    gamePlayManager.Move(MoveDirection.Right);
                                }
                                else
                                    gamePlayManager.Move(MoveDirection.Left);
                            }
                            if (swipType.y != 0)
                            {
                                if (swipType.y > 0)
                                {
                                    gamePlayManager.Move(MoveDirection.Up);
                                }
                                else
                                    gamePlayManager.Move(MoveDirection.Down);
                            }
                        }
                        break;
                }
            }
        }
	}

    public void StopVisual()
    {
        stopPanel.SetActive(true);
    }

    public void Resume()
    {
        stopPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
