using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; // 모호한 참조 오류뜸

public class GameManager : MonoBehaviour
{
    public static GameManager instance;// // 싱글톤 패턴으로 GameManager 인스턴스를 전역에서 접근할 수 있게 설정

    [Header("Money")]
    public int jelatin; // 젤라틴 자원
    public int gold; // 골드 자원

    [Space(10f)]
    [Header("List")]
    public List<Jelly> jelly_list = new List<Jelly>(); // 생성된 젤리들을 저장할 리스트
    public List<Data> jelly_data_list = new List<Data>(); // 저장된 젤리의 데이터를 저장할 리스트
    public bool[] jelly_unlock_list; // 젤리 잠금 해제 상태를 저장할 배열
    public List<SpecialCustomer> special_customer_list = new List<SpecialCustomer>();
    public List<Data> special_customer_data_list = new List<Data>();

    public int max_jelatin; // 젤라틴의 최대치
    public int max_gold; // 골드의 최대치

    [Space(10f)]
    [Header("Game On Off")]
    public bool isSell; // 젤리를 판매할 수 있는 상태인지 여부
    public bool isLive; // 게임이 활성화된 상태인지 여부

    [Space(10f)]
    [Header("Store Jelly")]
    public Sprite[] jelly_spritelist; // 젤리의 스프라이트 리스트
    public string[] jelly_namelist; // 젤리 이름 리스트
    public int[] jelly_jelatinlist; // 젤리 잠금 해제에 필요한 젤라틴 리스트
    public int[] jelly_goldlist; // 젤리 구매에 필요한 골드 리스트

    [Space(10f)]
    [Header("Map")]
    public int[] map_goldlist; // 맵 구매에 필요한 골드를 나타냄
    public int lock_cafe_list; // 잠긴 카페가 몇호점인지 나타냄

    [Space(10f)]
    [Header("Store Jelly(Unlock)")]
    public Text page_text; // 페이지를 표시하는 텍스트 UI
    public Image unlock_group_jelly_img; // 잠금 해제된 젤리의 이미지를 표시할 UI
    public Text unlock_group_gold_text; // 잠금 해제된 젤리의 구매 비용을 표시할 텍스트
    public Text unlock_group_name_text; // 잠금 해제된 젤리의 이름을 표시할 텍스트 UI

    [Space(10f)]
    [Header("Store Jelly(Lock)")]
    public GameObject lock_group; // 잠금된 젤리 그룹을 관리하는 오브젝트
    public Image lock_group_jelly_img; // 잠금된 젤리의 이미지를 표시할 UI
    public Text lock_group_jelatin_text; // 잠금 해제에 필요한 젤라틴 수량을 표시할 텍스트 UI

    // Animator 변경 관리를 위한 Animator 배열
    [Space(10f)]
    [Header("Animation")]
    public RuntimeAnimatorController[] level_ac; // 젤리 레벨에 따른 애니메이터 컨트롤러 리스트

    [Space(10f)]
    [Header("Canvers")]
    public Text jelatin_text; // 젤라틴 자원 수량을 표시할 텍스트 UI
    public Text gold_text; // 골드 자원 수량을 표시할 텍스트 UI


    public Image jelly_panel; // 젤리 메뉴 패널
    public Image plant_panel; // 플랜트 메뉴 패널
    public Image option_panel; // 옵션 메뉴 패널
    public Image map_panel; // 맵 메뉴 패널
    public Image random_panel; // 랜덤 메뉴 패널
    public Image collected_panel; // 도감 메뉴 패널
    public Image information_panel; // 정보 메뉴 패널


    [Space(10f)]
    [Header("Prefabs")]
    public GameObject prefab; // 젤리 프리팹
    public GameObject prefab_special_customer; // 단골손님 프리팹
    public GameObject favorability_effect_prefab; // 호감도 이펙트 프리팹

    [Space(10f)]
    [Header("Data")]
    public GameObject data_manager_obj; // DataManager 오브젝트
    DataManager data_manager; // DataManager 스크립트

    [Space(10f)]
    [Header("Animation")]
    Animator jelly_anim; // 젤리 패널 애니메이션 관리
    Animator plant_anim; // 플랜트 패널 애니메이션 관리
    Animator map_anim; // 맵 패널 애니메이션 관리
    Animator random_anim; // 랜덤 패널 애니메이션 관리
    Animator collected_anim; // 도감 패널 애니메이션 관리
    Animator information_anim; // 정보 패널 애니메이션 관리

