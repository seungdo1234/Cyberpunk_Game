using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillUI : MonoBehaviour
{
    private Slider skillSlider; // ��ų �����̴�
    [SerializeField]
    private TextMeshProUGUI coolTimeText; // ��Ÿ��
    [SerializeField]
    private Image[] skill_Image; // ��ų �̹���

    [Header("TeleAtk")]
    [SerializeField]
    private Slider teleAtkSlider; // �ڷ���Ʈ ���� ��ų ��Ÿ�� �����̴�
    private Coroutine teleCoroutine; // �ڷ�ƾ�� ������Ű�� ���� �ڷ�ƾ ����

    private void Start()
    {
        skillSlider = GetComponent<Slider>();
    }
    public void UsingSkill(float coolTime) // �÷��̾ ������ ��ô ��ų�� ����� �� ȣ��
    {
        StartCoroutine(KnifeCoolTimeActive(coolTime));
    }
    public void TeleAtkOn() // Enemy�� �������� �¾��� �� ȣ��
    {
        teleCoroutine = StartCoroutine(TeleAtkCoolTimeActive(5f));
    }
    private IEnumerator KnifeCoolTimeActive(float coolTime) // ������ ��Ÿ��
    {
        float elapsedTime = 0;
        coolTimeText.color = new Color(255, 255, 255, 255);
        // �����̴� �ִ� ����
        skillSlider.maxValue = coolTime;

        while (elapsedTime < coolTime)
        {
            elapsedTime += Time.deltaTime;
            coolTimeText.text = ((int)coolTime - (int)elapsedTime).ToString();
            skillSlider.value = coolTime - elapsedTime; // �����̴� �� ����
            yield return null;
        }
        // ���� ���ð��� ������ �����̴� �ʱ�ȭ
        coolTimeText.color = new Color(255, 255, 255 ,0);
        skillSlider.value = 0;
    }
    private IEnumerator TeleAtkCoolTimeActive(float coolTime) // �ڷ���Ʈ ���� ��Ÿ�� �� �̹��� ����
    {
        float elapsedTime = 0;
        // �����̴� �ִ� ����
        teleAtkSlider.maxValue = coolTime;
        skill_Image[1].color = new Color(255, 255, 255, 255);
        while (elapsedTime < coolTime)
        {
            elapsedTime += Time.deltaTime;
            teleAtkSlider.value = coolTime - elapsedTime; // �����̴� �� ����
            yield return null;
        }
        skill_Image[1].color = new Color(255, 255, 255, 0);
        // ���� ���ð��� ������ �����̴� �ʱ�ȭ
        teleAtkSlider.value = 0;
    }
    public void TeleAtkEnd() // �ڷ���Ʈ ������ �����ٸ� �ڷ���Ʈ ���� �ڷ�ƾ ����
    {
        skill_Image[1].color = new Color(255, 255, 255, 0);
        teleAtkSlider.value = 0;
        StopCoroutine(teleCoroutine);
        teleCoroutine = null;

    }
}
