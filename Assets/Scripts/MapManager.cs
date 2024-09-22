using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;// // 싱글톤 패턴으로 MapManager 인스턴스를 전역에서 접근할 수 있게 설정

    public GameObject cafe1; // 카페 1호점을 관리하는 오브젝트
    public GameObject cafe2; // 카페 2호점을 관리하는 오브젝트
    public bool cafe1_live; // 카페 1호점
    public bool cafe2_live; // 카페 2호점

    public Text cafe_text;

    private void Awake()
    {
        instance = this; // 싱글톤 패턴 적용
        cafe1_live = false;
        cafe2_live = true;
    }

    public void Clickcafe2()
    {
        if (cafe2_live)
        {
            cafe1.gameObject.SetActive(true); //cafe1 입장
            cafe2.gameObject.SetActive(false); //cafe2 닫기
            cafe1_live = true;
            cafe2_live = false;
            cafe_text.text = "1호점";
        }
        else
        {
            cafe2.gameObject.SetActive(true); //cafe2 입장
            cafe1.gameObject.SetActive(false); //cafe1 닫기
            cafe1_live = false;
            cafe2_live = true;
            cafe_text.text = "2호점";
        }
        
    }

}
