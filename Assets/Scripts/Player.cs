using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // ���� ������
    private bool isThrowing = false; // ��ô ������
    private bool throwSkill = false; // ��ų On
    private bool teleportAttack = false; // �ڷ���Ʈ ���� ��� ��
    public Transform[] pos;  // ���� �ڽ� ��ġ
    public Vector2[] boxSize; // �ڽ� ũ��
  
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Enemy enemy;
    private Transform playerPos;
    [SerializeField]
    private Background_Scroller background_Scroller;
    [SerializeField]
    private Transform throwPos; // �߻�ü ���� ��ġ
    [SerializeField]
    private GameObject knifePrefab; //  �߻�ü ������
    [SerializeField]
    private ThrowThings throwThings;
    // Start is called before the first frame update
    void Awake()
    {
        throwThings.ThrowTo(1f);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        playerPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && isThrowing == false && teleportAttack == false)
        {
            // ����
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
                Debug.Log("����");
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumpping", true);
            }


            // Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {
                // normalized : ���� ���͸� -1,1�� �������
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }
            // ���� ��ȯ
            if (Input.GetButton("Horizontal"))
            {
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
                // Flip�� �¿�� ���� �ڽ�, ������ ���� ��ġ ���� 
                X_Flip();
            }

            // animation
            if (Mathf.Abs(rigid.velocity.x) < 0.4)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }

            if (!anim.GetBool("isJumpping") && isThrowing == false && Input.GetKeyDown(KeyCode.LeftControl)) // Attack
            {
                anim.SetTrigger("ComboAtk1");
                StartCoroutine(ComboAtk2(.9f)); // �޺����� ����
            }
            else if ( throwSkill == false && Input.GetKeyDown(KeyCode.Z)) // ThorwKnife
            {
                StartCoroutine(IsThrowing()); // ������
                StartCoroutine(ThrowKnifeCoolTime());  // ��Ÿ��
            }
        }
        //Debug.Log(ThrowSkill);
    }
    private void X_Flip()
    {
        if (spriteRenderer.flipX == true)
        {
            throwThings.ThrowTo(-1f); // ������ �̵� ����
            throwPos.position = new Vector3(playerPos.position.x - 0.7f, playerPos.position.y + 0.1f, 0); // ������ ���� ����Ʈ 
            pos[0].position = new Vector3(playerPos.position.x - 1.1f, playerPos.position.y, 0); // ���� �ڽ�
        }
        else
        {
            throwThings.ThrowTo(1f);
            throwPos.position = new Vector3(playerPos.position.x + 0.7f, playerPos.position.y + 0.1f, 0);
            pos[0].position = new Vector3(playerPos.position.x + 1.1f, playerPos.position.y, 0);
        }
    }
    private IEnumerator ComboAtk2(float atkDelay) // �޺�����
    {
        int combo = 1, animPlay = 1;
        isAttacking = true;
        while (atkDelay >= 0) // ���� ������ �ȿ� ����Ű�� ���� �� �޺� ���� �ߵ�
        {
            yield return null; // ������ �ݺ��� �Ʒ��� �θ� ù �����ӿ� ���� Ű�� �ν��� �� 
                               //  Debug.Log(atkDelay);
            if (combo ==1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk1"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 1
                {
                    anim.SetTrigger("ComboAtk2");
   //                 atkDelay = 0.5f;
                    combo++;
                }
            }
            else if(combo == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 2
                {
                    anim.SetTrigger("ComboAtk3");
   //                 atkDelay = 1.084f;
                    combo = 0;
                }
            }
            if (animPlay == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2"))
            {
                animPlay++;
                atkDelay = 0.4f;
                Debug.Log(atkDelay);
            }
            else if (animPlay == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk3"))
            {
                animPlay = 0;
                atkDelay = .9f;
                Debug.Log(atkDelay);
            }
            atkDelay -= Time.deltaTime;
        }
        isAttacking = false;
    }
    private IEnumerator ThrowKnifeCoolTime() // Throw Skill ��Ÿ��
    {
        float ThrowCoolTime = 5f;
        while (ThrowCoolTime >= 0) // ��Ÿ��
        {
         //   Debug.Log(ThrowCoolTime);
            ThrowCoolTime -= Time.deltaTime;
            yield return null;
        }
        throwSkill = false;
        Debug.Log("��ų ON");
    }
    private IEnumerator IsThrowing()// ������
    {
        throwSkill = true; // ��ų On Off
        anim.SetTrigger("ThrowKnife");
        if (anim.GetBool("isJumpping")) // ������ �� �������� ���� �� ���� �ִϸ��̼��� ��� ������ �� ����
        {
            anim.SetBool("isJumpping", false);
        }
        isThrowing = true; // ������ �ִ� ��
        GameObject clone = Instantiate(knifePrefab, throwPos.position, Quaternion.identity);
        clone.GetComponent<ThrowThings>().Throw(20f);
        yield return new WaitForSeconds(.5f);
        isThrowing = false; // ������ ��� ��

    }
    public void SpecialAttack(Enemy enemy, int enemyMoveDirection) // Enemy�� �������� �¾��� ��
    {
        this.enemy = enemy;
        //  anim.SetTrigger("TeleportAttack");
        Debug.Log("������ ����");
         StartCoroutine(TeleportAttack(this.enemy, enemyMoveDirection));
        this.enemy = null;
    }
    private IEnumerator TeleportAttack(Enemy enemy, int enemyMoveDirection)
    {
        Debug.Log("�ڷ�ƾ");
        teleportAttack = true;
        anim.SetBool("KnifeHit", true);
        yield return new WaitForSeconds(.5f);
        // �ڷ���Ʈ�ϴ� ��ġ ����
        if (enemyMoveDirection == 1)
        {
            if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = false;
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == -1)
        {
            if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
            {
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = true;
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == 0)
        {
            if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = false;
                playerPos.position = new Vector3(enemy.transform.position.x - .3f, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = true;
                playerPos.position = new Vector3(enemy.transform.position.x + .3f, enemy.transform.position.y + 2f, 0);
            }
        }
        X_Flip();
        rigid.gravityScale = .25f;
        yield return new WaitForSeconds(.9f);
        anim.SetBool("KnifeHit", false);
        teleportAttack = false;
        rigid.gravityScale = 1f;

    }
    private void EnemyAtk(int damage) // �ִϸ��̼� Ʈ���� Ȱ��
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos[0].position, boxSize[0], 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<Enemy>().OnDamaged(damage, 1);
            }
        }
    }
    private void OnDrawGizmos() // ���� ������ ���� ������ �ʱ� ������ ����� Ȱ���Ͽ� �׸�-
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos[0].position, boxSize[0]);
    }
    // ������ �Է��� FixedUpdate
    void FixedUpdate()
    {
        if (isAttacking == false && isThrowing == false && teleportAttack == false)
        {
            // �̵�
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) // Right MaxSpeed
            {
                background_Scroller.BG_Scroll(0.1f); // ���������� ��� ��ũ��
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < maxSpeed * (-1)) // Left MaxSpeed
            {
                background_Scroller.BG_Scroll(-0.1f); // �������� ��� ��ũ��
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
            }

            // # ���� ����
            // RayCast : ������Ʈ �˻��� ���� Ray�� ��� ���
            // LayerMask : ���� ȿ���� �����ϴ� ���� �� 
            // ���� ������ ��ĭ ��
            if (rigid.velocity.y < 0)
            {
                if (!anim.GetBool("isJumpping")) // ������ ������ �ʰ� ������ �� ���� ����� ������ ��
                {
                    anim.SetBool("isJumpping", true);
                }
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
                // ���� ���� ������Ʈ�� ����
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
                // ���� x
                if (rayHit.collider != null)
                {
                    // distance : Ray�� ����� ���� �Ÿ�
                    if (rayHit.distance < 1.0f)
                    {
                        anim.SetBool("isJumpping", false);
                        Debug.Log("���� ����");
                    }
                }
            }
            
        }
    }
}
