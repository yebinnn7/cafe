using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPopup : MonoBehaviour
{
    private Vector2 initialPopupPosition; // UI 좌표를 저장하는 변수
    private Coroutine popupCoroutine;
    private Image goldPopupImage;
    public Text goldPopupText;
    private GameManager gameManager;
    private RectTransform rectTransform; // RectTransform 참조

    void Start()
    {
        goldPopupImage = GetComponent<Image>();
        goldPopupImage.gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>(); // RectTransform 가져오기
        initialPopupPosition = rectTransform.anchoredPosition; // 초기 UI 위치 저장

        gameManager = FindObjectOfType<GameManager>();
    }

    public void ShowGoldPopup(int amount)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }

        rectTransform.anchoredPosition = initialPopupPosition; // UI 위치 리셋
        goldPopupImage.gameObject.SetActive(true);

        if (gameManager != null)
        {
            goldPopupText.text = $" +{gameManager.cafeGold[gameManager.cafeNum - 1]}";
        }

        popupCoroutine = StartCoroutine(AnimateGoldPopup());
    }

    IEnumerator AnimateGoldPopup()
    {
        Vector2 endPos = initialPopupPosition + new Vector2(0, 20); // UI 기준 위로 50 이동
        Color startColor = goldPopupImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPopupPosition, endPos, t);
            goldPopupImage.color = Color.Lerp(startColor, endColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        goldPopupImage.gameObject.SetActive(false);
        popupCoroutine = null;
    }
}
