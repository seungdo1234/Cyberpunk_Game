using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitMark : MonoBehaviour
{
    [SerializeField] // �����̴��� �� ������Ʈ �Ÿ��� ��Ÿ���� ����
    private Vector3 distance;
    // target�� transform ������ ��Ÿ��
    private Transform targetTransform;

    // EnemySpawner���� ���� �����̴��� ������ �� ȣ���ϸ� target �Ű������� �i�ƴٴ� ���� Transform ������ ����
    public void Setup(Transform target)
    {
        // Slider UI�� �i�ƴٴ� target ����
        targetTransform = target;

    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� �i�ƴٴ� ����� ������� Slider UI�� ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = targetTransform.position + distance;
    }
}
