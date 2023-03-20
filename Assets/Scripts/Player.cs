using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // ���� ������
    private bool isThrowing = false; // ��ô ������
    private bool ThrowSkill = false; // ��ų On
    public Transform pos;  // ���� �ڽ� ��ġ
    public Vector2 boxSize; // �ڽ� ũ��
  
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Transform playerPos;
    [SerializeField]
    private Background_Scroller background_Scroller;
    [SerializeField]
    private Transform throwPos; // �߻�ü ���� ��ġ
    [SerializeField]
    private GameObject knifePrefab; //  �߻�ü ������
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        playerPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && isThrowing == false)
        {
            // ����
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
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

                // Flip�� �¿�� ���� �ڽ� ��ġ ���� 
                if (spriteRenderer.flipX == true)
                {
                    pos.position = new Vector3(playerPos.position.x - 1.1f , playerPos.position.y , 0);
                }
                else
                {
                    pos.position = new Vector3(playerPos.position.x + 1.1f, playerPos.position.y , 0);
                }
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
                StartCoroutine(EnemyAttack(0.8f, 1)); // Enemy����

                StartCoroutine(ComboAtk2(0.8f)); // �޺����� ����
            }
            else if (ThrowSkill == false && Input.GetKeyDown(KeyCode.Z)) // ThorwKnife
            {
                StartCoroutine(ThrowKnife());
            }
        }
    }
    private IEnumerator ComboAtk2(float atkDelay)
    {
        int combo = 1;
        isAttacking = true;
        while (atkDelay >= 0) // ���� ������ �ȿ� ����Ű�� ���� �� �޺� ���� �ߵ�
        {
            yield return null; // ������ �ݺ��� �Ʒ��� �θ� ù �����ӿ� ���� Ű�� �ν��� �� 
          //  Debug.Log(atkDelay);
            if (combo == 1 && Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 1
            {
                anim.SetTrigger("ComboAtk2");
                atkDelay = 0.8f;
                StartCoroutine(EnemyAttack(0.8f,1));
                combo++;
            }
            else if (combo == 2 && Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 2
            {
                anim.SetTrigger("ComboAtk3");
                StartCoroutine(EnemyAttack(1.2f,3));
                atkDelay = 1.4f;
                combo = 0;
            }
            atkDelay -= Time.deltaTime;
        }
        isAttacking = false;
    }
    private IEnumerator ThrowKnife()
    {
        float ThrowCoolTime = 5f;
        ThrowSkill = true;
        isThrowing = true;
        anim.SetTrigger("ThrowKnife");
        GameObject clone = Instantiate(knifePrefab, throwPos.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        isThrowing = false;
        while (ThrowCoolTime >= 0)
        {
            ThrowCoolTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("��ų ON");
        ThrowSkill = false;
    }
    private IEnumerator EnemyAttack(float delay,int damage) 
    {
        yield return new WaitForSeconds(delay); // EnemyHP -- �ϴ� Ÿ�̹��� ���߱� ���� ���� ������ ��ŭ ��ٸ�  
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<Enemy>().OnDamaged(damage);
            }
        }
    }

    private void OnDrawGizmos() // ���� ������ ���� ������ �ʱ� ������ ����� Ȱ���Ͽ� �׸�-
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    // ������ �Է��� FixedUpdate
    void FixedUpdate()
    {
        if (isAttacking == false && isThrowing == false)
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
            else if(rigid.velocity.y == 0)
            {
                if (anim.GetBool("isJumpping"))
                {
                    anim.SetBool("isJumpping", false);
                }
            }
        }
    }
}
