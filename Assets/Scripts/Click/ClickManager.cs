using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public LayerMask interactableLayers;  // 상호작용 가능한 레이어 (배경 포함)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, interactableLayers);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("Trash"))
                {
                    ClickableObject clickable = clickedObject.GetComponent<ClickableObject>();
                    // clickable.OnMouseDown();
                }
                else if (clickedObject.CompareTag("Jelly"))
                {
                    Jelly jelly = clickedObject.GetComponent<Jelly>();
                    // jelly.OnMouseDown();
                }
                else if (clickedObject.CompareTag("Special"))
                {
                    SpecialCustomer specialCustomer = clickedObject.GetComponent<SpecialCustomer>();
                    // specialCustomer.OnMouseDown();
                }
                else
                {
                    // 배경 클릭 처리
                    Debug.Log("배경 클릭");
                }
            }
        }
    }
}
