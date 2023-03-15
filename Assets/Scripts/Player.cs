using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // 공격 딜레이
    public Transform pos;  // 히팅 박스 위치
    public Vector2 boxSize; // 박스 크기

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
        // 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //    anim.SetBool("isRunning", false);
            anim.SetBool("isJumpping", true);
        }


        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            // normalized : 단위 벡터를 -1,1로 만들어줌
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
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
    private void OnDrawGizmos() // 공격 범위는 눈에 보이지 않기 때문에 기즈모를 활용하여 그림-
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    private IEnumerator AttackDelay() // 공격 딜레이를 위한 코루틴 (Update문에 쓸 경우 프레임 드랍 유발)
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }
    // 연속적 입력은 FixedUpdate
    void FixedUpdate()
    {
        // 이동
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // Right MaxSpeed
        {
            background_Scroller.BG_Scroll(0.1f); // 오른쪽으로 배경 스크롤
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left MaxSpeed
        {
            background_Scroller.BG_Scroll(-0.1f); // 왼쪽으로 배경 스크롤
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        // # 점프 착지
        // RayCast : 오브젝트 검색을 위해 Ray를 쏘는 방식
        // LayerMask : 물리 효과를 구분하는 정수 값 
        // 빔을 밑으로 한칸 쏨
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // 빔에 맞은 오브젝트의 정보
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // 관통 x
            if (rayHit.collider != null)
            {
                // distance : Ray에 닿았을 때의 거리
                if (rayHit.distance < 1.0f)
                {
                    anim.SetBool("isJumpping", false);
                    Debug.Log("땅에 닿음");
                }
            }
        }
    }

}
