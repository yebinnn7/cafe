using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPopup : MonoBehaviour
{
    private Vector3 initialPopupPosition; // �˾��� �ʱ� ��ġ ����
    private Coroutine popupCoroutine; // ���� ���� ���� �˾� �ڷ�ƾ ����
    private Image goldPopupImage;
    public Text goldPopupText; // �߰��� �ؽ�Ʈ UI
    private GameManager gameManager; // GameManager ����

    void Start()
    {
        goldPopupImage = GetComponent<Image>();
        goldPopupImage.gameObject.SetActive(false);
        initialPopupPosition = transform.position; // �ʱ� ��ġ ����

        gameManager = FindObjectOfType<GameManager>(); // GameManager ã��
    }

    public void ShowGoldPopup(int amount)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine); // ���� �ִϸ��̼� ����
        }

        transform.position = initialPopupPosition; // �ʱ� ��ġ ����
        goldPopupImage.gameObject.SetActive(true);

        if (gameManager != null)
        {
            goldPopupText.text = $"+{gameManager.goldReward}"; // ��� �� ����
        }

        popupCoroutine = StartCoroutine(AnimateGoldPopup());
    }

    IEnumerator AnimateGoldPopup()
    {
        Vector3 endPos = initialPopupPosition + new Vector3(0, 1, 0); // ���� 50��ŭ �̵�
        Color startColor = goldPopupImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // ���� ��������

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
        popupCoroutine = null; // �ڷ�ƾ ���� �� ���� ����
    }
}
