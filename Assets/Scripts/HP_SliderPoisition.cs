using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_SliderPoisition : MonoBehaviour
{
    [SerializeField] // �����̴��� �� ������Ʈ �Ÿ��� ��Ÿ���� ����
    private Vector3 distance;
    // target�� transform ������ ��Ÿ��
    private Transform targetTransform;
    // UI�� ��ġ������ ����
    private RectTransform rectTransform;

    // EnemySpawner���� ���� �����̴��� ������ �� ȣ���ϸ� target �Ű������� �i�ƴٴ� ���� Transform ������ ����
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
        // Slider UI�� �i�ƴٴ� target ����
        targetTransform = target;
        // RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� �i�ƴٴ� ����� ������� Slider UI�� ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�.

        // target ��ġ�� ���� ��ǥ�� �������� ���� ��ũ�������� ��ǥ�� ���� Slider�� ��ġ�� ����
        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�鳻���� ��ǥ + distance��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }
}
