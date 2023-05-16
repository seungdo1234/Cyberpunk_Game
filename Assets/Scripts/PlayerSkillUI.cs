using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSkillUI : MonoBehaviour
{
    private Slider skillSlider; // 스킬 슬라이더
    [SerializeField]
    private TextMeshProUGUI coolTimeText; // 쿨타임
    [SerializeField]
    private Image[] skill_Image; // 스킬 이미지

    [Header("TeleAtk")]
    [SerializeField]
    private Slider teleAtkSlider; // 텔레포트 공격 스킬 쿨타임 슬라이더
    private Coroutine teleCoroutine; // 코루틴을 정지시키기 위한 코루틴 변수

    private void Start()
    {
        skillSlider = GetComponent<Slider>();
    }
    public void UsingSkill(float coolTime) // 플레이어가 나이프 투척 스킬을 사용할 때 호출
    {
        StartCoroutine(KnifeCoolTimeActive(coolTime));
    }
    public void TeleAtkOn() // Enemy가 나이프에 맞았을 때 호출
    {
        teleCoroutine = StartCoroutine(TeleAtkCoolTimeActive(5f));
    }
    private IEnumerator KnifeCoolTimeActive(float coolTime) // 나이프 쿨타임
    {
        float elapsedTime = 0;
        coolTimeText.color = new Color(255, 255, 255, 255);
        // 슬라이더 최댓값 설정
        skillSlider.maxValue = coolTime;

        while (elapsedTime < coolTime)
        {
            elapsedTime += Time.deltaTime;
            coolTimeText.text = ((int)coolTime - (int)elapsedTime).ToString();
            skillSlider.value = coolTime - elapsedTime; // 슬라이더 값 설정
            yield return null;
        }
        // 재사용 대기시간이 끝나면 슬라이더 초기화
        coolTimeText.color = new Color(255, 255, 255 ,0);
        skillSlider.value = 0;
    }
    private IEnumerator TeleAtkCoolTimeActive(float coolTime) // 텔레포트 어택 쿨타임 및 이미지 변경
    {
        float elapsedTime = 0;
        // 슬라이더 최댓값 설정
        teleAtkSlider.maxValue = coolTime;
        skill_Image[1].color = new Color(255, 255, 255, 255);
        while (elapsedTime < coolTime)
        {
            elapsedTime += Time.deltaTime;
            teleAtkSlider.value = coolTime - elapsedTime; // 슬라이더 값 설정
            yield return null;
        }
        skill_Image[1].color = new Color(255, 255, 255, 0);
        // 재사용 대기시간이 끝나면 슬라이더 초기화
        teleAtkSlider.value = 0;
    }
    public void TeleAtkEnd() // 텔레포트 공격을 누른다면 텔레포트 어택 코루틴 중지
    {
        skill_Image[1].color = new Color(255, 255, 255, 0);
        teleAtkSlider.value = 0;
        StopCoroutine(teleCoroutine);
        teleCoroutine = null;

    }
}
