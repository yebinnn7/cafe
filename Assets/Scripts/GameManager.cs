using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; // ��ȣ�� ���� ������

public class GameManager : MonoBehaviour
{
    public static GameManager instance;// // �̱��� �������� GameManager �ν��Ͻ��� �������� ������ �� �ְ� ����

    [Header("Money")]
    public int jelatin; // ����ƾ �ڿ�
    public int gold; // ��� �ڿ�

    [Space(10f)]
    [Header("List")]
    public List<Jelly> jelly_list = new List<Jelly>(); // ������ �������� ������ ����Ʈ
    public List<Data> jelly_data_list = new List<Data>(); // ����� ������ �����͸� ������ ����Ʈ
    public bool[] jelly_unlock_list; // ���� ��� ���� ���¸� ������ �迭
    public List<SpecialCustomer> special_customer_list = new List<SpecialCustomer>();
    public List<Data> special_customer_data_list = new List<Data>();

    public int max_jelatin; // ����ƾ�� �ִ�ġ
    public int max_gold; // ����� �ִ�ġ

    [Space(10f)]
    [Header("Game On Off")]
    public bool isSell; // ������ �Ǹ��� �� �ִ� �������� ����
    public bool isLive; // ������ Ȱ��ȭ�� �������� ����

    [Space(10f)]
    [Header("Store Jelly")]
    public Sprite[] jelly_spritelist; // ������ ��������Ʈ ����Ʈ
    public string[] jelly_namelist; // ���� �̸� ����Ʈ
    public int[] jelly_jelatinlist; // ���� ��� ������ �ʿ��� ����ƾ ����Ʈ
    public int[] jelly_goldlist; // ���� ���ſ� �ʿ��� ��� ����Ʈ

    [Space(10f)]
    [Header("Map")]
    public int[] map_goldlist; // �� ���ſ� �ʿ��� ��带 ��Ÿ��
    public int lock_cafe_list; // ��� ī�䰡 ��ȣ������ ��Ÿ��

    [Space(10f)]
    [Header("Store Jelly(Unlock)")]
    public Text page_text; // �������� ǥ���ϴ� �ؽ�Ʈ UI
    public Image unlock_group_jelly_img; // ��� ������ ������ �̹����� ǥ���� UI
    public Text unlock_group_gold_text; // ��� ������ ������ ���� ����� ǥ���� �ؽ�Ʈ
    public Text unlock_group_name_text; // ��� ������ ������ �̸��� ǥ���� �ؽ�Ʈ UI

    [Space(10f)]
    [Header("Store Jelly(Lock)")]
    public GameObject lock_group; // ��ݵ� ���� �׷��� �����ϴ� ������Ʈ
    public Image lock_group_jelly_img; // ��ݵ� ������ �̹����� ǥ���� UI
    public Text lock_group_jelatin_text; // ��� ������ �ʿ��� ����ƾ ������ ǥ���� �ؽ�Ʈ UI

    // Animator ���� ������ ���� Animator �迭
    [Space(10f)]
    [Header("Animation")]
    public RuntimeAnimatorController[] level_ac; // ���� ������ ���� �ִϸ����� ��Ʈ�ѷ� ����Ʈ

    [Space(10f)]
    [Header("Canvers")]
    public Text jelatin_text; // ����ƾ �ڿ� ������ ǥ���� �ؽ�Ʈ UI
    public Text gold_text; // ��� �ڿ� ������ ǥ���� �ؽ�Ʈ UI


    public Image jelly_panel; // ���� �޴� �г�
    public Image plant_panel; // �÷�Ʈ �޴� �г�
    public Image option_panel; // �ɼ� �޴� �г�
    public Image map_panel; // �� �޴� �г�
    public Image random_panel; // ���� �޴� �г�
    public Image collected_panel; // ���� �޴� �г�
    public Image information_panel; // ���� �޴� �г�


    [Space(10f)]
    [Header("Prefabs")]
    public GameObject prefab; // ���� ������
    public GameObject prefab_special_customer; // �ܰ�մ� ������
    public GameObject favorability_effect_prefab; // ȣ���� ����Ʈ ������

    [Space(10f)]
    [Header("Data")]
    public GameObject data_manager_obj; // DataManager ������Ʈ
    DataManager data_manager; // DataManager ��ũ��Ʈ

    [Space(10f)]
    [Header("Animation")]
    Animator jelly_anim; // ���� �г� �ִϸ��̼� ����
    Animator plant_anim; // �÷�Ʈ �г� �ִϸ��̼� ����
    Animator map_anim; // �� �г� �ִϸ��̼� ����
    Animator random_anim; // ���� �г� �ִϸ��̼� ����
    Animator collected_anim; // ���� �г� �ִϸ��̼� ����
    Animator information_anim; // ���� �г� �ִϸ��̼� ����

