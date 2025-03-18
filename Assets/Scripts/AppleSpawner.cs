using UnityEngine;
using System.Collections.Generic;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject applePrefab;
    [SerializeField]
    private Transform appleParent;

    private readonly int width = 17, height = 10;
    private readonly int spacing =20;
    private List<Apple> appleList =new List<Apple>();
    public List<Apple> AppleList => appleList;

    private void Awake()
    {
        SpawnApples();   
    }

    int sum = 0;
    public void SpawnApples()
    {
        //프리팹 크기를 구해서 간격 만큼 추가
        Vector2 size = applePrefab.GetComponent<RectTransform>().sizeDelta;
        size += new Vector2(spacing, spacing);
        
        //w * h 개수만큼 사과 생성
        for ( int y =0; y < height; ++y)
        {
            for(int x = 0; x < width; ++ x)
            {
                GameObject clone = Instantiate(applePrefab, appleParent);
                RectTransform rect = clone.GetComponent<RectTransform>();

                float px = (-width * 0.5f + 0.5f + x) *size.x;
                float py = (height * 0.5f - 0.5f - y) *size.y;
                rect.anchoredPosition = new Vector2(px,py);

                Apple apple = clone.GetComponent<Apple>();
                apple.Number = Random.Range(1,10);

                //전체 합이 10의 배수가 되도록 마지막 사과의 숫자 선정
                if ( y == height -1 && x == width -1 )
                {
                    apple.Number = 10 -(sum %10);
                }

                sum += apple.Number;

                appleList.Add(apple);
             }
        }

        Debug.Log($"AppleSpawner::SpawnApples():{sum}");
    }

    public void DestroyApple(Apple removeItem)
    {
        appleList.Remove(removeItem);
        Destroy(removeItem.gameObject);
    }
}
