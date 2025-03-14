using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스 추가

public class Room : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager 오브젝트 참조
    public GameManager game_manager; // GameManager 스크립트 참조
    public GoldPopup goldPopup; // GoldPopup 스크립트 참조

    void Start()
    {
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        game_manager.ClickGetGold(game_manager.goldReward);
        goldPopup.ShowGoldPopup(game_manager.goldReward);
    }
}