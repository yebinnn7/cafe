using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialCustomer : MonoBehaviour
{
    // ������ �ĺ��ڿ� ���� ������ �����ϴ� ������
    public int id; // ������ ���� ID
    public int level; // ������ ����
    public float exp; // ������ ���� ����ġ

    public float required_exp; // ���� ���� �ʿ��� ����ġ
    public float max_exp; // �ִ� ����ġ

    // ���� �� �ٸ� ������Ʈ����� ��ȣ �ۿ��� ���� ������
    public GameObject game_manager_obj; // GameManager ������Ʈ ����
    public GameManager game_manager; // GameManager ��ũ��Ʈ ����
    public GameObject left_top; // ���� �� ��� ������Ʈ
    public GameObject right_bottom; // ������ �Ʒ� ��� ������Ʈ

    // ������ �ð��� ǥ���� ���� ������Ʈ��
    public SpriteRenderer sprite_renderer; // ��������Ʈ ������ ������Ʈ
    public Sprite backSprite; // ��������Ʈ�� �ݴ� ���
    public Animator anim; // �ִϸ����� ������Ʈ

    float pick_time; // ���콺 Ŭ�� �ð� ���� ����

    // �̵� ���� ������
    int move_delay; // �̵� ��� �ð�
    int move_time; // ���� �̵� �ð�

    float speed_x; // x�� �̵� �ӵ�
    float speed_y; // y�� �̵� �ӵ�

    // bool isWandering; // �������� �����̰� �ִ��� ����
    // bool isWalking; // �Ȱ� �ִ��� ����

    // �׸��� ������Ʈ ���� ������
    GameObject shadow; // �׸��� ������Ʈ
    float shadow_pos_y; // �׸����� y��ǥ

   

    // ���� �̵�
    public float moveSpeed = 1f; // ������Ʈ�� �̵��ϴ� �ӵ�
    private bool movingDown = true; // ���� �̵� ������ �Ʒ������� ����

    // ���� ���
    private bool isWaiting = false; // ��� ���¸� üũ�ϴ� ����
    public float minWaitTime = 4f;
    public float maxWaitTime = 7f;

    public int favorability = 0;
    public ParticleSystem favorability_effect;
    public GameObject favorabilityEffectInstance; // ��ƼŬ �ý��� �ν��Ͻ� ����(�ı��ϱ� ���ؼ�)

    Camera selectedCamera;
    private GameObject trash;

    public string specialMenuName;        // ��ȣ�ϴ� �޴� �̸�
    public string specialMenuDescription; // ��ȣ�ϴ� �޴� ����
    public Sprite specialMenuImage;

    bool isUnlock = false;
    bool isFavorabilityUp = false;



    // ���� ���� �� �ʿ��� ������ ������Ʈ�� �����ϴ� �ʱ�ȭ �Լ�
    void Awake()
    {
        
        // ���� ���� ������ �Ʒ��� ��� ������Ʈ�� ã��
        left_top = GameObject.Find("LeftTop").gameObject;
        right_bottom = GameObject.Find("RightBottom").gameObject;
        // GameManager ������Ʈ�� ã��, �ش� ��ũ��Ʈ�� ����
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        // SpriteRenderer�� Animator ������Ʈ ��������
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        

        // �׸��� ������Ʈ�� ã�� �׸��� ��ġ�� ����
        shadow = transform.Find("Shadow").gameObject;
        switch (id) // ���� ID�� ���� �׸����� y ��ǥ�� �ٸ��� ����
        {
            case 0: shadow_pos_y = -0.05f; break;
            case 6: shadow_pos_y = -0.12f; break;
            case 3: shadow_pos_y = -0.14f; break;
            case 10: shadow_pos_y = -0.16f; break;
            case 11: shadow_pos_y = -0.16f; break;
            default: shadow_pos_y = -0.05f; break;
        }

        // �׸��� ��ġ ����
        shadow.transform.localPosition = new Vector3(0, shadow_pos_y, 0);

        // GameManager���� ���� ȣ���� ������
        favorability = game_manager.GetFavorability(id);

        

        Invoke("CheckTrashInArea", 0.5f);
    }

    public void SetID(int newid)
    {
        id = newid;
    }




    // �� �����Ӹ��� ȣ��Ǵ� �Լ���, �ַ� ���� ������Ʈ�� ���
    void Update()
    {
        

        // ������ ����ġ�� �ִ� ����ġ���� ���� ��, �ð��� ������ ���� ����ġ�� ������Ŵ
        if (exp < max_exp)
            exp += Time.deltaTime;

        // ���� ����ġ�� �������� �ʿ��� ����ġ �̻��̸� ������ ������Ŵ
        if (exp > required_exp * level && level < 3) // ���� 3 ���ϱ����� ����
        {
            game_manager.ChangeAc(anim, ++level); // ������ ������ GameManager���� �ִϸ��̼ǰ� ���� ����
            SoundManager.instance.PlaySound("Grow");
        }


        if (!isWaiting) // ��� ���� �ƴϸ� �̵�
        {
            MoveObject(); // ������Ʈ �̵��� ���� �Լ��� ó��
        }

    }

    // ���콺 Ŭ�� �� ������ ��ġ�ϴ� �̺�Ʈ ó��
    public void OnMouseDown()
    {
        // ������ ���������� �ʴٸ� �ƹ��� ���۵� ���� ����
        if (!game_manager.isLive) return;

        // �ȴ� ������ ���߰� ��ġ �ִϸ��̼� ����
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        SoundManager.instance.PlaySound("Touch");
        
        if (!isFavorabilityUp)
        {

            // ȣ������ 20 �̻��� ��
            if (favorability >= 20 && isUnlock == false && game_manager.unlockMenu[id] == false)
            {
                game_manager.ClickMenuBtn(id);
                isUnlock = true;
                return; // ȣ������ 20 �̻��� ���� �� �̻� ������� ����
            }

            if (favorability < 20)
            {
                // ȣ������ 20 �̸��� ��쿡�� �����ϰ� ����Ʈ ����
                favorability += 2;
                game_manager.UpdateFavorability(id, favorability);

                // ����Ʈ ����
                FavEffectPlay();
            }

            isFavorabilityUp = true;
        }
        

    }



    // ������Ʈ�� �̵��� ó���ϴ� �Լ�
    void MoveObject()
    {
        if (movingDown)
        {
            // �Ʒ������� �̵�
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // y��ǥ�� -1.5�� �����ϸ� 5�� ���
            if (transform.position.y <= -0.9f)
            {
                movingDown = false; // �̵� ������ ���� �ٲ�
                StartCoroutine(WaitAtPosition()); // 5�� ��� ����
            }
        }
        else
        {
            // �������� �̵�
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            if (id < 17)
            {
                sprite_renderer.sprite = backSprite; // �ݴ� ��������Ʈ�� ��ȯ(test)
            }

            // y��ǥ�� 1.5�� �����ϸ� ������Ʈ ����
            if (transform.position.y >= 1.5f)
            {               
                RemoveFromSpecialCustomerList();
                Destroy(gameObject); // ������Ʈ �ı�
                Destroy(favorabilityEffectInstance); // ��ƼŬ �ý��� �ı�
            }
        }
    }

    // 5�ʰ� ����ϴ� �ڷ�ƾ
    IEnumerator WaitAtPosition()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        isWaiting = true; // ��� ���·� ����
        yield return new WaitForSeconds(waitTime); // 4~7�� ���
        isWaiting = false; // ��� ���� �� �ٽ� �̵�
    }

    void FavEffectPlay()
    {
        favorability_effect.transform.position = transform.position;
        favorability_effect.Play();
    }

    void RemoveFromSpecialCustomerList()
    {
        float xPosition = transform.position.x;

        // ������ ���� ���� xPosition���� �Ǵ�
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
        float xPosition = transform.position.x;  // ���� ������Ʈ�� X ��ǥ

        // Ʈ������ Ư�� ������ ���� ��� ȣ���� ����
        if (xPosition >= -1.25f && xPosition <= 1.25f)
        {
            // �ش� ������ trash ������Ʈ�� �ִ��� Ȯ��
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

        // �ش� �� ������ trash�� �ִ��� Ȯ���ϰ�, ȣ���� ����
        void CheckTrashInMap(float minX, float maxX)
        {
            // trash �±װ� ���� ��� ������Ʈ ã��
            GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");

            foreach (GameObject trash in trashObjects)
            {
                // trash ������Ʈ�� X��ǥ�� ������ ���� ���� �ִ��� Ȯ��
                if (trash.transform.position.x >= minX && trash.transform.position.x <= maxX)
                {
                    // ȣ���� ����
                    favorability -= 2;  // ���ϴ� ������ ȣ���� ����
                    Debug.Log("ȣ������ �����߽��ϴ�. ���� ȣ����: " + favorability);
                    game_manager.UpdateFavorability(id, favorability);

                }
            }
        }
    }
}