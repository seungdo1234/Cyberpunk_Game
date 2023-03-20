using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // 공격 딜레이
    private bool isThrowing = false; // 투척 딜레이
    private bool ThrowSkill = false; // 스킬 On
    public Transform pos;  // 히팅 박스 위치
    public Vector2 boxSize; // 박스 크기
  
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Transform playerPos;
    [SerializeField]
    private Background_Scroller background_Scroller;
    [SerializeField]
    private Transform throwPos; // 발사체 생성 위치
    [SerializeField]
    private GameObject knifePrefab; //  발사체 프리팹
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
            // 점프
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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

                // Flip시 좌우로 히팅 박스 위치 변경 
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
                StartCoroutine(EnemyAttack(0.8f, 1)); // Enemy공격

                StartCoroutine(ComboAtk2(0.8f)); // 콤보어택 시작
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
        while (atkDelay >= 0) // 공격 딜레이 안에 공격키를 누를 시 콤보 어택 발동
        {
            yield return null; // 리턴을 반복문 아래에 두면 첫 프레임에 공격 키가 인식이 됌 
          //  Debug.Log(atkDelay);
            if (combo == 1 && Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 1
            {
                anim.SetTrigger("ComboAtk2");
                atkDelay = 0.8f;
                StartCoroutine(EnemyAttack(0.8f,1));
                combo++;
            }
            else if (combo == 2 && Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 2
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
        Debug.Log("스킬 ON");
        ThrowSkill = false;
    }
    private IEnumerator EnemyAttack(float delay,int damage) 
    {
        yield return new WaitForSeconds(delay); // EnemyHP -- 하는 타이밍을 맞추기 위해 공격 딜레이 만큼 기다림  
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<Enemy>().OnDamaged(damage);
            }
        }
    }

    private void OnDrawGizmos() // 공격 범위는 눈에 보이지 않기 때문에 기즈모를 활용하여 그림-
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    // 연속적 입력은 FixedUpdate
    void FixedUpdate()
    {
        if (isAttacking == false && isThrowing == false)
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
                if (!anim.GetBool("isJumpping")) // 점프를 누르지 않고 떨어질 때 점프 모션이 나오게 함
                {
                    anim.SetBool("isJumpping", true);
                }
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