    [Space(10f)]
    [Header("Click On Off")]
    bool isJellyClick; // ���� ��ư�� Ŭ���� �������� ����
    bool isPlantClick; // �÷�Ʈ ��ư�� Ŭ���� �������� ����
    bool isOption; // �ɼ� �г��� Ȱ��ȭ�� �������� ����
    bool isMapClick; // �� ��ư�� Ŭ���� �������� ����
    bool isRandomClick; // ���� ��ư�� Ŭ���� �������� ����
    bool isCollectedClick; // ���� ��ư�� Ŭ���� �������� ����
    public bool isInformationClick;

    int page; // ���� ���õ� ������
    int index; // �ܰ� �մ� ��ȣ

    [Space(10f)]
    [Header("Upgrade")]
    // ���׷��̵� �ý��� ����
    public int num_level;
    public Text num_sub_text;
    public Text num_btn_text;
    public Button num_btn;
    public int[] num_goldlist;

    public Button mapunlock_Button;

    [Space(10f)]
    [Header("Click List")]
    public int click_level;
    public Text click_sub_text;
    public Text click_btn_text;
    public Button click_btn;
    public int[] click_goldlist;

    // ���� �����ð�
    [Space(10f)]
    [Header("SpawnTime")]
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 8f;

    [Space(10f)]
    [Header("Regular customer")]
    // �ܰ�մ� �̱� ����
    public int special_customer_gold;

    // �ܰ�մ� ����Ʈ
    public Sprite[] special_customer_spritelist; // ������ ��������Ʈ ����Ʈ
    public string[] special_customer_namelist; // ���� �̸� ����Ʈ
    public bool[] collected_list;
    public string[] collected_name;
    public Sprite[] collected_sprites;

    // �ܰ�մ� �����ð�
    public float minSpecialSpawnTime = 5f;
    public float maxSpecialSpawnTime = 8f;

    // �ܰ�մ� ȣ���� ���� ��ųʸ�
    public int[] specialCustomerFavorability;

    // Collected Manager ����
    private CollectedManager collectedManager;

    // Map���� ���� ��ġ �迭 ����
    Vector3[] spawnPos;

    // ��� ����
    public int[] machine_level;
    public Text machine_sub_text;
    public Text machine_btn_text;
    public Button machine_btn;
    public Text map_text;


    int machinePage;


    // �� ����� ������ �ݾ�
    public int[] machine_goldlist1 = new int[5];  // ��� 1�� �ݾ� (���� 1~5)
    public int[] machine_goldlist2 = new int[5];  // ��� 2�� �ݾ� (���� 1~5)
    public int[] machine_goldlist3 = new int[5];  // ��� 3�� �ݾ� (���� 1~5)
    public int[] machine_goldlist4 = new int[5];  // ��� 4�� �ݾ� (���� 1~5)
    public int[] machine_goldlist5 = new int[5];  // ��� 5�� �ݾ� (���� 1~5)

    // �� ����� �̹��� (1~5)
    public GameObject[] machine_list1 = new GameObject[6];  // ��� 1�� �̹��� (���� 1~5)
    public GameObject[] machine_list2 = new GameObject[6];  // ��� 2�� �̹��� (���� 1~5)
    public GameObject[] machine_list3 = new GameObject[6];  // ��� 3�� �̹��� (���� 1~5)
    public GameObject[] machine_list4 = new GameObject[6];  // ��� 4�� �̹��� (���� 1~5)
    public GameObject[] machine_list5 = new GameObject[6];  // ��� 5�� �̹��� (���� 1~5)

    // Map�� �մ� ����Ʈ
    public List<Jelly> map1JellyList = new List<Jelly>();
    public List<SpecialCustomer> map1specialCustomerList = new List<SpecialCustomer>(); // 1�� ��

    public List<Jelly> map2JellyList = new List<Jelly>();
    public List<SpecialCustomer> map2specialCustomerList = new List<SpecialCustomer>(); // 2�� ��

    public List<Jelly> map3JellyList = new List<Jelly>();
    public List<SpecialCustomer> map3specialCustomerList = new List<SpecialCustomer>(); // 3�� ��

    public List<Jelly> map4JellyList = new List<Jelly>();
    public List<SpecialCustomer> map4specialCustomerList = new List<SpecialCustomer>(); // 4�� ��

    public List<Jelly> map5JellyList = new List<Jelly>();
    public List<SpecialCustomer> map5specialCustomerList = new List<SpecialCustomer>(); // 5�� ��


    int specialNum = 0;
    



