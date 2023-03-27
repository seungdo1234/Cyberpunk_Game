using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private bool isAttacking = false; // 공격 딜레이
    private bool isThrowing = false; // 투척 딜레이
    private bool throwSkill = false; // 스킬 On
    private bool teleportAttack = false; // 텔레포트 공격 모션 중
    public Transform[] pos;  // 히팅 박스 위치
    public Vector2[] boxSize; // 박스 크기
  
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Enemy enemy;
    private Transform playerPos;
    [SerializeField]
    private Background_Scroller background_Scroller;
    [SerializeField]
    private Transform throwPos; // 발사체 생성 위치
    [SerializeField]
    private GameObject knifePrefab; //  발사체 프리팹
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
            // 점프
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
                Debug.Log("점프");
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
                // Flip시 좌우로 히팅 박스, 나이프 생성 위치 변경 
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
                StartCoroutine(ComboAtk2(.9f)); // 콤보어택 시작
            }
            else if ( throwSkill == false && Input.GetKeyDown(KeyCode.Z)) // ThorwKnife
            {
                StartCoroutine(IsThrowing()); // 던지기
                StartCoroutine(ThrowKnifeCoolTime());  // 쿨타임
            }
        }
        //Debug.Log(ThrowSkill);
    }
    private void X_Flip()
    {
        if (spriteRenderer.flipX == true)
        {
            throwThings.ThrowTo(-1f); // 나이프 이동 방향
            throwPos.position = new Vector3(playerPos.position.x - 0.7f, playerPos.position.y + 0.1f, 0); // 나이프 스폰 포인트 
            pos[0].position = new Vector3(playerPos.position.x - 1.1f, playerPos.position.y, 0); // 히팅 박스
        }
        else
        {
            throwThings.ThrowTo(1f);
            throwPos.position = new Vector3(playerPos.position.x + 0.7f, playerPos.position.y + 0.1f, 0);
            pos[0].position = new Vector3(playerPos.position.x + 1.1f, playerPos.position.y, 0);
        }
    }
    private IEnumerator ComboAtk2(float atkDelay) // 콤보어택
    {
        int combo = 1, animPlay = 1;
        isAttacking = true;
        while (atkDelay >= 0) // 공격 딜레이 안에 공격키를 누를 시 콤보 어택 발동
        {
            yield return null; // 리턴을 반복문 아래에 두면 첫 프레임에 공격 키가 인식이 됌 
                               //  Debug.Log(atkDelay);
            if (combo ==1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk1"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 1
                {
                    anim.SetTrigger("ComboAtk2");
   //                 atkDelay = 0.5f;
                    combo++;
                }
            }
            else if(combo == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 2
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
    private IEnumerator ThrowKnifeCoolTime() // Throw Skill 쿨타임
    {
        float ThrowCoolTime = 5f;
        while (ThrowCoolTime >= 0) // 쿨타임
        {
         //   Debug.Log(ThrowCoolTime);
            ThrowCoolTime -= Time.deltaTime;
            yield return null;
        }
        throwSkill = false;
        Debug.Log("스킬 ON");
    }
    private IEnumerator IsThrowing()// 던지기
    {
        throwSkill = true; // 스킬 On Off
        anim.SetTrigger("ThrowKnife");
        if (anim.GetBool("isJumpping")) // 떨어질 때 나이프를 던질 시 점프 애니메이션이 계속 나오는 것 수정
        {
            anim.SetBool("isJumpping", false);
        }
        isThrowing = true; // 던지고 있는 중
        GameObject clone = Instantiate(knifePrefab, throwPos.position, Quaternion.identity);
        clone.GetComponent<ThrowThings>().Throw(20f);
        yield return new WaitForSeconds(.5f);
        isThrowing = false; // 던지는 모션 끝

    }
    public void SpecialAttack(Enemy enemy, int enemyMoveDirection) // Enemy가 나이프에 맞았을 때
    {
        this.enemy = enemy;
        //  anim.SetTrigger("TeleportAttack");
        Debug.Log("나이프 맞춤");
         StartCoroutine(TeleportAttack(this.enemy, enemyMoveDirection));
        this.enemy = null;
    }
    private IEnumerator TeleportAttack(Enemy enemy, int enemyMoveDirection)
    {
        Debug.Log("코루틴");
        teleportAttack = true;
        anim.SetBool("KnifeHit", true);
        yield return new WaitForSeconds(.5f);
        // 텔레포트하는 위치 설정
        if (enemyMoveDirection == 1)
        {
            if (spriteRenderer.flipX) // 왼쪽을 바라보고 있다면
            {
                spriteRenderer.flipX = false;
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // 오른쪽을 바라보고 있다면
            {
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == -1)
        {
            if (spriteRenderer.flipX) // 왼쪽을 바라보고 있다면
            {
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // 오른쪽을 바라보고 있다면
            {
                spriteRenderer.flipX = true;
                playerPos.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == 0)
        {
            if (spriteRenderer.flipX) // 왼쪽을 바라보고 있다면
            {
                spriteRenderer.flipX = false;
                playerPos.position = new Vector3(enemy.transform.position.x - .3f, enemy.transform.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // 오른쪽을 바라보고 있다면
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
    private void EnemyAtk(int damage) // 애니메이션 트리거 활용
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
    private void OnDrawGizmos() // 공격 범위는 눈에 보이지 않기 때문에 기즈모를 활용하여 그림-
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos[0].position, boxSize[0]);
    }
    // 연속적 입력은 FixedUpdate
    void FixedUpdate()
    {
        if (isAttacking == false && isThrowing == false && teleportAttack == false)
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
            
        }
    }
}
