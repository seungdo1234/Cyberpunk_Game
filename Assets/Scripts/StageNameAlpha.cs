using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageNameAlpha : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // 지속 시간
    [SerializeField]
    private TextMeshProUGUI[] stageName; // 스테이지 이름

    private Image stageNameImage; // 스테이지 시작 시 나오는 스테이지 이름 이미지

    private void Start()
    {
        stageNameImage = GetComponent<Image>();
    }
    private IEnumerator AlphaLerp()
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, currentTime / lerpTime);
            stageNameImage.color = new Color(stageNameImage.color.r, stageNameImage.color.g, stageNameImage.color.b, alpha);
            for(int i = 0; i<stageName.Length; i++)
            {
                stageName[i].color = new Color(stageName[i].color.r, stageName[i].color.g, stageName[i].color.b, alpha);
            }
            yield return null;
        }
        yield return new WaitForSeconds(2f);


        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / lerpTime);
            stageNameImage.color = new Color(stageNameImage.color.r, stageNameImage.color.g, stageNameImage.color.b, alpha);
            for (int i = 0; i < stageName.Length; i++)
            {
                stageName[i].color = new Color(stageName[i].color.r, stageName[i].color.g, stageName[i].color.b, alpha);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void StartAlpha()
    {
        StartCoroutine(AlphaLerp());
    }
}
