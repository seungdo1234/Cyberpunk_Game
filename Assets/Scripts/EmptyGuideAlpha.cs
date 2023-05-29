using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmptyGuideAlpha : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // ���� �ð�
    [SerializeField]
    private TextMeshProUGUI emptyText; // ����� �� ���� �ȳ���

    private Image emptyGauge; // �������� ���� �� ������ �������� �̸� �̹���

    // Start is called before the first frame update
    void Start()
    {
        emptyGauge = GetComponent<Image>();
    }

    private IEnumerator AlphaLerp()
    {

        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / lerpTime);
            emptyGauge.color = new Color(emptyGauge.color.r, emptyGauge.color.g, emptyGauge.color.b, alpha);
            emptyText.color = new Color(emptyText.color.r, emptyText.color.g, emptyText.color.b, alpha);
            yield return null;
        }
    }
    public void EmptyGauge()
    {
        emptyGauge.color = new Color(emptyGauge.color.r, emptyGauge.color.g, emptyGauge.color.b, 255);
        emptyText.color = new Color(emptyText.color.r, emptyText.color.g, emptyText.color.b, 255);
        StartCoroutine(AlphaLerp());
    }
}
