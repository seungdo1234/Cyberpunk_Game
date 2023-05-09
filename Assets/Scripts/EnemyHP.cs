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
    private bool isDie = false; // ���� ��� s isDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    // �ܺ� Ŭ�������� Ȯ�� �� �� �ְ� ������Ƽ ���� (���ٽ�)
    public float MaxHp => maxHP;
    public float CuurentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; // ���� ü���� �ִ� ü�°� ��� ����
        //enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    public void TakeDamage(int damage)
    {
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
            anim.SetTrigger("Enemy_Exp");
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
    public void EnemyDie()
    {
        Destroy(gameObject);
    }
}
