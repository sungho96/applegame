using System.Collections.Generic;
using UnityEngine;

public class MouseDragController : MonoBehaviour
{   
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private AppleSpawner appleSpawner;

    [SerializeField]
    private RectTransform dragRectangle;

    private int sum = 0;
    private Rect dragRect;
    private Vector2 start = Vector2.zero;
    private Vector2 end = Vector2.zero;
    private AudioSource audioSource;
    private List<Apple> selectedAppleList = new List<Apple>();

    private void Awake()
    {
        dragRect = new Rect();
        audioSource = GetComponent<AudioSource>();

        //start,end (0,0)인 상태로 이미지 크기 00 설정 화면에 보이기 x
        DrawDragRectangle();
    }

    private void Update()
    {
        if (gameController.IsGameStart == false) return;
        if (Input.GetMouseButtonDown(0))
        {   
            start = Input.mousePosition;

            dragRect.Set(0,0,0,0); 
        }

        if ( Input.GetMouseButton(0))
        {
            Debug.Log($"총합 :{sum}");
            if ( sum == 10)
            {
                int score = 0;
                foreach ( Apple apple in selectedAppleList)
                {
                    score ++;
                    appleSpawner.DestroyApple(apple);
                }

                audioSource.Play();
                gameController.IncreaseScore(score);
            }
            else 
            {
                foreach( Apple apple in selectedAppleList)
                {
                    apple.OnDeselected();
                }
            }
            end = Input.mousePosition;

            //드래그 범위 이미지로 표현
            //영역 이미지 화면출력,계산,사과 선택
            DrawDragRectangle();
            CalculateDragRect();
            SelectApples();

        }
        if ( Input.GetMouseButtonUp(0))
        {
            //마우스 클릭 종료 때 드래그 범위 보이기 x
            //start,end 위치를 00 설정하고 드래그 범위 그리기
            start = end = Vector2.zero;
            DrawDragRectangle();

        }
    }

    private void DrawDragRectangle()
    {
        Vector2 uiStart, uiEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragRectangle.parent as RectTransform, start, null, out uiStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragRectangle.parent as RectTransform, end, null, out uiEnd);

        dragRectangle.anchoredPosition = (uiStart + uiEnd) * 0.5f;
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(uiStart.x - uiEnd.x), Mathf.Abs(uiStart.y - uiEnd.y));
    }
    private void CalculateDragRect()
    {
        if (Input.mousePosition.x < start.x)
        {
            dragRect.xMin = Input.mousePosition.x;
            dragRect.xMax = start.x;
        }
        else
        {
            dragRect.xMin = start.x;
            dragRect.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < start.y)
        {
            dragRect.yMin = Input.mousePosition.y;
            dragRect.yMax = start.y;
        }
        else
        {
            dragRect.yMin = start.y;
            dragRect.yMax = Input.mousePosition.y;
        }
    }
    
    private void SelectApples()
    {   
        sum=0;
        selectedAppleList.Clear();

        foreach (Apple apple in appleSpawner.AppleList)
        {
            //사과중심 드래그 영역
            if(  dragRect.Contains(apple.Position))
            {
                apple.OnSelected();
                selectedAppleList.Add(apple);
                sum += apple.Number;
            }
            else
            {
                apple.OnDeselected();
            }
        }
    }
}
