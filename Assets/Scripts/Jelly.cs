using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Jelly : MonoBehaviour
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

    // 젤라틴을 획득하는 처리와 관련 변수
    int jelatin_delay; // 젤라틴 획득 지연 시간
    bool isGetting; // 젤라틴을 획득 중인지 여부

    // 젤리 이동
    public float moveSpeed = 1f; // 오브젝트가 이동하는 속도
    private bool movingDown = true; // 현재 이동 방향이 아래쪽인지 여부

    // 젤리 대기
    private bool isWaiting = false; // 대기 상태를 체크하는 변수
    public float minWaitTime = 4f;
    public float maxWaitTime = 7f;

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

        // 초기 값 설정
        // isWandering = false; // 처음에는 젤리가 움직이지 않음
        // isWalking = false;
        isGetting = false; // 처음에는 젤라틴을 획득하지 않음

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

        // 젤라틴을 아직 획득하지 않았다면 젤라틴 획득 코루틴 실행
        if (!isGetting)
            StartCoroutine(GetJelatin());


        if (!isWaiting) // 대기 중이 아니면 이동
        {
            MoveObject(); // 오브젝트 이동을 별도 함수로 처리
        }

    }


    // 물리 업데이트 처리로, 고정된 시간 간격으로 호출됨 
    void FixedUpdate()
    {
        /*
        // 젤리가 현재 무작위로 움직이고 있지 않다면, 무작위로 이동하는 코루틴 실행
        if (!isWandering)
            StartCoroutine(Wander());

        // 걷는 상태일 때 이동 처리
        if (isWalking)
            Move();

        // 젤리의 현재 위치를 확인하여 경계를 벗어나지 않도록 방향을 반전시킴
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        // 왼쪽/오른쪽 경계를 넘으면 x축 방향을 반전
        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x)
            speed_x = -speed_x;
        // 위쪽/아래쪽 경계를 넘으면 y축 방향을 반전
        if (pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            speed_y = -speed_y;
        */
    }

    // 마우스 클릭 시 젤리를 터치하는 이벤트 처리
    public void OnMouseDown()
    {
        // 게임이 진행중이지 않다면 아무런 동작도 하지 않음
        if (!game_manager.isLive) return;

        // 걷는 동작을 멈추고 터치 애니메이션 실행
        // isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        // 경험치가 최대 경험치보다 적으면 경험치를 증가시킴
        if (exp < max_exp) ++exp;

        // GameManager에 젤라틴 획득 이벤트 전달
        game_manager.GetJelatin(id, level);

        SoundManager.instance.PlaySound("Touch");
    }

    // 마우스 드래그 시 젤리를 끌어당기는 동작 처리
    void OnMouseDrag()
    {
        // 게임이 진행 중이지 않으면 드래그 동작을 수행하지 않음
        if (!game_manager.isLive) return;

        pick_time += Time.deltaTime; // 마우스 클릭 시간을 누적

        // 클릭 시간이 너무 짧으면 드래그를 처리하지 않음
        if (pick_time < 0.1f) return;

        // 젤리가 걷는 동작을 멈추고 터치 애니메이션 실행
        // isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        // 마우스 위치를 월드 좌표로 변환하여 젤리의 위치를 이동
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y));

        transform.position = point;
    }

    // 마우스를 떼었을 때 호출되는 함수
    void OnMouseUp()
    {
        // 게임이 진행 중이지 않으면 아무런 동작도 하지 않음
        if (!game_manager.isLive) return;

        pick_time = 0; // 클릭 시간을 초기화

        // 젤리를 판매하는 중이면 골드를 획득하고 젤리를 삭제
        if (game_manager.isSell)
        {
            game_manager.GetGold(id, level, this); // 골드 획득

            Destroy(gameObject); // 젤리 삭제
        }

        // 젤리의 위치가 경계를 벗어났을 때, 젤리를 초기 위치로 되돌림
        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x ||
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            transform.position = new Vector3(0, -1, 0); // 초기 위치로 되돌림
    }

    /*
    // 젤리의 이동 처리 함수
    void Move()
    {
        // x축 속도가 음수이면 스프라이트를 좌우 반전하여 이동 방향에 맞춤
        if (speed_x != 0)
            sprite_renderer.flipX = speed_x < 0;

        // 현재 설정된 속도에 따라 젤리의 위치를 이동
        transform.Translate(speed_x, speed_y, speed_y);
    }

    // 젤리의 무작위 이동을 처리하는 코루틴 함수
    IEnumerator Wander()
    {
        // 무작위 이동 대기 시간과 이동 시간을 설정
        move_delay = Random.Range(3, 6); // 3~6초 동안 대기
        move_time = Random.Range(3, 6); // 3~6초 동안 이동

        // 젤리의 x축과 y축 속도를 무작위로 설정
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        // 젤리가 무작위로 이동 중임을 표시하는 플래그를 true로 설정
        isWandering = true;

        // 이동 대기 시간 동안 대기
        yield return new WaitForSeconds(move_delay);

        // 대기 후 걷는 상태로 변경하고 애니메이션 재생
        isWalking = true;
        anim.SetBool("isWalk", true); // "isWalk" 애니메이션 트리거

        // 설정된 이동 시간 동안 이동
        yield return new WaitForSeconds(move_time);

        // 이동이 끝나면 걷는 상태를 false로 설정하고 애니메이션 정지
        isWalking = false;
        anim.SetBool("isWalk", false); // "isWalk" 애니메이션 정지

        // 무작위 이동이 끝났음을 표시
        isWandering = false;
    }
    */

    // 젤라틴을 주기적으로 획득하는 코루틴 함수
    IEnumerator GetJelatin()
    {
        jelatin_delay = 3; // 젤라틴을 획득하는 데 걸리는 시간 설정

        // 젤라틴 획득 중임을 표시하는 플래그를 true로 설정
        isGetting = true;

        // GameManager에서 젤라틴 획득 함수 호출 (젤리 ID와 레벨 전달)
        game_manager.GetJelatin(id, level);

        // 젤라틴 획득 지연 시간(3초) 동안 대기
        yield return new WaitForSeconds(jelatin_delay);

        // 젤라틴 획득이 끝나면 플래그를 false로 설정
        isGetting = false;
    }

    // 오브젝트의 이동을 처리하는 함수
    void MoveObject()
    {
        if (movingDown)
        {
            // 아래쪽으로 이동
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // y좌표가 -1.5에 도달하면 5초 대기
            if (transform.position.y <= -1.5f)
            {
                movingDown = false; // 이동 방향을 위로 바꿈
                StartCoroutine(WaitAtPosition()); // 5초 대기 시작
            }
        }
        else
        {
            // 위쪽으로 이동
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // y좌표가 1.5에 도달하면 오브젝트 삭제
            if (transform.position.y >= 1.5f)
            {
                Destroy(gameObject); // 오브젝트 파괴
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
}