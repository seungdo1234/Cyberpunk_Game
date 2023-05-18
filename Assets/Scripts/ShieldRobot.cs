using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRobot : MonoBehaviour
{
    public float speed;
    public float damage;
    [SerializeField]
    private Transform attackLayerPoint; // ���� ���̾� ����Ʈ
   
    private bool isAttacking = false; // ���� ��
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Player player; // �÷��̾� ����
    private bool isDetecting; // Player Ž�� ����
    // Update is called once per frame

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
   
    private IEnumerator AttackMotion()
    {
        anim.SetBool("isWalking", false);
        isAttacking = true;
        anim.SetTrigger("isAttacking");
        yield return new WaitForSeconds(4f);
        isAttacking = false;
    }
    private void ShieldRobotAttack()  // �ִϸ��̼� Ʈ����
    {
        Debug.DrawRay(attackLayerPoint.position, Vector3.right * 5.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(attackLayerPoint.position, Vector3.right, 5f, LayerMask.GetMask("Player")); // ���� ���̾� 

        if(rayHit.collider != null)
        {
            rayHit.collider.GetComponent<Player>().OnDamaged(transform.position,damage);
        }
    }

    void Update()
    {
        float distanceX = player.transform.position.x - transform.position.x;
        float distanceY = player.transform.position.y - transform.position.y;

        if (!isAttacking)
        {
            if (Mathf.Abs(distanceY) < 2)
            {
                if (Mathf.Abs(distanceX) < 2.5)
                {
                    StartCoroutine(AttackMotion());
                }
                else if (distanceX < 10 && distanceX > 0) // �����ʿ� ���� ���
                {
                    anim.SetBool("isWalking", true);
                    if (spriteRenderer.flipX)
                    {
                        spriteRenderer.flipX = false;
                    }
                    rigid.velocity = new Vector2(speed, rigid.velocity.y);
                }
                else if (distanceX > -10 && distanceX <= 0) // ���ʿ� ���� ���
                {
                    anim.SetBool("isWalking", true);
                    if (!spriteRenderer.flipX)
                    {
                        spriteRenderer.flipX = true;
                    }
                    rigid.velocity = new Vector2(-speed, rigid.velocity.y);
                }
            }
            else
            {
                anim.SetBool("isWalking", false);
                rigid.velocity = new Vector2(0, rigid.velocity.y);

            }
        }

   //     Debug.Log(Mathf.Abs(distance));
    }
}
