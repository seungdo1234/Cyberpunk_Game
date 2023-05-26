using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrystalCoolTime : MonoBehaviour
{
    private float coolTime;
    private Image coolTimeImage;
    // target의 transform 정보를 나타냄
    private Transform targetTransform;
    // UI의 위치정보를 제어
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private IEnumerator StartCoolTime()
    {
        float timer = 0f;

        while (timer < coolTime)
        {
            timer += Time.deltaTime;
            float fillAmount = 1 - (timer / coolTime); // 슬라이드의 fillAmount 계산
            coolTimeImage.fillAmount = fillAmount;

            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetUp(Transform target, float coolTime)
    {
        // Image UI가 쫒아다닐 target 설정
        targetTransform = target;
        this.coolTime = coolTime;
        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
        coolTimeImage = GetComponent<Image>();
        StartCoroutine(StartCoolTime());
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // 오브젝트의 위치가 갱신된 이후에 Image UI도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 호출한다.

        // target 위치에 월드 좌표를 기준으로 현재 스크린에서의 좌표를 구해 Image의 위치를 설정
        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면내에서 좌표 + distance만큼 떨어진 위치를 Slider UI의 위치로 설정
        rectTransform.position = screenPosition;
    }
}
