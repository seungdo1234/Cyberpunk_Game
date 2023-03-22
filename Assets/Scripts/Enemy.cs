using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHP;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleColider;
    [SerializeField]
    Player player;
    public int nextMove;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        // �־��� �ð��� ���� ��, ������ �Լ��� �����ϴ� �Լ�
        Invoke("Think", 5.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        // ���� ���� ������Ʈ�� ����
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
        // ���� x
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    //��� �Լ�
    void Think()
    {
        // Random.Range(�ּ� ~ �ִ� ������ ���� �� ����) -> �ִ�� ����
        nextMove = Random.Range(-1, 2);

        // Run �ִϸ��̼� 
        anim.SetInteger("Enemy_Walk", nextMove);


        // ���� ��ȯ
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;
        }

        // �����ϴ� �ð� ���� 2 ~ 4��
        float nextThinkTime = Random.Range(2, 5);
        Invoke("Think", nextThinkTime);
    }
    void Turn()
    {
        nextMove *= -1; // 1�϶� -1, -1�϶� 1
        spriteRenderer.flipX = nextMove == -1;
        // ���� �۵� ���� ��� Invoke �Լ��� ���ߴ� �Լ�
        CancelInvoke();
        Invoke("Think", 3.0f);
    }
    public void OnDamaged(int damage, int attackType)
    {
        if(attackType == 2)
        {
           // player.EnemyInfo(this);
        }
        enemyHP -= damage;
    }
}
