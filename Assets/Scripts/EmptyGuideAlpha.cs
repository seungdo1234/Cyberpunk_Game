using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmptyGuideAlpha : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // 지속 시간
    [SerializeField]
    private TextMeshProUGUI emptyText; // 비었을 때 나는 안내말

    private Image emptyGauge; // 스테이지 시작 시 나오는 스테이지 이름 이미지

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