    [Space(10f)]
    [Header("Click On Off")]
    bool isJellyClick; // 젤리 버튼이 클릭된 상태인지 여부
    bool isPlantClick; // 플랜트 버튼이 클릭된 상태인지 여부
    bool isOption; // 옵션 패널이 활성화된 상태인지 여부
    bool isMapClick; // 맵 버튼이 클릭된 상태인지 여부
    bool isRandomClick; // 랜덤 버튼이 클릭된 상태인지 여부
    bool isCollectedClick; // 도감 버튼이 클릭된 상태인지 여부
    public bool isInformationClick;

    int page; // 현재 선택된 페이지
    int index; // 단골 손님 번호

    [Space(10f)]
    [Header("Upgrade")]
    // 업그레이드 시스템 변수
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

    // 젤리 스폰시간
    [Space(10f)]
    [Header("SpawnTime")]
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 8f;

    [Space(10f)]
    [Header("Regular customer")]
    // 단골손님 뽑기 가격
    public int special_customer_gold;

    // 단골손님 리스트
    public Sprite[] special_customer_spritelist; // 젤리의 스프라이트 리스트
    public string[] special_customer_namelist; // 젤리 이름 리스트
    public bool[] collected_list;

    // 단골손님 스폰시간
    public float minSpecialSpawnTime = 5f;
    public float maxSpecialSpawnTime = 8f;

    // 단골손님 호감도 저장 딕셔너리
    public int[] specialCustomerFavorability;

    // Collected Manager 참조
    private CollectedManager collectedManager;

    // Map별로 스폰 위치 배열 정의
    Vector3[] spawnPos;

    // 기계 구매
    public int[] machine_level;
    public Text[] machine_sub_text;
    public Text[] machine_btn_text;
    public Button[] machine_btn;


    // 각 기계의 레벨별 금액
    public int[] machine_goldlist1 = new int[5];  // 기계 1의 금액 (레벨 1~5)
    public int[] machine_goldlist2 = new int[5];  // 기계 2의 금액 (레벨 1~5)
    public int[] machine_goldlist3 = new int[5];  // 기계 3의 금액 (레벨 1~5)
    public int[] machine_goldlist4 = new int[5];  // 기계 4의 금액 (레벨 1~5)
    public int[] machine_goldlist5 = new int[5];  // 기계 5의 금액 (레벨 1~5)

    // 각 기계의 이미지 (1~5)
    public GameObject[] machine_list1 = new GameObject[5];  // 기계 1의 이미지 (레벨 1~5)
    public GameObject[] machine_list2 = new GameObject[5];  // 기계 2의 이미지 (레벨 1~5)
    public GameObject[] machine_list3 = new GameObject[5];  // 기계 3의 이미지 (레벨 1~5)
    public GameObject[] machine_list4 = new GameObject[5];  // 기계 4의 이미지 (레벨 1~5)
    public GameObject[] machine_list5 = new GameObject[5];  // 기계 5의 이미지 (레벨 1~5)

    // Map별 손님 리스트
    public List<Jelly> map1JellyList = new List<Jelly>();
    public List<SpecialCustomer> map1specialCustomerList = new List<SpecialCustomer>(); // 1번 맵

    public List<Jelly> map2JellyList = new List<Jelly>();
    public List<SpecialCustomer> map2specialCustomerList = new List<SpecialCustomer>(); // 2번 맵

    public List<Jelly> map3JellyList = new List<Jelly>();
    public List<SpecialCustomer> map3specialCustomerList = new List<SpecialCustomer>(); // 3번 맵

    public List<Jelly> map4JellyList = new List<Jelly>();
    public List<SpecialCustomer> map4specialCustomerList = new List<SpecialCustomer>(); // 4번 맵

    public List<Jelly> map5JellyList = new List<Jelly>();
    public List<SpecialCustomer> map5specialCustomerList = new List<SpecialCustomer>(); // 5번 맵

    



