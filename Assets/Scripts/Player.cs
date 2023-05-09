using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float clingingSpeed;
    private int throwDirc = 1; // 나이프 던지는 방향
    private bool isAttacking = false; // 공격 딜레이
    private bool isThrowing = false; // 투척 딜레이
    private bool throwSkill = false; // 스킬 On
    private bool isDamaged = false; // 피격 시
    private bool isClinging = false; // 매달려 있을 때
    private bool teleportAttack = false; // 텔레포트 공격 모션 중
    private bool isJumpAttacking = false; // 점프 공격 중
    public bool isCutScenePlaying; // 컷신 플레이 중 일때 조작이 안되게 함
    
    private int jumpNum = 0; // 점프 횟수 (최대 2 => 더블점프)
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
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        playerPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking == false && isThrowing == false && teleportAttack == false && isClinging == false && isCutScenePlaying== false)
        {
            // 점프
            if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumpping", true);
                jumpNum++;
                StartCoroutine(DoubleJump());
            }
            if (anim.GetBool("isJumpping")) // 점프 중일 떄
            {
                if (isJumpAttacking == false && Input.GetKeyDown(KeyCode.LeftControl)) // 점프공겨
                {
                    anim.SetTrigger("IsJumpAttack");
                    StartCoroutine(JumpAttack());
                }
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
                if (isJumpAttacking == false) // 점프공격 중일때는 플립 X 
                {
                    spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
                    X_Flip();     // Flip시 좌우로 히팅 박스, 나이프 생성 위치 변경 
                }
            }

            if (Mathf.Abs(rigid.velocity.x) < 0.4)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                //    Debug.Log(rigid.velocity);
                anim.SetBool("isRunning", true);
            }
            // animation


            if (!anim.GetBool("isJumpping") && isThrowing == false && Input.GetKeyDown(KeyCode.LeftControl)) // Attack
            {
                anim.SetTrigger("ComboAtk1");
                StartCoroutine(ComboAtk2(.725f)); // 콤보어택 시작
            }
            else if (throwSkill == false && Input.GetKeyDown(KeyCode.Z)) // ThorwKnife
            {
                StartCoroutine(IsThrowing()); // 던지기
                StartCoroutine(ThrowKnifeCoolTime());  // 쿨타임
            }

        }
        //Debug.Log(ThrowSkill);
    }
    private void X_Flip() // 히팅 박스, ThrowPoint 플립
    {
        if (spriteRenderer.flipX == true)
        {
            throwDirc = -1; // 나이프 이동 방향
            throwPos.position = new Vector3(playerPos.position.x - 1f, playerPos.position.y + 0.1f, 0); // 나이프 스폰 포인트 
            pos[0].position = new Vector3(playerPos.position.x - 1.1f, playerPos.position.y, 0); // 히팅 박스
        }
        else
        {
            throwDirc = 1;
            throwPos.position = new Vector3(playerPos.position.x + 1f, playerPos.position.y + 0.1f, 0);
            pos[0].position = new Vector3(playerPos.position.x + 1.1f, playerPos.position.y, 0);
        }
    }
    private IEnumerator DoubleJump()
    {
        while (true)
        {
            // Debug.Log(anim.GetBool("isJumpping"));
            if (!anim.GetBool("isJumpping"))
            {
                break;
            }
            yield return null;
            if (isJumpAttacking == false && jumpNum == 1 && Input.GetKeyDown(KeyCode.Space)) // 더블점프
            {

                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("IsJumpDown", true);
                jumpNum++;
            }
        }
        jumpNum = 0; // 점프 횟수 초기화
    }
    private IEnumerator ComboAtk2(float atkDelay) // 콤보어택
    {
        // combo : 현재 진행중인 콤보
        // animPlay : 현재 진행중인 애니메이션
        // attackMove : 공격 중 움직임
        int combo = 1, animPlay = 1, attackMove = 0; // 딱 한번만 누르기 위한 변수들
        isAttacking = true;
        while (atkDelay >= 0) // 공격 딜레이 안에 공격키를 누를 시 콤보 어택 발동
        {
            if (attackMove == 0 && animPlay > 1) // 공격 중 방향키 누를 시 이동
            {
                if (!spriteRenderer.flipX)
                {
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        attackMove = 1;
                        rigid.AddForce(new Vector2(1, 0) * 2.5f, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        attackMove = 1;
                        rigid.AddForce(new Vector2(-1, 0) * 2.5f, ForceMode2D.Impulse);
                    }
                }
            }
            yield return null; // 리턴을 반복문 아래에 두면 첫 프레임에 공격 키가 인식이 됌 
            if (combo == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk1"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 1
                {
                    anim.SetTrigger("ComboAtk2");
                    //                 atkDelay = 0.5f;
                    combo++;
                }
            }
            else if (combo == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // 콤보 2
                {
                    anim.SetTrigger("ComboAtk3");
                    //                 atkDelay = 1.084f;
                    combo = 0;
                }
            }
            if (animPlay == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2")) // 애니메이션이 끝날 때 AttackDelay 적용을 위함
            {
                animPlay++;
                attackMove = 0;
                atkDelay = 0.35f;
            }
            else if (animPlay == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk3"))
            {
                animPlay++;
                attackMove = 0;
                atkDelay = .725f;
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
        clone.GetComponent<ThrowThings>().Throw(20f, throwDirc);
        clone.GetComponent<Player>();
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
    private void Clinging(float h)
    {
        Debug.DrawRay(rigid.position, Vector3.right * h, new Color(0, 1, 0));
        // 빔에 맞은 오브젝트의 정보
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.right * h, 1, LayerMask.GetMask("Wall"));
        // 관통 x
        if (rayHit.collider != null)
        {
            // distance : Ray에 닿았을 때의 거리
            if (rayHit.distance < .5f)
            {
                anim.SetBool("isClinging", true);
                jumpNum = 1;
              //  StartCoroutine(IsClinging(h));
            }
        }
        else
        {
            anim.SetBool("isClinging", false);
        }

    }
    private IEnumerator IsClinging(float h)
    {
        isClinging = true;
        while (true)
        {
            if (!anim.GetBool("isClinging"))
            {
                break;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

                rigid.AddForce(new Vector2(-h,2) * 5, ForceMode2D.Impulse);
                anim.SetBool("isClinging", false);
                break;
            }
            yield return null;
        }
        isClinging = false;
    }
    private IEnumerator JumpAttack() // 점프 공격 코루틴
    {
        isJumpAttacking = true;
        float atkDelay = 1.111f;
        while (atkDelay >= 0)
        {
            yield return null;
            if (!anim.GetBool("isJumpping")) // 착지하면 해당 코루틴도 종료
            {
                break;
            }
            atkDelay -= Time.deltaTime;

        }
        isJumpAttacking = false;
    }
    public void OnDamaged(Vector2 targetPosition)
    {
        // 레이어를 PlayerDamaged로 바꿈
        gameObject.layer = 11;
        gameObject.tag = "PlayerDamage";
        // 피격시 플레이어 캐릭터의 투명도를 올림 Color (R,G,B,A)
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);

        // 피격시 뒤로 밀림
        // 플레이어의 x축 - 목표물의 x 축 했을 때 양수면 오른쪽 음수면 왼쪽으로 밀림
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 0) * 5, ForceMode2D.Impulse);


        StartCoroutine(DamagedDelay());
        // 2초 뒤 무적 해제
        Invoke("OffDamaged", 1);
    }
    private IEnumerator DamagedDelay()
    {
        isDamaged = true;
        anim.SetBool("isDamaged", true);
        yield return new WaitForSeconds(.5f);
        isDamaged = false;
        anim.SetBool("isDamaged", false);
    }
    void OffDamaged()
    {
        // 레이어를 Player로 바꿈
        gameObject.layer = 3;
        gameObject.tag = "Player";
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private void EnemyAtk(int damage) // 애니메이션 트리거 활용
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos[0].position, boxSize[0], 0); 
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                Debug.Log("공격");
                collider.GetComponent<EnemyHP>().TakeDamage(damage);
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

        if (isAttacking == false && isThrowing == false && teleportAttack == false && isDamaged == false && isCutScenePlaying == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (anim.GetBool("isJumpping") && h != 0) // 점프 중일때 벽타기 인식
            {
                Clinging(h);
            }
            else // 벽안타고 내려올때
            {
                anim.SetBool("isClinging", false);
            }

            if (anim.GetBool("isClinging"))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * clingingSpeed);
            }
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
                    if (rayHit.distance < 1.0f) // 땅에 착지 했을 때
                    {
                        jumpNum = 0; // 점프 횟수 초기화 
                        // 점프 애니메이션 초기화
                        anim.SetBool("isJumpping", false);
                        anim.SetBool("IsJumpDown", false);
                        anim.SetBool("isClinging", false);
                    }
                }
            }

        }
    }
}
