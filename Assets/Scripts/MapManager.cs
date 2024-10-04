using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;// // 싱글톤 패턴으로 MapManager 인스턴스를 전역에서 접근할 수 있게 설정

    [Space(10f)]
    [Header("Map Lock")]
    public Text lock_map_gold_text; // cafe 해금에 필요한 골드를 나타냄
    public GameObject[] maplock_group; // 각 잠금 맵 오브젝트들을 나타냄
    public GameObject[] maplock_button; // 각 잠금 맵 버튼을 나타냄
    int cafe; // 각 잠금 맵의 호점을 나타내는 변수

    [Space(10f)]
    [Header("Cafe Management")]
    public GameObject cafe1; // 카페 1호점을 관리하는 오브젝트
    public GameObject cafe2; // 카페 2호점을 관리하는 오브젝트
    public bool cafe1_live; // 카페 1호점
    public bool cafe2_live; // 카페 2호점

    [Space(10f)]
    [Header("Camera")]
    public Vector3 targetPosition; // 카페 위치를 나타냄

    private void Awake()
    {
        instance = this; // 싱글톤 패턴 적용
        cafe1_live = false;
        cafe2_live = true;
    }

    public void Clickcafe1()
    {
        Checkcafe(0);

        //if (!cafe1_live)
        //{

        //    cafe1.gameObject.SetActive(true); //cafe1 입장
        //    cafe2.gameObject.SetActive(false); //cafe2 닫기
        //    cafe2_live = false;
        //    cafe1_live = true;

        //    //카메라의 위치를 1호점으로 이동
        //    targetPosition = new Vector3(20, 0, -10);
        //    Camera.main.transform.position = targetPosition;
        //}

    }

    public void Clickcafe2()
    {
        Checkcafe(1);
        //if (!cafe2_live)
        //{

        //    cafe2.gameObject.SetActive(true); //cafe2 입장
        //    cafe1.gameObject.SetActive(false); //cafe1 닫기
        //    cafe1_live = false;
        //    cafe2_live = true;

        //    //카메라의 위치를 2호점으로 이동
        //    targetPosition = new Vector3(0, 0, -10);
        //    Camera.main.transform.position = targetPosition;
        //}

    }

    public void Clickcafe3()
    {
        Checkcafe(2);
    }

    public void Clickcafe4()
    {
        Checkcafe(3);

    }

    public void Clickcafe5()
    {
        Checkcafe(4);
    }

    public void Checkcafe(int a)
    {
        GameManager.instance.lock_cafe_list = a; // 몇호점을 선택했는지 gamemanager로 보냄

        cafe = GameManager.instance.map_goldlist[a]; 
        lock_map_gold_text.text = cafe.ToString(); // 각 맵에 필요한 골드량을 text로 나타내기 위해 전달
    }
}
