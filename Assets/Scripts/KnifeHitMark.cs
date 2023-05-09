using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitMark : MonoBehaviour
{
    [SerializeField] // 슬라이더와 적 오브젝트 거리를 나타내는 변수
    private Vector3 distance;
    // target의 transform 정보를 나타냄
    private Transform targetTransform;

    // EnemySpawner에서 적과 슬라이더를 생성할 때 호출하며 target 매개변수에 쫒아다닐 적의 Transform 정보를 제공
    public void Setup(Transform target)
    {
        // Slider UI가 쫒아다닐 target 설정
        targetTransform = target;

    }

    private void LateUpdate()
    {
        // 적이 파괴되어 쫒아다닐 대상이 사라지면 Slider UI도 삭제
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = targetTransform.position + distance;
    }
}
