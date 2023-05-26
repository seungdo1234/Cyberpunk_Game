using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrystalCoolTime : MonoBehaviour
{
    private float coolTime;
    private Image coolTimeImage;
    // target�� transform ������ ��Ÿ��
    private Transform targetTransform;
    // UI�� ��ġ������ ����
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
            float fillAmount = 1 - (timer / coolTime); // �����̵��� fillAmount ���
            coolTimeImage.fillAmount = fillAmount;

            yield return null;
        }
        Destroy(gameObject);
    }
    public void SetUp(Transform target, float coolTime)
    {
        // Image UI�� �i�ƴٴ� target ����
        targetTransform = target;
        this.coolTime = coolTime;
        // RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
        coolTimeImage = GetComponent<Image>();
        StartCoroutine(StartCoolTime());
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Image UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�.

        // target ��ġ�� ���� ��ǥ�� �������� ���� ��ũ�������� ��ǥ�� ���� Image�� ��ġ�� ����
        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�鳻���� ��ǥ + distance��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition;
    }
}
