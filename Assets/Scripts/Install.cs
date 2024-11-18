using UnityEngine;

public class Install : MonoBehaviour
{
    float pick_time; // 마우스 클릭 시간 측정 변수

    // 마우스 드래그 시 젤리를 끌어당기는 동작 처리
    void OnMouseDrag()
    {

        pick_time += Time.deltaTime; // 마우스 클릭 시간을 누적

        // 클릭 시간이 너무 짧으면 드래그를 처리하지 않음
        if (pick_time < 0.1f) return;

        // 마우스 위치를 월드 좌표로 변환하여 젤리의 위치를 이동
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y));

        transform.position = point;
    }
}
