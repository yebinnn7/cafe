using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialCustomer : MonoBehaviour
{
    // 젤리의 식별자와 상태 정보를 저장하는 변수들
    public int id; // 젤리의 고유 ID
    public int level; // 젤리의 레벨
    public float exp; // 젤리의 현재 경험치

    public float required_exp; // 레벨 업에 필요한 경험치
    public float max_exp; // 최대 경험치

    // 게임 내 다른 오브젝트들과의 상호 작용을 위한 변수들
    public GameObject game_manager_obj; // GameManager 오브젝트 참조
    public GameManager game_manager; // GameManager 스크립트 참조
    public GameObject left_top; // 왼쪽 위 경계 오브젝트
    public GameObject right_bottom; // 오른쪽 아래 경계 오브젝트

    // 젤리의 시각적 표현을 위한 컴포넌트들
    public SpriteRenderer sprite_renderer; // 스프라이트 렌더러 컴포넌트
    public Sprite[] backSprite; // 스프라이트의 반대 모습
    public Animator anim; // 애니메이터 컴포넌트

    float pick_time; // 마우스 클릭 시간 측정 변수

    // 이동 관련 번수들
    int move_delay; // 이동 대기 시간
    int move_time; // 실제 이동 시간

    float speed_x; // x축 이동 속도
    float speed_y; // y축 이동 속도

    // bool isWandering; // 무작위로 움직이고 있는지 여부
    // bool isWalking; // 걷고 있는지 여부

    // 그림자 오브젝트 관련 변수들
    GameObject shadow; // 그림자 오브젝트
    float shadow_pos_y; // 그림자의 y좌표

   

    // 젤리 이동
    public float moveSpeed = 1f; // 오브젝트가 이동하는 속도
    private bool movingDown = true; // 현재 이동 방향이 아래쪽인지 여부

    // 젤리 대기
    private bool isWaiting = false; // 대기 상태를 체크하는 변수
    public float minWaitTime = 4f;
    public float maxWaitTime = 7f;

    public int favorability = 0;
    public ParticleSystem favorability_effect;
    public GameObject favorabilityEffectInstance; // 파티클 시스템 인스턴스 추적(파괴하기 위해서)

    Camera selectedCamera;
    private GameObject trash;

    public string specialMenuName;        // 선호하는 메뉴 이름
    public string specialMenuDescription; // 선호하는 메뉴 설명
    public Sprite specialMenuImage;

    bool isUnlock = false;
    bool isFavorabilityUp = false;



    // 게임 시작 시 필요한 변수와 오브젝트를 설정하는 초기화 함수
    void Awake()
    {
        
        // 왼쪽 위와 오른쪽 아래의 경계 오브젝트를 찾음
        left_top = GameObject.Find("LeftTop").gameObject;
        right_bottom = GameObject.Find("RightBottom").gameObject;
        // GameManager 오브젝트를 찾고, 해당 스크립트를 참조
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        // SpriteRenderer와 Animator 컴포넌트 가져오기
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        

        // 그림자 오브젝트를 찾고 그림자 위치를 설정
        shadow = transform.Find("Shadow").gameObject;
        switch (id) // 젤리 ID에 따라 그림자의 y 좌표를 다르게 설정
        {
            case 0: shadow_pos_y = -0.05f; break;
            case 6: shadow_pos_y = -0.12f; break;
            case 3: shadow_pos_y = -0.14f; break;
            case 10: shadow_pos_y = -0.16f; break;
            case 11: shadow_pos_y = -0.16f; break;
            default: shadow_pos_y = -0.05f; break;
        }

        // 그림자 위치 설정
        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0);

        // GameManager에서 기존 호감도 가져옴
        favorability = game_manager.GetFavorability(id);

        

        Invoke("CheckTrashInArea", 0.5f);
    }

    public void SetID(int newid)
    {
        id = newid;
    }




    // 매 프레임마다 호출되는 함수로, 주로 상태 업데이트를 담당
    void Update()
    {
        

        // 젤리의 경험치가 최대 경험치보다 적을 때, 시간이 지남에 따라 경험치를 증가시킴
        if (exp < max_exp)
            exp += Time.deltaTime;

        // 현재 경험치가 레벨업에 필요한 경험치 이상이면 레벨을 증가시킴
        if (exp > required_exp * level && level < 3) // 레벨 3 이하까지만 가능
        {
            game_manager.ChangeAc(anim, ++level); // 레벨이 오르면 GameManager에서 애니메이션과 상태 변경
            SoundManager.instance.PlaySound("Grow");
        }


        if (!isWaiting) // 대기 중이 아니면 이동
        {
            MoveObject(); // 오브젝트 이동을 별도 함수로 처리
        }

    }

    // 마우스 클릭 시 젤리를 터치하는 이벤트 처리
    public void OnMouseDown()
    {
        // 게임이 진행중이지 않다면 아무런 동작도 하지 않음
        if (!game_manager.isLive) return;

        // 걷는 동작을 멈추고 터치 애니메이션 실행
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        SoundManager.instance.PlaySound("Touch");
        
        if (!isFavorabilityUp)
        {

            // 호감도가 20 이상일 때
            if (favorability >= 20 && isUnlock == false && game_manager.unlockMenu[id] == false)
            {
                game_manager.ClickMenuBtn(id);
                isUnlock = true;
                return; // 호감도가 20 이상일 때는 더 이상 진행되지 않음
            }

            if (favorability < 20)
            {
                // 호감도가 20 미만일 경우에만 증가하고 이펙트 생성
                favorability += 2;
                game_manager.UpdateFavorability(id, favorability);

                // 이펙트 생성
                FavEffectPlay();
            }

            isFavorabilityUp = true;
        }
        

    }



    // 오브젝트의 이동을 처리하는 함수
    void MoveObject()
    {
        if (movingDown)
        {
            // 아래쪽으로 이동
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // y좌표가 -1.5에 도달하면 5초 대기
            if (transform.position.y <= -0.9f)
            {
                movingDown = false; // 이동 방향을 위로 바꿈
                StartCoroutine(WaitAtPosition()); // 5초 대기 시작
            }
        }
        else
        {
            // 위쪽으로 이동
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            if (id < 17)
            {
                sprite_renderer.sprite = backSprite[id]; // 반대 스프라이트로 변환(test)
            }

            // y좌표가 1.5에 도달하면 오브젝트 삭제
            if (transform.position.y >= 1.5f)
            {               
                RemoveFromSpecialCustomerList();
                Destroy(gameObject); // 오브젝트 파괴
                Destroy(favorabilityEffectInstance); // 파티클 시스템 파괴
            }
        }
    }

    // 5초간 대기하는 코루틴
    IEnumerator WaitAtPosition()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        isWaiting = true; // 대기 상태로 변경
        yield return new WaitForSeconds(waitTime); // 4~7초 대기
        isWaiting = false; // 대기 종료 후 다시 이동
    }

    void FavEffectPlay()
    {
        favorability_effect.transform.position = transform.position;
        favorability_effect.Play();
    }

    void RemoveFromSpecialCustomerList()
    {
        float xPosition = transform.position.x;

        // 젤리가 속한 맵을 xPosition으로 판단
        if (xPosition >= -1.25f && xPosition <= 1.25f)
        {
            game_manager.map1specialCustomerList.Remove(this.GetComponent<SpecialCustomer>());
        }
        else if (xPosition >= 18.75f && xPosition <= 21.25f)
        {
            game_manager.map2specialCustomerList.Remove(this.GetComponent<SpecialCustomer>());
        }
        else if (xPosition >= 38.75f && xPosition <= 41.25f)
        {
            game_manager.map3specialCustomerList.Remove(this.GetComponent<SpecialCustomer>());
        }
        else if (xPosition >= 58.75f && xPosition <= 61.25f)
        {
            game_manager.map4specialCustomerList.Remove(this.GetComponent<SpecialCustomer>());
        }
        else if (xPosition >= 78.75f && xPosition <= 81.25f)
        {
            game_manager.map5specialCustomerList.Remove(this.GetComponent<SpecialCustomer>());
        }
    }

    void CheckTrashInArea()
    {
        float xPosition = transform.position.x;  // 현재 오브젝트의 X 좌표

        // 트레쉬가 특정 범위에 있을 경우 호감도 감소
        if (xPosition >= -1.25f && xPosition <= 1.25f)
        {
            // 해당 범위에 trash 오브젝트가 있는지 확인
            CheckTrashInMap(-1.25f, 1.25f);
            game_manager.map1specialCustomerList.Remove(this);
        }
        else if (xPosition >= 18.75f && xPosition <= 21.25f)
        {
            CheckTrashInMap(18.75f, 21.25f);
            game_manager.map2specialCustomerList.Remove(this);
        }
        else if (xPosition >= 38.75f && xPosition <= 41.25f)
        {
            CheckTrashInMap(38.75f, 41.25f);
            game_manager.map3specialCustomerList.Remove(this);
        }
        else if (xPosition >= 58.75f && xPosition <= 61.25f)
        {
            CheckTrashInMap(58.75f, 61.25f);
            game_manager.map4specialCustomerList.Remove(this);
        }
        else if (xPosition >= 78.75f && xPosition <= 81.25f)
        {
            CheckTrashInMap(78.75f, 81.25f);
            game_manager.map5specialCustomerList.Remove(this);
        }

        // 해당 맵 범위에 trash가 있는지 확인하고, 호감도 감소
        void CheckTrashInMap(float minX, float maxX)
        {
            // trash 태그가 붙은 모든 오브젝트 찾기
            GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");

            foreach (GameObject trash in trashObjects)
            {
                // trash 오브젝트의 X좌표가 지정된 범위 내에 있는지 확인
                if (trash.transform.position.x >= minX && trash.transform.position.x <= maxX)
                {
                    // 호감도 감소
                    favorability -= 2;  // 원하는 값으로 호감도 감소
                    Debug.Log("호감도가 감소했습니다. 현재 호감도: " + favorability);
                    game_manager.UpdateFavorability(id, favorability);

                }
            }
        }
    }
}