    void Awake()
    {
        instance = this; // �̱��� ���� ����

        // �г� �ִϸ����� �ʱ�ȭ
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        map_anim = map_panel.GetComponent<Animator>();
        random_anim = random_panel.GetComponent<Animator>();
        collected_anim = collected_panel.GetComponent<Animator>();
        information_anim = information_panel.GetComponent<Animator>();

        isLive = true; // ���� Ȱ��ȭ ���·� ����

        // UI �ʱ�ȭ
        jelatin_text.text = jelatin.ToString(); // int���� string������ ��ȯ
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString();

        // DataManager �ʱ�ȭ
        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0; // ù �������� �ʱ�ȭ
        jelly_unlock_list = new bool[12]; // ���� ��� ���� �迭 �ʱ�ȭ (12���� ����)

        collectedManager = FindObjectOfType<CollectedManager>();

        spawnPos = new Vector3[]
        {
             new Vector3(Random.Range(-1.25f, 1.25f), 1.3f, 0),
             new Vector3(Random.Range(18.75f, 21.25f), 1.3f, 0),
            new Vector3(Random.Range(38.75f, 41.25f), 1.3f, 0),
            new Vector3(Random.Range(58.75f, 61.25f), 1.3f, 0),
            new Vector3(Random.Range(78.75f, 81.25f), 1.3f, 0),

         };
    }

    void Start()
    {
        // �����͸� �ҷ����� ���� ȣ��, ���� �ε�� ���� ȣ��ǹǷ� �ణ�� ���� �� ����
        // Invoke("LoadData", 0.1f);
        // StartCoroutine(SpawnJellyRandomly());
        // StartCoroutine(SpawnSpecialRandomly());

        StartCoroutine(SpawnJellyOnMap1());
        StartCoroutine(SpawnJellyOnMap2());
        StartCoroutine(SpawnJellyOnMap3());
        StartCoroutine(SpawnJellyOnMap4());
        StartCoroutine(SpawnJellyOnMap5());

        // �� �ʺ��� �ܰ�մ� ���� �ڷ�ƾ�� ����
        StartCoroutine(SpawnSpecialOnMap1()); // 1�� ��
        StartCoroutine(SpawnSpecialOnMap2()); // 2�� ��
        StartCoroutine(SpawnSpecialOnMap3()); // 3�� ��
        StartCoroutine(SpawnSpecialOnMap4()); // 4�� ��
        StartCoroutine(SpawnSpecialOnMap5()); // 5�� ��

        //
        LoadAndPullPlayerData();
    }

    void Update()
    {
        // '���' ��ư�� ������ �� ó��
        if (Input.GetButtonDown("Cancel"))
        {
            if (isJellyClick) ClickJellyBtn(); // ���� �޴��� ���� ������ ����
            else if (isPlantClick) ClickPlantBtn(); // �÷�Ʈ �޴��� ���� ������ ����
            else if (isMapClick) ClickMapBtn(); // �÷�Ʈ �޴��� ���� ������ ����
            else if (isRandomClick) ClickRandomBtn(); // ���� �޴��� ���� ������ ����
            else if (isCollectedClick) ClickCollectedBtn(); // ���� �޴��� ���� ������ ����
            else if (isInformationClick) collectedManager.ExitButton(); // ���� �޴��� ���� ������ ����
            else Option(); // �ɼ� �޴��� ���ų� ����
        }

    }

    void LateUpdate()
    {
        // �ε巴�� �ؽ�Ʈ ���� ������Ʈ�ϸ� �ε��Ҽ��� ��� ������ �ּ�ȭ�ϱ� ���� �ݿø� ����
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }

    // ���� ������ ���� Animator Controller�� ����
    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    // ����ƾ ȹ��
    public void GetJelatin(int id, int level)
    {
        jelatin += (id + 1) * level * click_level; // id�� ������ ���� ����ƾ ����

        if (jelatin > max_jelatin) // ����ƾ�� �ִ�ġ�� ���� �ʵ��� ����
            jelatin = max_jelatin;
    }

    // ��� ȹ�� �� ���� ����
    public void GetGold(int id, int level, Jelly jelly)
    {
        gold += jelly_goldlist[id] * level; // ��� �߰�

        if (gold > max_gold) // ��尡 �ִ�ġ�� ���� �ʵ��� ����
            gold = max_gold;

        jelly_list.Remove(jelly); // ���� ����Ʈ���� ����

        SoundManager.instance.PlaySound("Sell");
    }

    // �Ǹ� ���� üũ
    public void CheckSell()
    {
        isSell = !isSell; // �Ǹ� ���¸� ���
    }

