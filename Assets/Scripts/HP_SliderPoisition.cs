using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_SliderPoisition : MonoBehaviour
{
    [SerializeField] // 슬라이더와 적 오브젝트 거리를 나타내는 변수
    private Vector3 distance;
    // target의 transform 정보를 나타냄
    private Transform targetTransform;
    // UI의 위치정보를 제어
    private RectTransform rectTransform;

    // EnemySpawner에서 적과 슬라이더를 생성할 때 호출하며 target 매개변수에 쫒아다닐 적의 Transform 정보를 제공
    public void Setup(Transform target, int enemyType)
    {
        if (enemyType == 1)
        {
            distance = Vector3.down * 50.0f;
        }
        else if(enemyType == 0)
        {
            distance = Vector3.down * 30.0f;
        }
        // Slider UI가 쫒아다닐 target 설정
        targetTransform = target;
        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 쫒아다닐 대상이 사라지면 Slider UI도 삭제
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트의 위치가 갱신된 이후에 Slider UI도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 호출한다.

        // target 위치에 월드 좌표를 기준으로 현재 스크린에서의 좌표를 구해 Slider의 위치를 설정
        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면내에서 좌표 + distance만큼 떨어진 위치를 Slider UI의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }
}
