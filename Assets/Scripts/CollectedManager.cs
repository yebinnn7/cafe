using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class CollectedManager : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager 오브젝트 참조
    public GameManager game_manager; // GameManager 스크립트 참조

    public bool[] collected_list;
    public Sprite[] customer_spritelist;
    public string[] customer_namelist;
    public int[] customer_favorability;
    public string[] collected_name;
    public Sprite[] collected_sprites;

    public Image lockGroupImage0;
    public Image lockGroupImage1;
    public Image lockGroupImage2;


    public Text pageText;
    int page;

    public Image SpecialSprite0;
    public Image SpecialSprite1;
    public Image SpecialSprite2;


    public Text SpecialName0;
    public Text SpecialName1;
    public Text SpecialName2;


    public Button SpecialButton0;
    public Button SpecialButton1;
    public Button SpecialButton2;


    public Image InformationImage;
    public Text InformationText;
    public Text InformationFavorability;

    public Image information_panel;
    Animator information_anim;

    public Text InformationMenuName;
    public Text InformationMenuDescription;
    public Image InformationMenuImage;

    public Image UnlockPanel;


    // Start is called before the first frame update
    void Awake()
    {
        // GameManager 오브젝트를 찾고, 해당 스크립트를 참조
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        collected_list = game_manager.collected_list;
        customer_spritelist = game_manager.special_customer_spritelist;
        customer_namelist = game_manager.special_customer_namelist;
        customer_favorability = game_manager.specialCustomerFavorability;
        collected_name = game_manager.collected_name;
        collected_sprites = game_manager.collected_sprites;

        // 첫 페이지로 초기화
        page = 0;

        information_anim = information_panel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
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
    }*/

    public void UpdateLockGroupImages()
    {
        for (int i = 0; i < 3; i++)
        {
            int index = page * 3 + i;

            if (index < collected_name.Length) // collected_name 배열 범위 내인지 확인
            {
                bool isUnlocked = !string.IsNullOrEmpty(collected_name[index]); // 이름이 있는지 확인
                switch (i)
                {
                    case 0:
                        lockGroupImage0.gameObject.SetActive(!isUnlocked);
                        break;
                    case 1:
                        lockGroupImage1.gameObject.SetActive(!isUnlocked);
                        break;
                    case 2:
                        lockGroupImage2.gameObject.SetActive(!isUnlocked);
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


    public void ChangePage()
    {
        int startIndex = page * 3;
        pageText.text = string.Format("#{0:00}", (page + 1));

        for (int i = 0; i < 3; i++)
        {
            if (startIndex + i < collected_name.Length)
            {
                Image[] specialSprites = { SpecialSprite0, SpecialSprite1, SpecialSprite2 };
                Text[] specialNames = { SpecialName0, SpecialName1, SpecialName2 };

                // 이름과 스프라이트 업데이트
                specialSprites[i].sprite = collected_sprites[startIndex + i];
                specialNames[i].text = collected_name[startIndex + i];
            }
        }

        UpdateLockGroupImages();
    }

    public void ClickSpecialBitton0()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 3;

        InformationImage.sprite = collected_sprites[index];
        InformationText.text = collected_name[index];
        InformationFavorability.text = "호감도 " + customer_favorability[index];

        InformationMenuName.text = game_manager.collected_menu_name[index];
        InformationMenuDescription.text = game_manager.collected_menu_description[index];
        InformationMenuImage.sprite = game_manager.collected_menu_image[index];

        game_manager.isInformationClick = true;

        if (game_manager.unlockMenu[index] == true)
        {
            UnlockPanel.gameObject.SetActive(false);
        }
        else
        {
            UnlockPanel.gameObject.SetActive(true);
        }

    }

    public void ClickSpecialBitton1()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 3 + 1;

        InformationImage.sprite = collected_sprites[index];
        InformationText.text = collected_name[index];
        InformationFavorability.text = "호감도 " + customer_favorability[index];

        InformationMenuName.text = game_manager.collected_menu_name[index];
        InformationMenuDescription.text = game_manager.collected_menu_description[index];
        InformationMenuImage.sprite = game_manager.collected_menu_image[index];

        game_manager.isInformationClick = true;

        if (game_manager.unlockMenu[index] == true)
        {
            UnlockPanel.gameObject.SetActive(false);
        }
        else
        {
            UnlockPanel.gameObject.SetActive(true);
        }
    }

    public void ClickSpecialBitton2()
    {
        information_anim.SetTrigger("doShow");

        int index = page * 3 + 2;

        InformationImage.sprite = collected_sprites[index];
        InformationText.text = collected_name[index];
        InformationFavorability.text = "호감도 " + customer_favorability[index];

        InformationMenuName.text = game_manager.collected_menu_name[index];
        InformationMenuDescription.text = game_manager.collected_menu_description[index];
        InformationMenuImage.sprite = game_manager.collected_menu_image[index];

        game_manager.isInformationClick = true;

        if (game_manager.unlockMenu[index] == true)
        {
            UnlockPanel.gameObject.SetActive(false);
        }
        else
        {
            UnlockPanel.gameObject.SetActive(true);
        }
    }


    public void ExitButton()
    {
        information_anim.SetTrigger("doHide");

        game_manager.isInformationClick = false;

    }
}