    void Awake()
    {
        instance = this; // 싱글톤 패턴 적용

        // 패널 애니메이터 초기화
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        map_anim = map_panel.GetComponent<Animator>();
        random_anim = random_panel.GetComponent<Animator>();
        collected_anim = collected_panel.GetComponent<Animator>();
        information_anim = information_panel.GetComponent<Animator>();

        isLive = true; // 게임 활성화 상태로 설정

        // UI 초기화
        jelatin_text.text = jelatin.ToString(); // int형을 string형으로 변환
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString();

        // DataManager 초기화
        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0; // 첫 페이지로 초기화
        jelly_unlock_list = new bool[12]; // 젤리 잠금 해제 배열 초기화 (12개의 젤리)

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
        // 데이터를 불러오기 위한 호출, 씬이 로드된 직후 호출되므로 약간의 지연 후 실행
        // Invoke("LoadData", 0.1f);
        // StartCoroutine(SpawnJellyRandomly());
        // StartCoroutine(SpawnSpecialRandomly());

        StartCoroutine(SpawnJellyOnMap1());
        StartCoroutine(SpawnJellyOnMap2());
        StartCoroutine(SpawnJellyOnMap3());
        StartCoroutine(SpawnJellyOnMap4());
        StartCoroutine(SpawnJellyOnMap5());

        // 각 맵별로 단골손님 스폰 코루틴을 시작
        StartCoroutine(SpawnSpecialOnMap1()); // 1번 맵
        StartCoroutine(SpawnSpecialOnMap2()); // 2번 맵
        StartCoroutine(SpawnSpecialOnMap3()); // 3번 맵
        StartCoroutine(SpawnSpecialOnMap4()); // 4번 맵
        StartCoroutine(SpawnSpecialOnMap5()); // 5번 맵

        for (int i = 0; i < machine_btn.Length; i++)
        {
            // 각 버튼에 해당하는 인덱스를 전달하여 함수 연결
            int index = i;
            machine_btn[i].onClick.AddListener(() => BuyMachine(index));
        }
    }

    void Update()
    {
        // '취소' 버튼이 눌렸을 때 처리
        if (Input.GetButtonDown("Cancel"))
        {
            if (isJellyClick) ClickJellyBtn(); // 젤리 메뉴가 열려 있으면 닫음
            else if (isPlantClick) ClickPlantBtn(); // 플랜트 메뉴가 열려 있으면 닫음
            else if (isMapClick) ClickMapBtn(); // 플랜트 메뉴가 열려 있으면 닫음
            else if (isRandomClick) ClickRandomBtn(); // 랜덤 메뉴가 열려 있으면 닫음
            else if (isCollectedClick) ClickCollectedBtn(); // 도감 메뉴가 열려 있으면 닫음
            else if (isInformationClick) collectedManager.ExitButton(); // 도감 메뉴가 열려 있으면 닫음
            else Option(); // 옵션 메뉴를 열거나 닫음
        }

    }

    void LateUpdate()
    {
        // 부드럽게 텍스트 값을 업데이트하며 부동소수점 계산 오차를 최소화하기 위해 반올림 적용
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }

    // 젤리 레벨에 따라 Animator Controller를 변경
    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    // 젤라틴 획득
    public void GetJelatin(int id, int level)
    {
        jelatin += (id + 1) * level * click_level; // id와 레벨에 따라 젤라틴 증가

        if (jelatin > max_jelatin) // 젤라틴이 최대치를 넘지 않도록 제한
            jelatin = max_jelatin;
    }

    // 골드 획득 및 젤리 제거
    public void GetGold(int id, int level, Jelly jelly)
    {
        gold += jelly_goldlist[id] * level; // 골드 추가

        if (gold > max_gold) // 골드가 최대치를 넘지 않도록 제한
            gold = max_gold;

        jelly_list.Remove(jelly); // 젤리 리스트에서 제거

        SoundManager.instance.PlaySound("Sell");
    }

    // 판매 상태 체크
    public void CheckSell()
    {
        isSell = !isSell; // 판매 상태를 토글
    }

