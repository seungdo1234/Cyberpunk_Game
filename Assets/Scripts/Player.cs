using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // ���� ������
    public Transform pos;  // ���� �ڽ� ��ġ
    public Vector2 boxSize; // �ڽ� ũ��

    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [SerializeField]
    private Background_Scroller background_Scroller;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ����
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //    anim.SetBool("isRunning", false);
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

        if (isAttacking == false && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isAttacking = true;
            anim.SetTrigger("isAttacking");
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize,0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<Enemy>().OnDamaged(1);
                }
            }
            StartCoroutine(AttackDelay());
        }
    }
    private void OnDrawGizmos() // ���� ������ ���� ������ �ʱ� ������ ����� Ȱ���Ͽ� �׸�-
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    private IEnumerator AttackDelay() // ���� �����̸� ���� �ڷ�ƾ (Update���� �� ��� ������ ��� ����)
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }
    // ������ �Է��� FixedUpdate
    void FixedUpdate()
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
