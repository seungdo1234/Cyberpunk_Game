using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; // �ִ� ü��
    [SerializeField]
    private float currentHP; // ���� ü��
    [SerializeField]
    private int enemyType; // 0. �ͷ�, 1. ����
    [SerializeField]
    private GameObject knifeHitMarkPrefab; // ������ �¾��� �� ��Ÿ�� ǥ�� 5�ʵ� �����
    private bool isDie = false; // ���� ��� s isDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GameObject markClone;
    // �ܺ� Ŭ�������� Ȯ�� �� �� �ְ� ������Ƽ ���� (���ٽ�)
    public float MaxHp => maxHP;
    public float CuurentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; // ���� ü���� �ִ� ü�°� ��� ����
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
        // Tip. ���� ü���� damage ��ŭ �����ؼ� ���� ��Ȳ�� �� ���� Ÿ���� ������ ���ÿ� ������
        // enemy.OnDie() �Լ��� ���� �� ����� �� �ִ�,

        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 �������� �ʴ´�.
        if (isDie == true) return;

        // ���� ü���� damage��ŭ ����
        currentHP -= damage;
        Debug.Log(currentHP);
        // HitAlphaAnimation() : ���� ������ ���ҽ�Ű�� �ڷ�ƾ
        // �ش� �ڷ�ƾ�� �̹� ���� �� �϶��� ����Ͽ� stop�� �� start��
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
            // �� ĳ���� ���
            //enemy.OnDie(EnemyDestroyType.kill);
        }
    }
    // ���� ���� �������� ����ȭ�ϴ� �ڷ�ƾ
    // ���� ������ �� ������ 40%�� ������ �� 0.05�� �ڿ� 100%�� �ٲ���
    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        // ���� ������ 40%�� ����
        color.a = 0.5f;
        spriteRenderer.color = color;

        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.3f);

        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
    public void EnemyDie() // �ִϸ��̼� Ʈ����
    {
        Destroy(gameObject);
    }
}
