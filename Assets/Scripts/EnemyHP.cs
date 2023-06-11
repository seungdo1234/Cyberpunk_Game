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
    private int enemyType; // 0. �ͷ�, 1. ���� 5. ����
    [SerializeField]
    private GameObject knifeHitMarkPrefab; // ������ �¾��� �� ��Ÿ�� ǥ�� 5�ʵ� �����
    private bool isDie = false; // ���� ��� s isDie�� true�� ����
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GameObject markClone;

    private Odium odium;
    private bool odiumPage2;
    // �ܺ� Ŭ�������� Ȯ�� �� �� �ְ� ������Ƽ ���� (���ٽ�)
    public float MaxHp => maxHP;
    public float CuurentHP => currentHP;

    private void Awake()
    {
        if(enemyType == 5)
        {
            odium = GetComponent<Odium>();
        }
        //currentHP = maxHP; // ���� ü���� �ִ� ü�°� ��� ����
        currentHP = 1; // ���� ü���� �ִ� ü�°� ��� ����
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
        if (isDie == true || (enemyType == 5 && odium.pageTranform == true)) return;
        // ���� ü���� damage��ŭ ����
        currentHP -= damage;
        Debug.Log(currentHP);
        // HitAlphaAnimation() : ���� ������ ���ҽ�Ű�� �ڷ�ƾ
        // �ش� �ڷ�ƾ�� �̹� ���� �� �϶��� ����Ͽ� stop�� �� start��
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
            if(enemyType == 5)
            {
                if (!odiumPage2)
                {
                    odiumPage2 = true;
                    odium.OdiumPage2();
                }
            }
            else
            {
                isDie = true;
                gameObject.layer = 10;
                gameObject.tag = "EnemyDeath";
                if (enemyType == 0)
                {
                    anim.SetTrigger("Enemy_Exp");
                }
                else if (enemyType == 1)
                {
                    anim.SetTrigger("ShieldRobotDeath");
                }
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
    private IEnumerator HP_Recovery(float hp)
    {
        float currentTime = 0f;
        while (currentTime < 2f)
        {
            currentTime += Time.deltaTime;
            float hpValue = Mathf.Lerp(0, hp, currentTime / 2f);
            currentHP = hpValue;
            yield return null;
        }
    }
    public void OdiumRecoveryHP()
    {
        float hp = Mathf.Floor(maxHP / 2);
        StartCoroutine(HP_Recovery(hp));
    }
    public void EnemyDie() // �ִϸ��̼� Ʈ����
    {
        Destroy(gameObject);
    }
}
