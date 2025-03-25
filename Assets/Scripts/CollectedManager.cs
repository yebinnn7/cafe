using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class CollectedManager : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager ������Ʈ ����
    public GameManager game_manager; // GameManager ��ũ��Ʈ ����

    public bool[] collected_list
    {
        get { return game_manager.collected_list; }
        set { game_manager.collected_list = value; }
    }
    public Sprite[] customer_spritelist
    {
        get { return game_manager.special_customer_spritelist; }
        set { game_manager.special_customer_spritelist = value; }
    }
    public string[] customer_namelist
    {
        get { return game_manager.special_customer_namelist; }
        set { game_manager.special_customer_namelist = value; }
    }
    public string[] customer_identifier;
    public int[] customer_favorability
    {
        get { return game_manager.specialCustomerFavorability; }
        set { game_manager.specialCustomerFavorability = value; }
    }
    public string[] collected_name
    {
        get { return game_manager.collected_name; }
        set { game_manager.collected_name = value; }
    }
    public List<string> collected_identifier = new List<string>();
    public Sprite[] collected_sprites
    {
        get { return game_manager.collected_sprites; }
        set { game_manager.collected_sprites = value; }
    }

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
        // GameManager ������Ʈ�� ã��, �ش� ��ũ��Ʈ�� ����
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        customer_identifier = new string[]
        {
            SpecialCustomerRegistry.DEAN_IDENTIFIER,
            SpecialCustomerRegistry.TEAMPLAY_MVP_IDENTIFIER,
            SpecialCustomerRegistry.BROKEN_GIRL_IDENTIFIER,
            SpecialCustomerRegistry.COUPLE_IDENTIFIER,
            SpecialCustomerRegistry.SOLO_EATER_IDENTIFIER,
            SpecialCustomerRegistry.FRESHMAN_IDENTIFIER,
            SpecialCustomerRegistry.RAIN_WETTED_IDENTIFIER,
            SpecialCustomerRegistry.F_GIVER_PROF_IDENTIFIER,
            SpecialCustomerRegistry.TEST_PREPARER_IDENTIFIER,
            SpecialCustomerRegistry.EARLY_BIRD_IDENTIFIER,
            SpecialCustomerRegistry.APPLY_FAILER_IDENTIFIER,
            SpecialCustomerRegistry.SENIOR_AND_JUNIOR_IDENTIFIER,
            SpecialCustomerRegistry.GRASS_ENJOYER_IDENTIFIER,
            SpecialCustomerRegistry.TOP_STUDENT_IDENTIFIER,
            SpecialCustomerRegistry.DEVELOPER_IDENTIFIER
        };  // Migrated from customer_namelist

        // ù �������� �ʱ�ȭ
        page = 0;

        information_anim = information_panel.GetComponent<Animator>();
    }

    void Start()
    {
        PullAndApplyPlayerDataModel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void UpdateLockGroupImages()
    {
        // �� �������� �´� �ε����� ������� ��� ���¸� ������Ʈ
        for (int i = 0; i < 5; i++)
        {
            int index = page * 5 + i; // ���� �������� �´� �ε��� ���
            if (index < collected_list.Length) // �ε����� collected_list�� ������ �ʰ����� �ʵ��� Ȯ��
            {
                // ��� ���� ������Ʈ
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

            if (index < collected_name.Length) // collected_name �迭 ���� ������ Ȯ��
            {
                bool isUnlocked = !string.IsNullOrEmpty(collected_name[index]); // �̸��� �ִ��� Ȯ��
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

        collected_list[index] = value; // collected_list �� ������Ʈ
        UpdateLockGroupImages(); // UI ���� ȣ��

    }

    // �������� �������� �̵�
    public void PageUp()
    {
        if (page >= 4) // �ִ� �������� ���� �ʵ��� ����
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        ++page;
        ChangePage(); // ������ ����
        SoundManager.instance.PlaySound("Button");
    }

    // �������� �������� �̵�
    public void PageDown()
    {
        if (page <= 0) // �ּ� �������� ���� �ʵ��� ����
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        --page;
        ChangePage(); // ������ ����
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

                // �̸��� ��������Ʈ ������Ʈ
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
        InformationFavorability.text = "ȣ���� " + customer_favorability[index];

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
        InformationFavorability.text = "ȣ���� " + customer_favorability[index];

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
        InformationFavorability.text = "ȣ���� " + customer_favorability[index];

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

    public void PullAndApplyPlayerDataModel()
    {
        SpecialCustomerModel curr;
        int collected_index = 0;

        foreach (string each in game_manager.customerUnlocked)
        {
            try
            {
                curr = SpecialCustomerRegistry.Get(each);
            }
            catch (KeyNotFoundException e)
            {
                if (string.IsNullOrEmpty(each))
                {
                    continue;
                }
                else
                {
                    throw e;
                }
            }
            collected_identifier[collected_index] = curr.identifier;
            collected_sprites[collected_index] = Sprite.Create(
                curr.Sprite.frontSprite,
                new Rect(0, 0, curr.Sprite.frontSprite.width, curr.Sprite.frontSprite.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            collected_name[collected_index] = curr.DisplayName;
            collected_index++;
        }
    }
}