    // 젤리 메뉴 버튼 클릭 처리
    public void ClickJellyBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isPlantClick) // 플랜트 메뉴가 열려 있으면 닫음
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // 랜덤 메뉴가 열려 있으면 닫음
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // 도감 메뉴가 열려 있으면 닫음
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // 도감 메뉴가 열려 있으면 닫음
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }


        if (isJellyClick) // 젤리 메뉴가 열려 있으면 닫음
            jelly_anim.SetTrigger("doHide");
        else // 젤리 메뉴가 닫혀 있으면 열음
            jelly_anim.SetTrigger("doShow");

        isJellyClick = !isJellyClick; // 젤리 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
    }

    // 플랜트 메뉴 버튼 클릭 처리
    public void ClickPlantBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isJellyClick) // 젤리 메뉴가 열려 있으면 닫음
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // 랜덤 메뉴가 열려 있으면 닫음
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // 도감 메뉴가 열려 있으면 닫음
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // 도감 메뉴가 열려 있으면 닫음
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isPlantClick) // 플랜트 메뉴가 열려 있으면 닫음
            plant_anim.SetTrigger("doHide");
        else // 플랜트 메뉴가 닫혀 있으면 열음
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick; // 플랜트 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
    }

    public void ClickMapBtn()
    {
        SoundManager.instance.PlaySound("Button");

        if (isJellyClick) // 젤리 메뉴가 열려 있으면 닫음
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick) // 플랜트 메뉴가 열려 있으면 닫음
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isRandomClick) // 랜덤 메뉴가 열려 있으면 닫음
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isCollectedClick) // 도감 메뉴가 열려 있으면 닫음
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // 도감 메뉴가 열려 있으면 닫음
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
            map_anim.SetTrigger("doHide");
        else // 맵 메뉴가 닫혀 있으면 열음
            map_anim.SetTrigger("doShow");

        isMapClick = !isMapClick; // 맵 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
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

        if (isJellyClick) // 젤리 메뉴가 열려 있으면 닫음
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick) // 플랜트 메뉴가 열려 있으면 닫음
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isCollectedClick) // 도감 메뉴가 열려 있으면 닫음
        {
            collected_anim.SetTrigger("doHide");
            isCollectedClick = false;
            isLive = true;
        }

        if (isInformationClick) // 도감 메뉴가 열려 있으면 닫음
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isRandomClick) // 랜덤 메뉴가 열려 있으면 닫음
            random_anim.SetTrigger("doHide");
        else // 랜덤 메뉴가 닫혀 있으면 열음
            random_anim.SetTrigger("doShow");


        isRandomClick = !isRandomClick; // 맵 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
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

        if (isJellyClick) // 젤리 메뉴가 열려 있으면 닫음
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
            
        }

        if (isPlantClick) // 플랜트 메뉴가 열려 있으면 닫음
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
        {
            map_anim.SetTrigger("doHide");
            isMapClick = false;
            isLive = true;
        }

        if (isRandomClick) // 랜덤 메뉴가 열려 있으면 닫음
        {
            random_anim.SetTrigger("doHide");
            isRandomClick = false;
            isLive = true;
        }

        if (isInformationClick) // 도감 메뉴가 열려 있으면 닫음
        {
            information_anim.SetTrigger("doHide");
            isInformationClick = false;
            // isLive = true;
        }

        if (isCollectedClick) // 도감 메뉴가 열려 있으면 닫음
            collected_anim.SetTrigger("doHide");
        else // 랜덤 메뉴가 닫혀 있으면 열음
            collected_anim.SetTrigger("doShow");



        isCollectedClick = !isCollectedClick; // 맵 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
    }

    public void ClickCollectedExitBtn()
    {
        collected_anim.SetTrigger("doHide");

        isCollectedClick = false;
        isLive = true;
    }

    // 옵션 패널 열고 닫기
    public void Option()
    {
        isOption = !isOption; // 옵션 패널 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글

        option_panel.gameObject.SetActive(isOption); // 옵션 패널 표시/숨기기
        Time.timeScale = isOption == true ? 0 : 1; // 옵션 패널이 열리면 시간 정지

        if (isOption) SoundManager.instance.PlaySound("Pause In");
        else SoundManager.instance.PlaySound("Pause Out");
    }

    // 페이지를 다음으로 이동
    public void PageUp()
    {
        if (page >= 11) // 최대 페이지를 넘지 않도록 제한
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

    // 페이지 변경에 따른 UI 업데이트
    void ChangePage()
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]); // 현재 페이지의 젤리가 잠금 상태인지 여부에 따라 잠금 그룹 표시

        page_text.text = string.Format("#{0:00}", (page + 1)); // 페이지 번호 표시

        if (lock_group.activeSelf) // 잠금 상태일 경우 잠금 그룹 UI 업데이트
        {
            lock_group_jelly_img.sprite = jelly_spritelist[page]; // 현재 페이지에 해당하는 젤리 이미지를 잠금 그룹에 설정
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]); // 해당 젤리의 해제에 필요한 젤라틴 수량을 텍스트로 표시

            lock_group_jelly_img.SetNativeSize(); // 이미지의 크기를 원본 크기로 설정
        }
        else // 잠금 해제 상태일 경우 잠금 해제 그룹 UI 업데이트
        {
            unlock_group_jelly_img.sprite = jelly_spritelist[page]; // 현재 페이지에 해당하는 젤리 이미지를 잠금 해제 그룹에 설정
            // 젤리의 이름과 가격을 텍스트로 표시
            unlock_group_name_text.text = jelly_namelist[page];
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);

            unlock_group_jelly_img.SetNativeSize(); // 이미지의 크기를 원본 크기로 설정
        }
    }

    // 젤리 잠금 해제 함수
    public void Unlock()
    {
        // 현재 젤라틴이 잠금 해제에 필요한 젤라틴 수보다 적으면 함수 종료
        if (jelatin < jelly_jelatinlist[page])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        jelly_unlock_list[page] = true; // 젤리 잠금 해제 상태로 변경
        ChangePage(); // 페이지 UI 업데이트

        jelatin -= jelly_jelatinlist[page]; // 젤라틴 수량 감소

        SoundManager.instance.PlaySound("Unlock");
    }

    public void MapChange()
    {
        MapManager.instance.maplock_group[lock_cafe_list].gameObject.SetActive(false); //맵 잠금 해제
        MapManager.instance.maplock_button[lock_cafe_list].gameObject.SetActive(false); //맵 잠금 버튼 해제
    }

    //맵 잠금 해제 함수
    public void MapUnlock()
    {
        // 현재 골드가 잠금 해제에 필요한 골드보다 적으면 함수 종료
        if (gold < map_goldlist[lock_cafe_list])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        MapChange(); // 페이지 UI 업데이트

        gold -= map_goldlist[lock_cafe_list]; //골드 감소

        SoundManager.instance.PlaySound("Unlock");

        mapunlock_Button.gameObject.SetActive(false);
    }

    // 젤리 구매 함수
    public void BuyJelly()
    {
        // 현재 골드가 젤리 구매에 필요한 골드보다 적으면 함수 종료
        if (gold < jelly_goldlist[page] || jelly_list.Count >= num_level * 2)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= jelly_goldlist[page]; // 골드 감소

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity); // 젤리 프리팹 생성
        Jelly jelly = obj.GetComponent<Jelly>(); // 생성된 젤리 오브젝트의 Jelly 스크립트를 가져옴
        obj.name = "Jelly " + page; // 젤리 오브젝트의 이름을 현재 페이지 번호로 설정
        jelly.id = page; // 젤리의 ID를 현재 페이지로 설정
        jelly.sprite_renderer.sprite = jelly_spritelist[page]; // 젤리의 스프라이트 이미지를 현재 페이지에 해당하는 이미지로 설정

        jelly_list.Add(jelly); // 젤리를 젤리 리스트에 추가

        SoundManager.instance.PlaySound("Buy");
    }

    // 게임 데이터를 불러오는 함수
    void LoadData()
    {
        // 현재 페이지의 젤리 잠금 상태에 따라 잠금 그룹 표시
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        // 저장된 젤리 데이터를 불러와서 게임에 반영
        for (int i = 0; i < jelly_data_list.Count; ++i)
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity); // 젤리 프리팹 생성, 저장된 위치(pos)에 생성
            Jelly jelly = obj.GetComponent<Jelly>(); // 생성된 젤리 오브젝트의 Jelly 스크립트를 가져옴
            // 젤리의 ID, 레벨, 경험치를 저장된 데이터로 설정
            jelly.id = jelly_data_list[i].id;
            jelly.level = jelly_data_list[i].level;
            jelly.exp = jelly_data_list[i].exp;
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id]; // 젤리의 스프라이트 이미지를 저장된 ID에 해당하는 이미지로 설정
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1]; // 젤리의 애니메이션 컨트롤러를 레벨에 맞게 설정
            obj.name = "Jelly " + jelly.id; // 젤리 오브젝트의 이름을 Jelly + ID로 설정

            jelly_list.Add(jelly); // 젤리를 젤리 리스트에 추가

            num_sub_text.text = "젤리 수용량 " + num_level * 2;
            if (num_level >= 5) num_btn.gameObject.SetActive(false);
            else num_btn_text.text = string.Format("{0:n0}", num_goldlist[num_level]);

            click_sub_text.text = "클릭 생산량 X " + click_level;
            if (click_level >= 5) click_btn.gameObject.SetActive(false);
            else click_btn_text.text = string.Format("{0:n0}", click_goldlist[click_level]);
        }
    }

    // 애플리케이션 종료 시 데이터를 저장하는 함수
    public void Exit()
    {
        // 데이터 매니저를 통해 게임 데이터를 JSON 형식으로 저장
        data_manager.JsonSave();

        SoundManager.instance.PlaySound("Pause Out");

        Application.Quit();
    }

    // 젤리 수용량 업그레이드 함수
    public void NumUpgrade()
    {
        if (gold < num_goldlist[num_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= num_goldlist[num_level++];

        num_sub_text.text = "젤리 수용량 " + num_level * 2;

        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_goldlist[num_level]);

        SoundManager.instance.PlaySound("Unlock");
        // SoundManager.instance.PlaySound("Buy");

    }

    // 클릭 생산량 업그레이드 함수
    public void ClickUpgrade()
    {
        if (gold < click_goldlist[click_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= click_goldlist[click_level++];

        click_sub_text.text = "클릭 생산량 X " + click_level * 2;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_goldlist[click_level]);

        SoundManager.instance.PlaySound("Unlock");
        // SoundManager.instance.PlaySound("Buy");
    }

    public void BuyMachine(int machineIndex)
    {
        // 기계 금액을 기계의 레벨에 맞게 가져옴
        int currentMachineGold = 0;
        GameObject[] currentMachineList = null;

        // 각 기계에 대해 레벨에 맞는 금액과 이미지를 가져옴
        switch (machineIndex)
        {
            case 0:
                currentMachineGold = machine_goldlist1[machine_level[machineIndex]];
                currentMachineList = machine_list1;
                break;
            case 1:
                currentMachineGold = machine_goldlist2[machine_level[machineIndex]];
                currentMachineList = machine_list2;
                break;
            case 2:
                currentMachineGold = machine_goldlist3[machine_level[machineIndex]];
                currentMachineList = machine_list3;
                break;
            case 3:
                currentMachineGold = machine_goldlist4[machine_level[machineIndex]];
                currentMachineList = machine_list4;
                break;
            case 4:
                currentMachineGold = machine_goldlist5[machine_level[machineIndex]];
                currentMachineList = machine_list5;
                break;
        }

        // 금액 비교
        if (gold < currentMachineGold)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        // 기계 활성화
        currentMachineList[machine_level[machineIndex]].SetActive(true);

        // 금액 차감
        gold -= currentMachineGold;

        // 기계 레벨 업
        machine_level[machineIndex]++;

        // 보유 기계 수 업데이트
        machine_sub_text[machineIndex].text = "보유 기계: " + machine_level[machineIndex] + "개";
        


        // 기계 구매 후 버튼 상태 업데이트
        if (machine_level[machineIndex] >= 5)
        {
            machine_btn_text[machineIndex].text = "최대 보유";
            machine_btn[machineIndex].interactable = false;  // 버튼 비활성화
        }
        else
        {
            // 다음 레벨 금액을 버튼에 표시
            switch (machineIndex)
            {
                case 0:
                    currentMachineGold = machine_goldlist1[machine_level[machineIndex]];
                    break;
                case 1:
                    currentMachineGold = machine_goldlist2[machine_level[machineIndex]];
                    break;
                case 2:
                    currentMachineGold = machine_goldlist3[machine_level[machineIndex]];
                    break;
                case 3:
                    currentMachineGold = machine_goldlist4[machine_level[machineIndex]];
                    break;
                case 4:
                    currentMachineGold = machine_goldlist5[machine_level[machineIndex]];
                    break;
            }

            machine_btn_text[machineIndex].text = string.Format("{0:n0}", currentMachineGold);
        }

        // 소리 재생
        SoundManager.instance.PlaySound("Unlock");
    }


    

    /*
    IEnumerator SpawnJellyRandomly()
    {
        while (true) // 무한 반복
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            spawnJelly(); // 젤리 스폰
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
        

        GameObject obj = Instantiate(prefab, spawnPos[RandomSpawnPos], Quaternion.identity); // 젤리 프리팹 생성
        Jelly jelly = obj.GetComponent<Jelly>(); // 생성된 젤리 오브젝트의 Jelly 스크립트를 가져옴
        obj.name = "Jelly " + page; // 젤리 오브젝트의 이름을 현재 페이지 번호로 설정
        jelly.id = page; // 젤리의 ID를 현재 페이지로 설정
        jelly.sprite_renderer.sprite = jelly_spritelist[page]; // 젤리의 스프라이트 이미지를 현재 페이지에 해당하는 이미지로 설정

        jelly_list.Add(jelly); // 젤리를 젤리 리스트에 추가
    }
    */

    // 특정 Map에 젤리 생성
    void SpawnJellyOnMap(Vector3 spawnCenter, float spawnRangeX, List<Jelly> jellyList)
    {
        float randomX = Random.Range(spawnCenter.x - spawnRangeX, spawnCenter.x + spawnRangeX); // x좌표 랜덤화
        Vector3 spawnPosition = new Vector3(randomX, spawnCenter.y, spawnCenter.z); // y, z는 고정

        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Jelly jelly = obj.GetComponent<Jelly>();

        page = Random.Range(0, 6); // 랜덤 페이지 설정
        obj.name = "Jelly " + page;
        jelly.id = page;
        jelly.sprite_renderer.sprite = jelly_spritelist[page];

        jellyList.Add(jelly); // 해당 Map의 Jelly 리스트에 추가
    }

    // Map 1에 젤리 생성
    IEnumerator SpawnJellyOnMap1()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(0, 1.3f, 0), 1.25f, map1JellyList); // 맵 1 중심과 범위
        }
    }

    // Map 2에 젤리 생성
    IEnumerator SpawnJellyOnMap2()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(20, 1.3f, 0), 1.25f, map2JellyList); // 맵 2 중심과 범위
        }
    }

    // Map 3에 젤리 생성
    IEnumerator SpawnJellyOnMap3()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(40, 1.3f, 0), 1.25f, map3JellyList); // 맵 3 중심과 범위
        }
    }

    // Map 4에 젤리 생성
    IEnumerator SpawnJellyOnMap4()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(60, 1.3f, 0), 1.25f, map4JellyList); // 맵 4 중심과 범위
        }
    }

    // Map 5에 젤리 생성
    IEnumerator SpawnJellyOnMap5()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnJellyOnMap(new Vector3(80, 1.3f, 0), 1.25f, map5JellyList); // 맵 5 중심과 범위
        }
    }

    /* 뽑기 함수
    랜덤 패널에서 버튼을 클릭하면 단골손님리스트 0~6의 인덱스 중 하나 랜덤으로 뽑음
    해당 인덱스의 젤리는 collected 처리
    */
    public void RandomPick()
    {
        // 현재 젤라틴이 잠금 해제에 필요한 젤라틴 수보다 적으면 함수 종료
        if (gold < special_customer_gold)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= special_customer_gold;

        // 모든 젤리가 해제되었으면 종료
        if (AllSpecialColleted())
        {
            return;
        }

        int index;

        do
        {
            index = Random.Range(0, collected_list.Length); // 0부터 5까지 랜덤 인덱스 선택
        } while (collected_list[index]); // 이미 해제된 젤리 인덱스는 선택하지 않음

        collected_list[index] = true; // 젤리 잠금 해제 상태로 변경
        collectedManager.UpdateCollectedList(index, true); // index 1의 collected_list를 true로 변경
        SoundManager.instance.PlaySound("Unlock");

    }

    // 모든 젤리가 해제되었는지 체크하는 메소드
    private bool AllSpecialColleted()
    {
        foreach (bool collected in collected_list)
        {
            if (!collected)
            {
                return false; // 하나라도 해제되지 않은 젤리가 있으면 false 반환
            }
        }
        return true; // 모두 해제되었으면 true 반환
    }


    /* 단골손님 생성 함수
    spawnJelly처럼 랜덤으로 뽑되, collected_list[page] = false이면 다른 수 찾음
    true이면 spawnJelly처럼 생성
    이펙트 추가?
    */
    // 특정 Map에 단골손님 생성
    void SpawnSpecialOnMap(Vector3 spawnPos, int index, List<SpecialCustomer> specialCustomerList)
    {
        if (collected_list[index]) // index가 collected_list에서 true일 때만 스폰
        {
            // 단골손님 오브젝트 생성
            GameObject obj = Instantiate(prefab_special_customer, spawnPos, Quaternion.identity); // 단골손님 프리팹 생성
            SpecialCustomer specialCustomer = obj.GetComponent<SpecialCustomer>(); // 생성된 단골손님 오브젝트의 SpecialCustomer 스크립트를 가져옴
            obj.name = "Special Customer " + index; // 단골손님 오브젝트의 이름을 현재 페이지 번호로 설정
            specialCustomer.id = index;
            specialCustomer.sprite_renderer.sprite = special_customer_spritelist[index]; // 단골손님의 스프라이트 이미지를 설정

            // 파티클 시스템 프리팹 생성
            GameObject instantFavEffectObj = Instantiate(favorability_effect_prefab);
            ParticleSystem instantFavEffect = instantFavEffectObj.GetComponent<ParticleSystem>();

            // 파티클 시스템을 단골손님에 할당
            specialCustomer.favorability_effect = instantFavEffect;

            // 파티클 시스템 인스턴스를 추적하도록 설정
            specialCustomer.favorabilityEffectInstance = instantFavEffectObj;

            // GameManager에서 단골손님의 호감도를 불러옴
            specialCustomer.favorability = GetFavorability(index);

            // 단골손님을 리스트에 추가
            specialCustomerList.Add(specialCustomer);
        }
    }

    // 각 맵별로 단골손님을 스폰하는 코루틴
    IEnumerator SpawnSpecialOnMap1()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            int index = Random.Range(0, 5); // 1번 맵에서의 단골손님 생성 (0 ~ 4)
            Vector3 spawnPos = new Vector3(Random.Range(-1.25f, 1.25f), 1.3f, 0); // 1번 맵의 랜덤 위치
            SpawnSpecialOnMap(spawnPos, index, map1specialCustomerList); // 단골손님 생성
        }
    }

    IEnumerator SpawnSpecialOnMap2()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            int index = Random.Range(5, 9); // 2번 맵에서의 단골손님 생성 (5 ~ 8)
            Vector3 spawnPos = new Vector3(Random.Range(18.75f, 21.25f), 1.3f, 0); // 2번 맵의 랜덤 위치
            SpawnSpecialOnMap(spawnPos, index, map2specialCustomerList); // 단골손님 생성
        }
    }

    IEnumerator SpawnSpecialOnMap3()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            int index = Random.Range(9, 14); // 3번 맵에서의 단골손님 생성 (9 ~ 13)
            Vector3 spawnPos = new Vector3(Random.Range(38.75f, 41.25f), 1.3f, 0); // 3번 맵의 랜덤 위치
            SpawnSpecialOnMap(spawnPos, index, map3specialCustomerList); // 단골손님 생성
        }
    }

    IEnumerator SpawnSpecialOnMap4()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            int index = Random.Range(14, 19); // 4번 맵에서의 단골손님 생성 (14 ~ 18)
            Vector3 spawnPos = new Vector3(Random.Range(58.75f, 61.25f), 1.3f, 0); // 4번 맵의 랜덤 위치
            SpawnSpecialOnMap(spawnPos, index, map4specialCustomerList);
        }
    }

    IEnumerator SpawnSpecialOnMap5()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpecialSpawnTime, maxSpecialSpawnTime); // 랜덤 시간 설정
            yield return new WaitForSeconds(waitTime); // 랜덤 시간만큼 대기
            int index = Random.Range(19, 24); // 5번 맵에서의 단골손님 생성 (19 ~ 23)
            Vector3 spawnPos = new Vector3(Random.Range(78.75f, 81.25f), 1.3f, 0); // 5번 맵의 랜덤 위치
            SpawnSpecialOnMap(spawnPos, index, map5specialCustomerList); // 단골손님 생성
        }
    }


    // 호감도 업데이트 함수
    public void UpdateFavorability(int index, int favorability)
    {

        specialCustomerFavorability[index] = favorability;

    }

    // 특정 단골손님의 호감도를 불러오는 함수
    public int GetFavorability(int index)
    {

        return specialCustomerFavorability[index];

    }

    // 클릭 골드 획득 함수
    public void ClickGetGold(int goldReward)
    {
        gold += goldReward;
    }
}
