using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Space(10f)]
    [Header("Map Lock")]
    public Text lock_map_gold_text;
    public GameObject[] maplock_group;
    public GameObject[] maplock_button;
    int cafe;

    [Space(10f)]
    [Header("Cafe Management")]
    public GameObject cafe1;
    public GameObject cafe2;
    public GameObject cafe3;
    public GameObject cafe4;
    public GameObject cafe5;

    public bool cafe1_live;
    public bool cafe2_live;
    public bool cafe3_live;
    public bool cafe4_live;
    public bool cafe5_live;

    [Space(10f)]
    [Header("Camera")]
    public Vector3 targetPosition;

    private void Awake()
    {
        instance = this;
    }

    public void Clickcafe1() { Checkcafe(0); }
    public void Clickcafe2() { Checkcafe(1); }
    public void Clickcafe3() { Checkcafe(2); }
    public void Clickcafe4() { Checkcafe(3); }
    public void Clickcafe5() { Checkcafe(4); }

    public void Checkcafe(int a)
    {
        GameManager.instance.lock_cafe_list = a;
        GameManager.instance.mapunlock_Button.gameObject.SetActive(true);
        cafe = GameManager.instance.map_goldlist[a];
        lock_map_gold_text.text = cafe.ToString();
    }

    public void Movecafe1() { MoveToCafe(new Vector3(0, 0, -10), cafe1); }
    public void Movecafe2() { MoveToCafe(new Vector3(20, 0, -10), cafe2); }
    public void Movecafe3() { MoveToCafe(new Vector3(40, 0, -10), cafe3); }
    public void Movecafe4() { MoveToCafe(new Vector3(60, 0, -10), cafe4); }
    public void Movecafe5() { MoveToCafe(new Vector3(80, 0, -10), cafe5); }

    private void MoveToCafe(Vector3 position, GameObject activeCafe)
    {
        targetPosition = position;
        Camera.main.transform.position = targetPosition;

        // 모든 카페 비활성화 후 선택한 카페만 활성화
        cafe1.SetActive(activeCafe == cafe1);
        cafe2.SetActive(activeCafe == cafe2);
        cafe3.SetActive(activeCafe == cafe3);
        cafe4.SetActive(activeCafe == cafe4);
        cafe5.SetActive(activeCafe == cafe5);
    }
}