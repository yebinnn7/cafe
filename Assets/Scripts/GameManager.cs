using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;// // 싱글톤 패턴으로 GameManager 인스턴스를 전역에서 접근할 수 있게 설정

    public int jelatin; // 젤라틴 자원
    public int gold; // 골드 자워
    public List<Jelly> jelly_list = new List<Jelly>(); // 생성된 젤리들을 저장할 리스트
    public List<Data> jelly_data_list = new List<Data>(); // 저장된 젤리의 데이터를 저장할 리스트
    public bool[] jelly_unlock_list; // 젤리 잠금 해제 상태를 저장할 배열

    public int max_jelatin; // 젤라틴의 최대치
    public int max_gold; // 골드의 최대치

    public bool isSell; // 젤리를 판매할 수 있는 상태인지 여부
    public bool isLive; // 게임이 활성화된 상태인지 여부

    public Sprite[] jelly_spritelist; // 젤리의 스프라이트 리스트
    public string[] jelly_namelist; // 젤리 이름 리스트
    public int[] jelly_jelatinlist; // 젤리 잠금 해제에 필요한 젤라틴 리스트
    public int[] jelly_goldlist; // 젤리 구매에 필요한 골드 리스트
    public int map_goldlist;

    public Text page_text; // 페이지를 표시하는 텍스트 UI
    public Image unlock_group_jelly_img; // 잠금 해제된 젤리의 이미지를 표시할 UI
    public Text unlock_group_gold_text; // 잠금 해제된 젤리의 구매 비용을 표시할 텍스트
    public Text unlock_group_name_text; // 잠금 해제된 젤리의 이름을 표시할 텍스트 UI

    public GameObject lock_group; // 잠금된 젤리 그룹을 관리하는 오브젝트
    public GameObject maplock_group; // 잠금된 젤리 그룹을 관리하는 오브젝트
    public Image lock_group_jelly_img; // 잠금된 젤리의 이미지를 표시할 UI
    public Text lock_group_jelatin_text; // 잠금 해제에 필요한 젤라틴 수량을 표시할 텍스트 UI

    public Text lock_group_map_text; // 맵 잠금 해제에 필요한 골드를 표시할 텍스트 UI

    // Animator 변경 관리를 위한 Animator 배열
    public RuntimeAnimatorController[] level_ac; // 젤리 레벨에 따른 애니메이터 컨트롤러 리스트

    public Text jelatin_text; // 젤라틴 자원 수량을 표시할 텍스트 UI
    public Text gold_text; // 골드 자원 수량을 표시할 텍스트 UI


    public Image jelly_panel; // 젤리 메뉴 패널
    public Image plant_panel; // 플랜트 메뉴 패널
    public Image option_panel; // 옵션 메뉴 패널
    public Image map_panel; // 맵 메뉴 패널

    public GameObject prefab; // 젤리 프리팹

    public GameObject data_manager_obj; // DataManager 오브젝트
    DataManager data_manager; // DataManager 스크립트

    Animator jelly_anim; // 젤리 패널 애니메이션 관리
    Animator plant_anim; // 플랜트 패널 애니메이션 관리
    Animator map_anim; // 맵 패널 애니메이션 관리

    bool isJellyClick; // 젤리 버튼이 클릭된 상태인지 여부
    bool isPlantClick; // 플랜트 버튼이 클릭된 상태인지 여부
    bool isOption; // 옵션 패널이 활성화된 상태인지 여부
    bool isMapClick; // 맵 버튼이 클릭된 상태인지 여부

    int page; // 현재 선택된 페이지

    // 업그레이드 시스템 변수
    public int num_level;
    public Text num_sub_text;
    public Text num_btn_text;
    public Button num_btn;
    public int[] num_goldlist;

    public int click_level;
    public Text click_sub_text;
    public Text click_btn_text;
    public Button click_btn;
    public int[] click_goldlist;

    // 젤리 스폰시간
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 8f;
    public float jellyLifetime;

    // 젤리 스폰위치


    void Awake()
    {
        instance = this; // 싱글톤 패턴 적용

        // 패널 애니메이터 초기화
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        map_anim = map_panel.GetComponent<Animator>();

        isLive = true; // 게임 활성화 상태로 설정

        // UI 초기화
        jelatin_text.text = jelatin.ToString(); // int형을 string형으로 변환
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString();
        lock_group_map_text.text = map_goldlist.ToString();

        // DataManager 초기화
        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0; // 첫 페이지로 초기화
        jelly_unlock_list = new bool[12]; // 젤리 잠금 해제 배열 초기화 (12개의 젤리)
    }

    void Start()
    {
        // 데이터를 불러오기 위한 호출, 씬이 로드된 직후 호출되므로 약간의 지연 후 실행
        // Invoke("LoadData", 0.1f);
        StartCoroutine(SpawnJellyRandomly());
    }

    void Update()
    {
        // '취소' 버튼이 눌렸을 때 처리
        if (Input.GetButtonDown("Cancel"))
        {
            if (isJellyClick) ClickJellyBtn(); // 젤리 메뉴가 열려 있으면 닫음
            else if (isPlantClick) ClickPlantBtn(); // 플랜트 메뉴가 열려 있으면 닫음
            else if (isMapClick) ClickMapBtn(); // 플랜트 메뉴가 열려 있으면 닫음
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

        if (isMapClick) // 맵 메뉴가 열려 있으면 닫음
            map_anim.SetTrigger("doHide");
        else // 맵 메뉴가 닫혀 있으면 열음
            map_anim.SetTrigger("doShow");

        isMapClick = !isMapClick; // 맵 클릭 상태를 토글
        isLive = !isLive; // 게임 활성화 상태를 토글
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
        maplock_group.gameObject.SetActive(false); //맵 잠금 해제
    }

    //맵 잠금 해제 함수
    public void MapUnlock()
    {
        // 현재 골드가 잠금 해제에 필요한 골드보다 적으면 함수 종료
        if (gold < map_goldlist)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        MapChange(); // 페이지 UI 업데이트

        gold -= map_goldlist; //골드 감소

        SoundManager.instance.PlaySound("Unlock");
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
        page = Random.Range(0, 12);

        GameObject obj = Instantiate(prefab, new Vector3(Random.Range(-4.5f, 4.5f), 1.3f, 0), Quaternion.identity); // 젤리 프리팹 생성
        Jelly jelly = obj.GetComponent<Jelly>(); // 생성된 젤리 오브젝트의 Jelly 스크립트를 가져옴
        obj.name = "Jelly " + page; // 젤리 오브젝트의 이름을 현재 페이지 번호로 설정
        jelly.id = page; // 젤리의 ID를 현재 페이지로 설정
        jelly.sprite_renderer.sprite = jelly_spritelist[page]; // 젤리의 스프라이트 이미지를 현재 페이지에 해당하는 이미지로 설정

        jelly_list.Add(jelly); // 젤리를 젤리 리스트에 추가
    }
}