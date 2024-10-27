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
    public GameObject cafe3; // 카페 3호점을 관리하는 오브젝트
    public GameObject cafe4; // 카페 4호점을 관리하는 오브젝트
    public GameObject cafe5; // 카페 5호점을 관리하는 오브젝트

    public bool cafe1_live; // 카페 1호점
    public bool cafe2_live; // 카페 2호점
    public bool cafe3_live; // 카페 3호점
    public bool cafe4_live; // 카페 4호점
    public bool cafe5_live; // 카페 5호점

    [Space(10f)]
    [Header("Camera")]
    public Vector3 targetPosition; // 카페 위치를 나타냄

    private void Awake()
    {
        instance = this; // 싱글톤 패턴 적용
    }

    public void Clickcafe1()
    {
        Checkcafe(0);
    }

    public void Clickcafe2()
    {
        Checkcafe(1);
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

        GameManager.instance.mapunlock_Button.gameObject.SetActive(true);

        cafe = GameManager.instance.map_goldlist[a]; 
        lock_map_gold_text.text = cafe.ToString(); // 각 맵에 필요한 골드량을 text로 나타내기 위해 전달
    }

    public void Movecafe1()
    {

        //카메라의 위치를 1호점으로 이동
        targetPosition = new Vector3(0, 0, -10);
        Camera.main.transform.position = targetPosition;
        
    }

    public void Movecafe2()
    {
        //카메라의 위치를 2호점으로 이동
        targetPosition = new Vector3(20, 0, -10);
        Camera.main.transform.position = targetPosition;
    }

    public void Movecafe3()
    {
        //카메라의 위치를 3호점으로 이동
        targetPosition = new Vector3(40, 0, -10);
        Camera.main.transform.position = targetPosition;
    }

    public void Movecafe4()
    {
        //카메라의 위치를 4호점으로 이동
        targetPosition = new Vector3(60, 0, -10);
        Camera.main.transform.position = targetPosition;
    }

    public void Movecafe5()
    {
        //카메라의 위치를 5호점으로 이동
        targetPosition = new Vector3(80, 0, -10);
        Camera.main.transform.position = targetPosition;
    }

}
