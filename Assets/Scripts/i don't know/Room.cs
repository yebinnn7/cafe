using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���� ���ӽ����̽� �߰�

public class Room : MonoBehaviour
{
    public GameObject game_manager_obj; // GameManager ������Ʈ ����
    public GameManager game_manager; // GameManager ��ũ��Ʈ ����
    public GoldPopup goldPopup; // GoldPopup ��ũ��Ʈ ����

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