    // ���� �޴� ��ư Ŭ�� ó��
    public void ClickJellyBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isPlantClick) // �÷�Ʈ �޴��� ���� ������ ����
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // �� �޴��� ���� ������ ����
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // ���� �޴��� ���� ������ ����
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // ���� �޴��� ���� ������ ����
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // ���� �޴��� ���� ������ ����
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }


        if (isJellyClick) // ���� �޴��� ���� ������ ����
            jelly_anim.SetTrigger("doHide");
        else // ���� �޴��� ���� ������ ����
            jelly_anim.SetTrigger("doShow");

        isJellyClick = !isJellyClick; // ���� Ŭ�� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���
    }

    // �÷�Ʈ �޴� ��ư Ŭ�� ó��
    public void ClickPlantBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isJellyClick) // ���� �޴��� ���� ������ ����
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isMapClick) // �� �޴��� ���� ������ ����
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // ���� �޴��� ���� ������ ����
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // ���� �޴��� ���� ������ ����
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // ���� �޴��� ���� ������ ����
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isPlantClick) // �÷�Ʈ �޴��� ���� ������ ����
            plant_anim.SetTrigger("doHide");
        else // �÷�Ʈ �޴��� ���� ������ ����
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick; // �÷�Ʈ Ŭ�� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���
    }

    public void ClickMapBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isJellyClick) // ���� �޴��� ���� ������ ����
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick) // �÷�Ʈ �޴��� ���� ������ ����
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isRandomClick) // ���� �޴��� ���� ������ ����
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // ���� �޴��� ���� ������ ����
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // ���� �޴��� ���� ������ ����
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isMapClick) // �� �޴��� ���� ������ ����
            map_anim.SetTrigger("doHide");
        else // �� �޴��� ���� ������ ����
            map_anim.SetTrigger("doShow");

        isMapClick = !isMapClick; // �� Ŭ�� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���
    }

    public void ClickMapExitBtn()
    {
        map_anim.SetTrigger("doHide");

        isMapClick = false;
        isLive = true;
    }

    public void ClickRandomBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isJellyClick) // ���� �޴��� ���� ������ ����
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick) // �÷�Ʈ �޴��� ���� ������ ����
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // �� �޴��� ���� ������ ����
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isCollectedClick) // ���� �޴��� ���� ������ ����
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // ���� �޴��� ���� ������ ����
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isRandomClick) // ���� �޴��� ���� ������ ����
            random_anim.SetTrigger("doHide");
        else // ���� �޴��� ���� ������ ����
            random_anim.SetTrigger("doShow");


        isRandomClick = !isRandomClick; // �� Ŭ�� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���
    }

    public void ClickRandomExitBtn()
    {
        random_anim.SetTrigger("doHide");

        isRandomClick = false;
        isLive = true;
    }



    public void ClickCollectedBtn()
    {
        SoundManager.instance.PlaySound("Button");
        collectedManager.ChangePage();

        if (isJellyClick) // ���� �޴��� ���� ������ ����
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
            
        }

        if (isPlantClick) // �÷�Ʈ �޴��� ���� ������ ����
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // �� �޴��� ���� ������ ����
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // ���� �޴��� ���� ������ ����
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isInformationClick) // ���� �޴��� ���� ������ ����
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isCollectedClick) // ���� �޴��� ���� ������ ����
            collected_anim.SetTrigger("doHide");
        else // ���� �޴��� ���� ������ ����
            collected_anim.SetTrigger("doShow");



        isCollectedClick = !isCollectedClick; // �� Ŭ�� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���
    }

    public void ClickCollectedExitBtn()
    {
        collected_anim.SetTrigger("doHide");

        isCollectedClick = false;
        isLive = true;
    }

    // �ɼ� �г� ���� �ݱ�
    public void Option()
    {
        isOption = !isOption; // �ɼ� �г� ���¸� ���
        isLive = !isLive; // ���� Ȱ��ȭ ���¸� ���

        option_panel.gameObject.SetActive(isOption); // �ɼ� �г� ǥ��/�����
        Time.timeScale = isOption == true ? 0 : 1; // �ɼ� �г��� ������ �ð� ����

        if (isOption) SoundManager.instance.PlaySound("Pause In");
        else SoundManager.instance.PlaySound("Pause Out");
    }

    // �������� �������� �̵�
    public void PageUp()
    {
        if (page >= 11) // �ִ� �������� ���� �ʵ��� ����
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

    // ������ ���濡 ���� UI ������Ʈ
    void ChangePage()
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // ���� �������� ������ ��� �������� ���ο� ���� ��� �׷� ǥ��

        page_text.text = string.Format("#{0:00}", (page + 1)); // ������ ��ȣ ǥ��

        if (lock_group.activeSelf) // ��� ������ ��� ��� �׷� UI ������Ʈ
        {
            lock_group_jelly_img.sprite = jelly_spritelist[page]; // ���� �������� �ش��ϴ� ���� �̹����� ��� �׷쿡 ����
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]); // �ش� ������ ������ �ʿ��� ����ƾ ������ �ؽ�Ʈ�� ǥ��

            lock_group_jelly_img.SetNativeSize(); // �̹����� ũ�⸦ ���� ũ��� ����
        }
        else // ��� ���� ������ ��� ��� ���� �׷� UI ������Ʈ
        {
            unlock_group_jelly_img.sprite = jelly_spritelist[page]; // ���� �������� �ش��ϴ� ���� �̹����� ��� ���� �׷쿡 ����
            // ������ �̸��� ������ �ؽ�Ʈ�� ǥ��
            unlock_group_name_text.text = jelly_namelist[page];
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);

            unlock_group_jelly_img.SetNativeSize(); // �̹����� ũ�⸦ ���� ũ��� ����
        }
    }

    // ���� ��� ���� �Լ�
    public void Unlock()
    {
        // ���� ����ƾ�� ��� ������ �ʿ��� ����ƾ ������ ������ �Լ� ����
        if (jelatin < jelly_jelatinlist[page])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        jelly_unlock_list[page] = true; // ���� ��� ���� ���·� ����
        ChangePage(); // ������ UI ������Ʈ

        jelatin -= jelly_jelatinlist[page]; // ����ƾ ���� ����

        SoundManager.instance.PlaySound("Unlock");
    }

    public void MapChange()
    {
        MapManager.instance.maplock_group[lock_cafe_list].gameObject.SetActive(false); //�� ��� ����
        MapManager.instance.maplock_button[lock_cafe_list].gameObject.SetActive(false); //�� ��� ��ư ����
    }

    //�� ��� ���� �Լ�
    public void MapUnlock()
    {
        // ���� ��尡 ��� ������ �ʿ��� ��庸�� ������ �Լ� ����
        if (gold < map_goldlist[lock_cafe_list])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        MapChange(); // ������ UI ������Ʈ

        gold -= map_goldlist[lock_cafe_list]; //��� ����

        SoundManager.instance.PlaySound("Unlock");

        mapunlock_Button.gameObject.SetActive(false);
    }

    // ���� ���� �Լ�
    public void BuyJelly()
    {
        // ���� ��尡 ���� ���ſ� �ʿ��� ��庸�� ������ �Լ� ����
        if (gold < jelly_goldlist[page] || jelly_list.Count >= num_level * 2)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= jelly_goldlist[page]; // ��� ����

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity); // ���� ������ ����
        Jelly jelly = obj.GetComponent<Jelly>(); // ������ ���� ������Ʈ�� Jelly ��ũ��Ʈ�� ������
        obj.name = "Jelly " + page; // ���� ������Ʈ�� �̸��� ���� ������ ��ȣ�� ����
        jelly.id = page; // ������ ID�� ���� �������� ����
        jelly.sprite_renderer.sprite = jelly_spritelist[page]; // ������ ��������Ʈ �̹����� ���� �������� �ش��ϴ� �̹����� ����

        jelly_list.Add(jelly); // ������ ���� ����Ʈ�� �߰�

        SoundManager.instance.PlaySound("Buy");
    }

    // ���� �����͸� �ҷ����� �Լ�
    void LoadData()
    {
        // ���� �������� ���� ��� ���¿� ���� ��� �׷� ǥ��
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        // ����� ���� �����͸� �ҷ��ͼ� ���ӿ� �ݿ�
        for (int i = 0; i < jelly_data_list.Count; ++i)
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity); // ���� ������ ����, ����� ��ġ(pos)�� ����
            Jelly jelly = obj.GetComponent<Jelly>(); // ������ ���� ������Ʈ�� Jelly ��ũ��Ʈ�� ������
            // ������ ID, ����, ����ġ�� ����� �����ͷ� ����
            jelly.id = jelly_data_list[i].id;
            jelly.level = jelly_data_list[i].level;
            jelly.exp = jelly_data_list[i].exp;
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id]; // ������ ��������Ʈ �̹����� ����� ID�� �ش��ϴ� �̹����� ����
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1]; // ������ �ִϸ��̼� ��Ʈ�ѷ��� ������ �°� ����
            obj.name = "Jelly " + jelly.id; // ���� ������Ʈ�� �̸��� Jelly + ID�� ����

            jelly_list.Add(jelly); // ������ ���� ����Ʈ�� �߰�

            num_sub_text.text = "���� ���뷮 " + num_level * 2;
            if (num_level >= 5) num_btn.gameObject.SetActive(false);
            else num_btn_text.text = string.Format("{0:n0}", num_goldlist[num_level]);

            click_sub_text.text = "Ŭ�� ���귮 X " + click_level;
            if (click_level >= 5) click_btn.gameObject.SetActive(false);
            else click_btn_text.text = string.Format("{0:n0}", click_goldlist[click_level]);
        }
    }

    // ���ø����̼� ���� �� �����͸� �����ϴ� �Լ�
    public void Exit()
    {
        // ������ �Ŵ����� ���� ���� �����͸� JSON �������� ����
        data_manager.JsonSave();
        // PushAndSavePlayerData();

        SoundManager.instance.PlaySound("Pause Out");

        Application.Quit();
    }

    // ���� ���뷮 ���׷��̵� �Լ�
    public void NumUpgrade()
    {
        if (gold < num_goldlist[num_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= num_goldlist[num_level++];

        num_sub_text.text = "���� ���뷮 " + num_level * 2;

        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_goldlist[num_level]);

        SoundManager.instance.PlaySound("Unlock");
        // SoundManager.instance.PlaySound("Buy");

    }

    // Ŭ�� ���귮 ���׷��̵� �Լ�
    public void ClickUpgrade()
    {
        if (gold < click_goldlist[click_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= click_goldlist[click_level++];

        click_sub_text.text = "Ŭ�� ���귮 X " + click_level * 2;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_goldlist[click_level]);

        SoundManager.instance.PlaySound("Unlock");
        // SoundManager.instance.PlaySound("Buy");
    }

    public void MachinePageUp()
    {
        UnityEngine.Debug.Log("������ up");

        if (machinePage >= 4) // �ִ� �������� ���� �ʵ��� ����
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        ++machinePage;
        ChangeMachinePage(); // ������ ����
        SoundManager.instance.PlaySound("Button");

    }

    public void MachinePageDown()
    {
        if (machinePage <= 0) // �ּ� �������� ���� �ʵ��� ����
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        --machinePage;
        ChangeMachinePage(); // ������ ����
        SoundManager.instance.PlaySound("Button");
    }

    public void ChangeMachinePage()
    {
        MachineUIUpdate();
        
    }

    public void MachineUIUpdate()
    {
        int mapIndex = machinePage; // ���� �������� �� �ε����� ���
        int currentMachineGold = 0;

        // mapIndex�� �ش��ϴ� ��� ���� ��������
        switch (mapIndex)
        {
            case 0:
                currentMachineGold = machine_goldlist1[machine_level[mapIndex]];
                break;
            case 1:
                currentMachineGold = machine_goldlist2[machine_level[mapIndex]];
                break;
            case 2:
                currentMachineGold = machine_goldlist3[machine_level[mapIndex]];
                break;
            case 3:
                currentMachineGold = machine_goldlist4[machine_level[mapIndex]];
                break;
            case 4:
                currentMachineGold = machine_goldlist5[machine_level[mapIndex]];
                break;
        }

        // ��� ���� ������Ʈ
        machine_sub_text.text = "보유 기계: " + machine_level[mapIndex] + "개";
        if (machine_level[mapIndex] >= 5)
        {
            machine_btn_text.text = "최대 보유";
            machine_btn.interactable = false;
        }
        else
        {
            machine_btn_text.text = string.Format("{0:n0}", currentMachineGold);
            machine_btn.interactable = true;
        }

        // �� �ε����� UI�� �ݿ�
        map_text.text = "지점: " + (mapIndex + 1);
    }

    public void BuyMachine()
    {
        // ��� �ݾ��� ����� ������ �°� ������
        int currentMachineGold = 0;
        GameObject[] currentMachineList = null;

        // �� ��迡 ���� ������ �´� �ݾװ� �̹����� ������
        switch (machinePage)
        {
            case 0:
                currentMachineGold = machine_goldlist1[machine_level[machinePage]];
                currentMachineList = machine_list1;
                break;
            case 1:
                currentMachineGold = machine_goldlist2[machine_level[machinePage]];
                currentMachineList = machine_list2;
                break;
            case 2:
                currentMachineGold = machine_goldlist3[machine_level[machinePage]];
                currentMachineList = machine_list3;
                break;
            case 3:
                currentMachineGold = machine_goldlist4[machine_level[machinePage]];
                currentMachineList = machine_list4;
                break;
            case 4:
                currentMachineGold = machine_goldlist5[machine_level[machinePage]];
                currentMachineList = machine_list5;
                break;
        }


        // �ݾ� ��
        if (gold < currentMachineGold)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        // ��� Ȱ��ȭ
        currentMachineList[machine_level[machinePage]].SetActive(true);
        // 5번째 원소도 활성화
        if (machine_level[machinePage] == 4)
        {
            currentMachineList[machine_level[machinePage] + 1].SetActive(true);
            
        }


        // �ݾ� ����
        gold -= currentMachineGold;

        // ��� ���� ��
        machine_level[machinePage]++;

        MachineUIUpdate();

        // �Ҹ� ���
        SoundManager.instance.PlaySound("Unlock");
    }




    /*
    IEnumerator SpawnJellyRandomly()
    {
        while (true) // ���� �ݺ�
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            spawnJelly(); // ���� ����
        }
    }

    
    void spawnJelly()
    {
        page = Random.Range(0, 6);
        Vector3[] spawnPos = new Vector3[]
        {
            new Vector3(Random.Range(-4.5f, 4.5f), 1.3f, 0),            
            new Vector3(Random.Range(15.5f, 24.5f), 1.3f, 0),
            new Vector3(Random.Range(35.5f, 44.5f), 1.3f, 0),           
            new Vector3(Random.Range(55.5f, 64.5f), 1.3f, 0),         
            new Vector3(Random.Range(75.5f, 84.5f), 1.3f, 0),

        };

        int RandomSpawnPos = Random.Range(0, 5);
        

        GameObject obj = Instantiate(prefab, spawnPos[RandomSpawnPos], Quaternion.identity); // ���� ������ ����
        Jelly jelly = obj.GetComponent<Jelly>(); // ������ ���� ������Ʈ�� Jelly ��ũ��Ʈ�� ������
        obj.name = "Jelly " + page; // ���� ������Ʈ�� �̸��� ���� ������ ��ȣ�� ����
        jelly.id = page; // ������ ID�� ���� �������� ����
        jelly.sprite_renderer.sprite = jelly_spritelist[page]; // ������ ��������Ʈ �̹����� ���� �������� �ش��ϴ� �̹����� ����

        jelly_list.Add(jelly); // ������ ���� ����Ʈ�� �߰�
    }
    */

    // Ư�� Map�� ���� ����
    void SpawnJellyOnMap(Vector3 spawnCenter, float spawnRangeX, List<Jelly> jellyList)
    {
        float randomX = Random.Range(spawnCenter.x - spawnRangeX, spawnCenter.x + spawnRangeX); // x��ǥ ����ȭ
        Vector3 spawnPosition = new Vector3(randomX, spawnCenter.y, spawnCenter.z); // y, z�� ����

        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Jelly jelly = obj.GetComponent<Jelly>();

        page = Random.Range(0, 6); // ���� ������ ����
        obj.name = "Jelly " + page;
        jelly.id = page;
        jelly.sprite_renderer.sprite = jelly_spritelist[page];

        jellyList.Add(jelly); // �ش� Map�� Jelly ����Ʈ�� �߰�
    }

    // Map 1�� ���� ����
    IEnumerator SpawnJellyOnMap1()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(0, 1.3f, 0), 1.25f, map1JellyList); // �� 1 �߽ɰ� ����
        }
    }

    // Map 2�� ���� ����
    IEnumerator SpawnJellyOnMap2()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(20, 1.3f, 0), 1.25f, map2JellyList); // �� 2 �߽ɰ� ����
        }
    }

    // Map 3�� ���� ����
    IEnumerator SpawnJellyOnMap3()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(40, 1.3f, 0), 1.25f, map3JellyList); // �� 3 �߽ɰ� ����
        }
    }

    // Map 4�� ���� ����
    IEnumerator SpawnJellyOnMap4()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(60, 1.3f, 0), 1.25f, map4JellyList); // �� 4 �߽ɰ� ����
        }
    }

    // Map 5�� ���� ����
    IEnumerator SpawnJellyOnMap5()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(80, 1.3f, 0), 1.25f, map5JellyList); // �� 5 �߽ɰ� ����
        }
    }

    /* �̱� �Լ�
    ���� �гο��� ��ư�� Ŭ���ϸ� �ܰ�մԸ���Ʈ 0~6�� �ε��� �� �ϳ� �������� ����
    �ش� �ε����� ������ collected ó��
    */
    public void RandomPick()
    {
        // ���� ����ƾ�� ��� ������ �ʿ��� ����ƾ ������ ������ �Լ� ����
        if (gold < special_customer_gold)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= special_customer_gold;

        // ��� ������ �����Ǿ����� ����
        if (AllSpecialColleted())
        {
            return;
        }

        int index;

        do
        {
            index = Random.Range(0, collected_list.Length); // 0���� 5���� ���� �ε��� ����
            
            
        } while (collected_list[index]); // �̹� ������ ���� �ε����� �������� ����


        collected_name[specialNum] = special_customer_namelist[index];
        collected_sprites[specialNum] = special_customer_spritelist[index];
        specialNum++;

        collected_list[index] = true; // ���� ��� ���� ���·� ����
        collectedManager.UpdateCollectedList(index, true); // index 1�� collected_list�� true�� ����
        SoundManager.instance.PlaySound("Unlock");

    }

    // ��� ������ �����Ǿ����� üũ�ϴ� �޼ҵ�
    private bool AllSpecialColleted()
    {
        foreach (bool collected in collected_list)
        {
            if (!collected)
            {
                return false; // �ϳ��� �������� ���� ������ ������ false ��ȯ
            }
        }
        return true; // ��� �����Ǿ����� true ��ȯ
    }


    /* �ܰ�մ� ���� �Լ�
    spawnJellyó�� �������� �̵�, collected_list[page] = false�̸� �ٸ� �� ã��
    true�̸� spawnJellyó�� ����
    ����Ʈ �߰�?
    */
    // Ư�� Map�� �ܰ�մ� ����
    void SpawnSpecialOnMap(Vector3 spawnPos, int index, List<SpecialCustomer> specialCustomerList)
    {
        // ����: �̸��� ��������Ʈ�� ��ȿ�� ��쿡�� ����
        if (!string.IsNullOrEmpty(collected_name[index]) && collected_sprites[index] != null)
        {
            // �ܰ�մ� ������Ʈ ����
            GameObject obj = Instantiate(prefab_special_customer, spawnPos, Quaternion.identity); // �ܰ�մ� ������ ����
            SpecialCustomer specialCustomer = obj.GetComponent<SpecialCustomer>(); // ������ �ܰ�մ� ������Ʈ�� SpecialCustomer ��ũ��Ʈ�� ������
            obj.name = "Special Customer " + index; // �ܰ�մ� ������Ʈ�� �̸��� ���� ������ ��ȣ�� ����
            specialCustomer.id = index;
            specialCustomer.sprite_renderer.sprite = collected_sprites[index]; // �ܰ�մ��� ��������Ʈ �̹����� ����

            // ��ƼŬ �ý��� ������ ����
            GameObject instantFavEffectObj = Instantiate(favorability_effect_prefab);
            ParticleSystem instantFavEffect = instantFavEffectObj.GetComponent<ParticleSystem>();

            // ��ƼŬ �ý����� �ܰ�մԿ� �Ҵ�
            specialCustomer.favorability_effect = instantFavEffect;

            // ��ƼŬ �ý��� �ν��Ͻ��� �����ϵ��� ����
            specialCustomer.favorabilityEffectInstance = instantFavEffectObj;

            // GameManager���� �ܰ�մ��� ȣ������ �ҷ���
            specialCustomer.favorability = GetFavorability(index);

            // �ܰ�մ��� ����Ʈ�� �߰�
            specialCustomerList.Add(specialCustomer);
        }
    }

    // �� �ʺ��� �ܰ�մ��� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnSpecialOnMap1()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            int index = Random.Range(0, 3); // 1�� �ʿ����� �ܰ�մ� ���� (0 ~ 4)
            Vector3 spawnPos = new Vector3(Random.Range(-1.25f, 1.25f), 1.3f, 0); // 1�� ���� ���� ��ġ
            SpawnSpecialOnMap(spawnPos, index, map1specialCustomerList); // �ܰ�մ� ����
        }
    }

    IEnumerator SpawnSpecialOnMap2()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            int index = Random.Range(3, 6); // 2�� �ʿ����� �ܰ�մ� ���� (5 ~ 8)
            Vector3 spawnPos = new Vector3(Random.Range(18.75f, 21.25f), 1.3f, 0); // 2�� ���� ���� ��ġ
            SpawnSpecialOnMap(spawnPos, index, map2specialCustomerList); // �ܰ�մ� ����
        }
    }

    IEnumerator SpawnSpecialOnMap3()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            int index = Random.Range(6, 9); // 3�� �ʿ����� �ܰ�մ� ���� (9 ~ 13)
            Vector3 spawnPos = new Vector3(Random.Range(38.75f, 41.25f), 1.3f, 0); // 3�� ���� ���� ��ġ
            SpawnSpecialOnMap(spawnPos, index, map3specialCustomerList); // �ܰ�մ� ����
        }
    }

    IEnumerator SpawnSpecialOnMap4()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            int index = Random.Range(9, 12); // 4�� �ʿ����� �ܰ�մ� ���� (14 ~ 18)
            Vector3 spawnPos = new Vector3(Random.Range(58.75f, 61.25f), 1.3f, 0); // 4�� ���� ���� ��ġ
            SpawnSpecialOnMap(spawnPos, index, map4specialCustomerList);
        }
    }

    IEnumerator SpawnSpecialOnMap5()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // ���� �ð� ����
            yield return new WaitForSeconds(waitTime); // ���� �ð���ŭ ���
            int index = Random.Range(12, 15); // 5�� �ʿ����� �ܰ�մ� ���� (19 ~ 23)
            Vector3 spawnPos = new Vector3(Random.Range(78.75f, 81.25f), 1.3f, 0); // 5�� ���� ���� ��ġ
            SpawnSpecialOnMap(spawnPos, index, map5specialCustomerList); // �ܰ�մ� ����
        }
    }


    // ȣ���� ������Ʈ �Լ�
    public void UpdateFavorability(int index, int favorability)
    {

        specialCustomerFavorability[index] = favorability;

    }

    // Ư�� �ܰ�մ��� ȣ������ �ҷ����� �Լ�
    public int GetFavorability(int index)
    {

        return specialCustomerFavorability[index];

    }

    // Ŭ�� ��� ȹ�� �Լ�
    public void ClickGetGold(int goldReward)
    {
        gold += goldReward;
    }

    /**
     *  Bridge for PlayerDataModel families
     */

    // declaring local variable in this position is for optimization
    private PlayerDataModel playerDataModel;
    
    public void PushAndSavePlayerData()
    {
        PlayerDataContainer.I.PlayerData = new PlayerDataModel
        (
            jelatin,
            gold,
            jelly_unlock_list,
            jelly_data_list,
            num_level,
            click_level,
            SoundManager.instance != null ? SoundManager.instance.bgm_slider.value : PlayerDataModelDefaults.BGM_VOLUME,
            SoundManager.instance != null ? SoundManager.instance.sfx_slider.value : PlayerDataModelDefaults.SFX_VOLUME
        );
        PlayerDataContainer.I.PushDataToLocal();
    }

    public void LoadAndPullPlayerData()
    {
        playerDataModel = PlayerDataContainer.I.PlayerData;
        jelatin = playerDataModel.jelatin;
        gold = playerDataModel.gold;
        jelly_unlock_list = playerDataModel.jellyUnlocks;
        jelly_data_list = playerDataModel.jelly.Select(each => (Data)each).ToList();
        num_level = playerDataModel.numLevel;
        click_level = playerDataModel.clickLevel;
        if (SoundManager.instance)
        {
            SoundManager.instance.bgm_slider.value = playerDataModel.bgmVolume;
            SoundManager.instance.sfx_slider.value = playerDataModel.sfxVolume;
        }
    }
}
