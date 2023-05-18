using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; // 최대 체력
    [SerializeField]
    private float currentHP; // 현재 체력
    [SerializeField]
    private int enemyType; // 0. 터렛, 1. 솔져
    [SerializeField]
    private GameObject knifeHitMarkPrefab; // 나이프 맞았을 때 나타는 표식 5초뒤 사라짐
    private bool isDie = false; // 적이 사망 s isDie를 true로 설정
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GameObject markClone;
    // 외부 클래스에서 확인 할 수 있게 프로퍼티 생성 (람다식)
    public float MaxHp => maxHP;
    public float CuurentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; // 현재 체력을 최대 체력과 길게 설정
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    private IEnumerator MarkDuration(GameObject markClone)
    {
        yield return new WaitForSeconds(5f);
        Destroy(markClone);

    }
    private void SpawnMark()
    {
        markClone = Instantiate(knifeHitMarkPrefab);

        markClone.GetComponent<KnifeHitMark>().Setup(transform);

        StartCoroutine(MarkDuration(markClone));

    }
    public void TakeDamage(int damage, int hitType)
    {
        if (hitType == 2)
        {
            GameObject.FindWithTag("SkillUI").GetComponent<PlayerSkillUI>().TeleAtkOn();
            SpawnMark();
            GameObject.FindWithTag("Player").GetComponent<Player>().SpecialAttack(this, 0);
            if (enemyType == 0)
            {
                GameObject.FindWithTag("Player").GetComponent<Player>().SpecialAttack(this, 0);
            }
            else
            {
             //   GameObject.FindWithTag("Player").GetComponent<Player>().SpecialAttack(enemy, 0);
            }
        }
        if (damage == 5)
        {
            Destroy(markClone);
          //  GameObject.FindWithTag("SkillUI").GetComponent<PlayerSkillUI>().TeleAtkEnd();
        }
        // Tip. 적의 체력이 damage 만큼 감소해서 죽을 상황일 때 여러 타워의 공격을 동시에 받으면
        // enemy.OnDie() 함수가 여러 번 실행될 수 있다,

        // 현재 적의 상태가 사망 상태이면 아래 코드를 실행하지 않는다.
        if (isDie == true) return;

        // 현재 체력을 damage만큼 감소
        currentHP -= damage;
        Debug.Log(currentHP);
        // HitAlphaAnimation() : 적의 투명도를 감소시키는 코루틴
        // 해당 코루틴이 이미 실행 중 일때를 고려하여 stop한 후 start함
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
             isDie = true;
            if(enemyType == 0)
            {
                anim.SetTrigger("Enemy_Exp");
            }
            else if (enemyType == 1)
            {
                anim.SetTrigger("ShieldRobotDeath");
            }
            // 적 캐릭터 사망
            //enemy.OnDie(EnemyDestroyType.kill);
        }
    }
    // 적이 공격 당했음을 가시화하는 코루틴
    // 공격 당했을 때 투명도를 40%로 설정한 후 0.05초 뒤에 100%로 바꿔줌
    private IEnumerator HitAlphaAnimation()
    {
        // 현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;

        // 적의 투명도를 40%로 설정
        color.a = 0.5f;
        spriteRenderer.color = color;

        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.3f);

        // 적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
    public void EnemyDie() // 애니메이션 트리거
    {
        Destroy(gameObject);
    }
}
