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
        // 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
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
        // 빔에 맞은 오브젝트의 정보
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
        // 관통 x
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    //재귀 함수
    void Think()
    {
        // Random.Range(최소 ~ 최대 범위의 랜덤 수 생성) -> 최대는 제외
        nextMove = Random.Range(-1, 2);

        // Run 애니메이션 
        anim.SetInteger("Enemy_Walk", nextMove);


        // 방향 전환
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;
        }

        // 생각하는 시간 랜덤 2 ~ 4초
        float nextThinkTime = Random.Range(2, 5);
        Invoke("Think", nextThinkTime);
    }
    void Turn()
    {
        nextMove *= -1; // 1일때 -1, -1일때 1
        spriteRenderer.flipX = nextMove == -1;
        // 현재 작동 중인 모든 Invoke 함수를 멈추는 함수
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
