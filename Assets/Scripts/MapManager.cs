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

    public Vector3 targetPosition; // 카페 위치를 나타냄

    private void Awake()
    {
        instance = this; // 싱글톤 패턴 적용
        cafe1_live = false;
        cafe2_live = true;
    }

    public void Clickcafe1()
    {
        if (!cafe1_live)
        {

            cafe1.gameObject.SetActive(true); //cafe1 입장
            cafe2.gameObject.SetActive(false); //cafe2 닫기
            cafe2_live = false;
            cafe1_live = true;

            //카메라의 위치를 1호점으로 이동
            targetPosition = new Vector3(20, 0, -10);
            Camera.main.transform.position = targetPosition;
        }

    }

    public void Clickcafe2()
    {
        if (!cafe2_live)
        {
            
            cafe2.gameObject.SetActive(true); //cafe2 입장
            cafe1.gameObject.SetActive(false); //cafe1 닫기
            cafe1_live = false;
            cafe2_live = true;

            //카메라의 위치를 2호점으로 이동
            targetPosition = new Vector3(0, 0, -10);
            Camera.main.transform.position = targetPosition;
        }
        
    }

}
