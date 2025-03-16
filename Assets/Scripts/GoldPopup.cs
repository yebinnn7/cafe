using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPopup : MonoBehaviour
{
    private Vector3 initialPopupPosition; // 팝업의 초기 위치 저장
    private Coroutine popupCoroutine; // 현재 실행 중인 팝업 코루틴 참조
    private Image goldPopupImage;
    public Text goldPopupText; // 추가된 텍스트 UI
    private GameManager gameManager; // GameManager 참조

    void Start()
    {
        goldPopupImage = GetComponent<Image>();
        goldPopupImage.gameObject.SetActive(false);
        initialPopupPosition = transform.position; // 초기 위치 저장

        gameManager = FindObjectOfType<GameManager>(); // GameManager 찾기
    }

    public void ShowGoldPopup(int amount)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine); // 이전 애니메이션 중지
        }

        transform.position = initialPopupPosition; // 초기 위치 리셋
        goldPopupImage.gameObject.SetActive(true);

        if (gameManager != null)
        {
            goldPopupText.text = $"+{gameManager.goldReward}"; // 골드 값 적용
        }

        popupCoroutine = StartCoroutine(AnimateGoldPopup());
    }

    IEnumerator AnimateGoldPopup()
    {
        Vector3 endPos = initialPopupPosition + new Vector3(0, 1, 0); // 위로 50만큼 이동
        Color startColor = goldPopupImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // 점점 투명해짐

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(initialPopupPosition, endPos, t);
            goldPopupImage.color = Color.Lerp(startColor, endColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        goldPopupImage.gameObject.SetActive(false);
        popupCoroutine = null; // 코루틴 종료 후 참조 해제
    }
}
