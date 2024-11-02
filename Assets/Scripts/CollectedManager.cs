using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedManager : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager 오브젝트 참조
    public GameManager game_manager; // GameManager 스크립트 참조

    public bool[] collected_list;

    public Image lockGroupImage0;
    public Image lockGroupImage1;
    public Image lockGroupImage2;
    public Image lockGroupImage3;
    public Image lockGroupImage4;


    // Start is called before the first frame update
    void Awake()
    {
        // GameManager 오브젝트를 찾고, 해당 스크립트를 참조
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        collected_list = game_manager.collected_list;

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLockGroupImages()
    {
        lockGroupImage0.gameObject.SetActive(!collected_list[0]);
        lockGroupImage1.gameObject.SetActive(!collected_list[1]);
        lockGroupImage2.gameObject.SetActive(!collected_list[2]);
        lockGroupImage3.gameObject.SetActive(!collected_list[3]);
        lockGroupImage4.gameObject.SetActive(!collected_list[4]);
    }

    public void UpdateCollectedList(int index, bool value)
    { 
       
            collected_list[index] = value; // collected_list 값 업데이트
            UpdateLockGroupImages(); // UI 갱신 호출
        
    }
}
