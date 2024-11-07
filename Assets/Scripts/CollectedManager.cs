using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedManager : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager 오브젝트 참조
    public GameManager game_manager; // GameManager 스크립트 참조

    public bool[] collected_list;
    public Sprite[] customer_spritelist;
    public string[] customer_namelist;

    public Image lockGroupImage0;
    public Image lockGroupImage1;
    public Image lockGroupImage2;
    public Image lockGroupImage3;
    public Image lockGroupImage4;

    public Text pageText;
    int page;

    public Image SpecialSprite0;
    public Image SpecialSprite1;
    public Image SpecialSprite2;
    public Image SpecialSprite3;
    public Image SpecialSprite4;

    public Text SpecialName0;
    public Text SpecialName1;
    public Text SpecialName2;
    public Text SpecialName3;
    public Text SpecialName4;

    public Button SpecialButton0;
    public Button SpecialButton1;
    public Button SpecialButton2;
    public Button SpecialButton3;
    public Button SpecialButton4;

    public Image InformationImage;
    public Text InformationText;

    public Image information_panel; 
    Animator information_anim;

    // Start is called before the first frame update
    void Awake()
    {
        // GameManager 오브젝트를 찾고, 해당 스크립트를 참조
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        collected_list = game_manager.collected_list;
        customer_spritelist = game_manager.special_customer_spritelist;
        customer_namelist = game_manager.special_customer_namelist;

        // 첫 페이지로 초기화
        page = 0;

        information_anim = information_panel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLockGroupImages()
    {
        // 각 페이지에 맞는 인덱스를 기반으로 잠금 상태를 업데이트
        for (int i = 0; i < 5; i++)
        {
            int index = page * 5 + i; // 현재 페이지에 맞는 인덱스 계산
            if (index < collected_list.Length) // 인덱스가 collected_list의 범위를 초과하지 않도록 확인
            {
                // 잠금 상태 업데이트
                switch (i)
                {
                    case 0:
                        lockGroupImage0.gameObject.SetActive(!collected_list[index]);
                        break;
                    case 1:
                        lockGroupImage1.gameObject.SetActive(!collected_list[index]);
                        break;
                    case 2:
                        lockGroupImage2.gameObject.SetActive(!collected_list[index]);
                        break;
                    case 3:
                        lockGroupImage3.gameObject.SetActive(!collected_list[index]);
                        break;
                    case 4:
                        lockGroupImage4.gameObject.SetActive(!collected_list[index]);
                        break;
                }
            }
        }
    }


    public void UpdateCollectedList(int index, bool value)
    { 
       
            collected_list[index] = value; // collected_list 값 업데이트
            UpdateLockGroupImages(); // UI 갱신 호출
        
    }

    // 페이지를 다음으로 이동
    public void PageUp()
    {
        if (page >= 4) // 최대 페이지를 넘지 않도록 제한
        {
           SoundManager.instance.PlaySound("Fail");
            return;
        }

        ++page;
        ChangePage(); // 페이지 변경
        SoundManager.instance.PlaySound("Button");
    }

    // 페이지를 이전으로 이동
    public void PageDown()
    {
        if (page <= 0) // 최소 페이지를 넘지 않도록 제한
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        --page;
        ChangePage(); // 페이지 변경
        SoundManager.instance.PlaySound("Button");
    }

    void ChangePage()
    {
        int startIndex = page * 5; // 각 페이지에 대해 시작 인덱스 계산
        pageText.text = string.Format("#{0:00}", (page + 1)); // 페이지 번호 표시

        for (int i = 0; i < 5; i++)
        {
            if (startIndex + i < customer_spritelist.Length) // 인덱스 범위 확인
            {
                // SpecialSprite 배열 생성
                Image[] specialSprites = { SpecialSprite0, SpecialSprite1, SpecialSprite2, SpecialSprite3, SpecialSprite4 };
                specialSprites[i].sprite = customer_spritelist[startIndex + i];
            }
        }


        for (int i = 0; i < 5; i++)
        {
            if (startIndex + i < customer_namelist.Length) // 인덱스 범위 확인
            {
                Text[] specialNames = { SpecialName0, SpecialName1, SpecialName2, SpecialName3, SpecialName4 };
                specialNames[i].text = customer_namelist[startIndex + i].ToString(); // text 속성에 할당
            }
        }



        // 페이지가 변경될 때 잠금 상태 업데이트
        UpdateLockGroupImages(); // UI 갱신 호출
    }

    public void ClickSpecialBitton0()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 5;

        InformationImage.sprite = customer_spritelist[index];
        InformationText.text = customer_namelist[index];

        game_manager.isInformationClick = true;



    }

    public void ClickSpecialBitton1()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 5 + 1;

        InformationImage.sprite = customer_spritelist[index];
        InformationText.text = customer_namelist[index];

        game_manager.isInformationClick = true;
    }

    public void ClickSpecialBitton2()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 5 + 2;

        InformationImage.sprite = customer_spritelist[index];
        InformationText.text = customer_namelist[index];

        game_manager.isInformationClick = true;
    }

    public void ClickSpecialBitton3()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 5 + 3;

        InformationImage.sprite = customer_spritelist[index];
        InformationText.text = customer_namelist[index];

        game_manager.isInformationClick = true;
    }

    public void ClickSpecialBitton4()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 5 + 4;

        InformationImage.sprite = customer_spritelist[index];
        InformationText.text = customer_namelist[index];

        game_manager.isInformationClick = true;
    }

    public void ExitButton()
    {
        information_anim.SetTrigger("doHide");

        game_manager.isInformationClick = false;
    }
}

