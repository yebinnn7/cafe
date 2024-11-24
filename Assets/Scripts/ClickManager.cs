using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public LayerMask backgroundLayer;  // 배경 레이어 (예: 8번 레이어)
    public LayerMask objectLayer;      // 오브젝트 레이어 (예: 0번 레이어)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 배경과 오브젝트 레이어에 대해 두 번의 Raycast를 사용하여 우선순위 처리
            RaycastHit2D backgroundHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, backgroundLayer);
            RaycastHit2D objectHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, objectLayer);

            // 오브젝트가 클릭되었으면
            if (objectHit.collider != null)
            {
                Debug.Log($"{objectHit.collider.gameObject.name} 클릭됨!");
                // 오브젝트 클릭 처리
            }
            // 배경이 클릭되었으면 (오브젝트가 없을 때)
            else if (backgroundHit.collider != null)
            {
                Debug.Log("배경 클릭됨!");
                // 배경 클릭 처리
            }
        }
    }
}