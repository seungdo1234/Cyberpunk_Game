using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float downJumpPower;
    public float clingingSpeed;
    public float wallJumpSpeed;
    private int throwDirc = 1; // ������ ������ ����
    private bool isAttacking = false; // ���� ������
    private bool isThrowing = false; // ��ô ������
    private bool throwSkill = false; // ��ų On
    private bool isDamaged = false; // �ǰ� ��
    private bool isClinging = false; // �Ŵ޷� ���� ��
    private bool teleportAttack = false; // �ڷ���Ʈ ���� ��� ��
    private bool isJumpAttacking = false; // ���� ���� ��
    private bool leftControl = false; // �������� �����ð����� ���� Ű �Է� ����
    private bool rightControl = false; // �������� �����ð����� ������ Ű �Է� ����
    public bool isCutScenePlaying; // �ƽ� �÷��� �� �϶� ������ �ȵǰ� ��

    private int jumpNum = 0; // ���� Ƚ�� (�ִ� 2 => ��������)
    public Transform[] pos;  // ���� �ڽ� ��ġ
    public Vector2[] boxSize; // �ڽ� ũ��

    [Header("Teleport")]
    [SerializeField]
    private float radius; // �ڷ���Ʈ �� ������
    [SerializeField]
    private LayerMask crystalLayer; // �ڷ���Ʈ ��ü�� ũ����Ż ���̾� ����
    [SerializeField]
    private GameObject teleportCircle; // ����Ʈ ���� �� ������ �ڷ���Ʈ ���� ������Ʈ
    [SerializeField]
    private GameObject teleportTarget; // �ڷ���Ʈ ��� ũ����Ż ���� ������Ʈ
    [SerializeField]
    private Slider teleportSlider;  // �ڷ���Ʈ ������
    [SerializeField]
    private EmptyGuideAlpha emptyGuideAlpha; // �������� ���� �� ������ ����;
    private bool playingEmptyGuide; // EmptyGuideâ�� ������ �ִٸ� 

    private bool isTeleportReady; // ����Ʈ�� ������ �ִ���
    private bool teleportAnim; // �ڷ���Ʈ �ִϸ��̼��� ������ �ִ���

    private PlayerStat playerStat; // �÷��̾� ����
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleColider;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private EnemyHP enemy; // enemy ����
    private Transform playerPos;

    [Header("ThrowKnife")]
    [SerializeField]
    private Transform throwPos; // �߻�ü ���� ��ġ
    [SerializeField]
    private GameObject knifePrefab; //  �߻�ü ������
    [SerializeField]
    private ThrowThings throwThings; // �߻�ü �ø�

    [Header("PlayerSkill_UI")]
    [SerializeField]
    private PlayerSkillUI Skills_UI;

    // Start is called before the first frame update
    void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleColider = GetComponent<CapsuleCollider2D>();
        playerPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!teleportAnim && !isTeleportReady && isAttacking == false && isThrowing == false && teleportAttack == false && isClinging == false && isCutScenePlaying == false)
        {
            // �� ����
            if (!Input.GetKey(KeyCode.DownArrow) && Input.GetButtonDown("Jump") && !anim.GetBool("isJumpping"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumpping", true);
                jumpNum++;
                StartCoroutine(DoubleJump());
            }
            // �Ʒ� ����
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyUp(KeyCode.Space))
            {
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("JumpPlatform"));
                if (rayHit.collider != null)
                {
                    rayHit.collider.GetComponent<JumpPlatform>().PlayerDownJump();
                    rigid.AddForce(Vector2.down * downJumpPower, ForceMode2D.Impulse);
                }
            }

            if (anim.GetBool("isJumpping")) // ���� ���� ��
            {
                if (isJumpAttacking == false && Input.GetKeyDown(KeyCode.LeftControl)) // ��������
                {
                    anim.SetTrigger("IsJumpAttack");
                    StartCoroutine(JumpAttack());
                }
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
                if (isJumpAttacking == false) // �������� ���϶��� �ø� X 
                {
                    spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
                    X_Flip();     // Flip�� �¿�� ���� �ڽ�, ������ ���� ��ġ ���� 
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
                StartCoroutine(ComboAtk2(.725f)); // �޺����� ����
            }
            else if (throwSkill == false && Input.GetKeyDown(KeyCode.Z)) // ThorwKnife
            {
                StartCoroutine(IsThrowing()); // ������
                StartCoroutine(ThrowKnifeCoolTime());  // ��Ÿ��
            }
            if (teleportSlider.value >= 0.2f && !isTeleportReady && Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(TeleportOn());
            }
            else if(!playingEmptyGuide && teleportSlider.value < 0.2f && !isTeleportReady && Input.GetKey(KeyCode.LeftShift))
            {
                emptyGuideAlpha.EmptyGauge();
                StartCoroutine(PlayingEmptyGuide());
            }
        }
        if (isTeleportReady && !Input.GetKey(KeyCode.LeftShift))
        {
            isTeleportReady = false;
        }
      //  Debug.Log(teleportSlider.value);
    }
    private void X_Flip() // ���� �ڽ�, ThrowPoint �ø�
    {
        if (spriteRenderer.flipX == true)
        {
            throwDirc = -1; // ������ �̵� ����
            throwPos.position = new Vector3(playerPos.position.x - 1f, playerPos.position.y + 0.1f, 0); // ������ ���� ����Ʈ 
            pos[0].position = new Vector3(playerPos.position.x - 1.1f, playerPos.position.y, 0); // ���� �ڽ�
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
            if (isJumpAttacking == false && jumpNum == 1 && Input.GetKeyDown(KeyCode.Space)) // ��������
            {

                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("IsJumpDown", true);
                jumpNum++;
            }
        }
        jumpNum = 0; // ���� Ƚ�� �ʱ�ȭ
    }
    private IEnumerator ComboAtk2(float atkDelay) // �޺�����
    {
        // combo : ���� �������� �޺�
        // animPlay : ���� �������� �ִϸ��̼�
        // attackMove : ���� �� ������
        int combo = 1, animPlay = 1, attackMove = 0; // �� �ѹ��� ������ ���� ������
        isAttacking = true;
        while (atkDelay >= 0) // ���� ������ �ȿ� ����Ű�� ���� �� �޺� ���� �ߵ�
        {
            if (attackMove == 0 && animPlay > 1) // ���� �� ����Ű ���� �� �̵�
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
            yield return null; // ������ �ݺ��� �Ʒ��� �θ� ù �����ӿ� ���� Ű�� �ν��� �� 
            if (combo == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk1"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 1
                {
                    anim.SetTrigger("ComboAtk2");
                    //                 atkDelay = 0.5f;
                    combo++;
                }
            }
            else if (combo == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2"))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl)) // �޺� 2
                {
                    anim.SetTrigger("ComboAtk3");
                    //                 atkDelay = 1.084f;
                    combo = 0;
                }
            }
            if (animPlay == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Combo_Atk2")) // �ִϸ��̼��� ���� �� AttackDelay ������ ����
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
    private IEnumerator ThrowKnifeCoolTime() // Throw Skill ��Ÿ��
    {
        float ThrowCoolTime = 8f;
        Skills_UI.UsingSkill(8f);
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
        clone.GetComponent<ThrowThings>().Throw(20f, throwDirc);
        clone.GetComponent<Player>();
        yield return new WaitForSeconds(.5f);
        isThrowing = false; // ������ ��� ��

    }
    public void SpecialAttack(EnemyHP enemy, int enemyMoveDirection) // Enemy�� �������� �¾��� ��
    {
        Debug.Log(enemy.name);
        this.enemy = enemy;
        //  anim.SetTrigger("TeleportAttack");
        StartCoroutine(TeleportAttack(enemy, enemyMoveDirection));
        this.enemy = null;
    }
    private IEnumerator TeleportAttack(EnemyHP enemyPos, int enemyMoveDirection)
    {
        float atkDelay = 5f;
        bool keyEvent = false;
        while (atkDelay >= 0)
        {
            if (!keyEvent && !isAttacking && !isClinging && !isJumpAttacking && !isThrowing && Input.GetKeyDown(KeyCode.Z)) // ǥ���� ����� �ѹ��� z 
            {
                keyEvent = true;
                teleportAttack = true;
                anim.SetTrigger("TeleAtk");
                yield return new WaitForSeconds(.5f);
                if (enemyMoveDirection == 0)
                {
                    if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
                    {
                        spriteRenderer.flipX = false;
                        playerPos.position = new Vector3(enemyPos.transform.position.x - .3f, enemyPos.transform.position.y + 2f, 0);
                    }
                    else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
                    {
                        spriteRenderer.flipX = true;
                        playerPos.position = new Vector3(enemyPos.transform.position.x + .3f, enemyPos.transform.position.y + 2f, 0);
                    }
                }
                X_Flip();
                rigid.gravityScale = .25f;
                Skills_UI.TeleAtkEnd();
                rigid.velocity = new Vector2(0, 0); // �ӵ� �ʱ�ȭ
                anim.SetBool("isJumpping", false);
                yield return new WaitForSeconds(.9f);
                teleportAttack = false;
                rigid.gravityScale = 1f;
                break;
            }
            atkDelay -= Time.deltaTime;
            yield return null;
        }

        /*
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
                playerPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                playerPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == -1)
        {
            if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
            {
                playerPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = true;
                playerPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y + 2f, 0);
            }
        }
        else if (enemyMoveDirection == 0)
        {
            if (spriteRenderer.flipX) // ������ �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = false;
                playerPos.position = new Vector3(enemyPos.position.x - .3f, enemyPos.position.y + 2f, 0);
            }
            else if (!spriteRenderer.flipX) // �������� �ٶ󺸰� �ִٸ�
            {
                spriteRenderer.flipX = true;
                playerPos.position = new Vector3(enemyPos.position.x + .3f, enemyPos.position.y + 2f, 0);
            }
        }
        X_Flip();
        rigid.gravityScale = .25f;
        yield return new WaitForSeconds(.9f);
        anim.SetBool("KnifeHit", false);
        teleportAttack = false;
        rigid.gravityScale = 1f;
        */
    }
    private void Clinging(float h)
    {
        Debug.DrawRay(rigid.position, Vector3.right * h, new Color(0, 1, 0));
        // ���� ���� ������Ʈ�� ����
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.right * h, .5f, LayerMask.GetMask("Wall"));
        // ���� x
        if (rayHit.collider != null)
        {
            // distance : Ray�� ����� ���� �Ÿ�
            if (rayHit.distance < .5f)
            {
                anim.SetBool("isClinging", true);
                jumpNum = 1; // ���� �ʱ�ȭ
                StartCoroutine(IsClinging(h));
            }
        }
        else
        {
            anim.SetBool("isClinging", false);
        }

    }
    private IEnumerator IsClinging(float h) // �� ���� �ڷ�ƾ
    {
        isClinging = true;
        while (true)
        {
            if (!anim.GetBool("isClinging"))
            {
                break;
            }
            if (Input.GetKeyDown(KeyCode.Space)) // ����
            {
                StartCoroutine(ClingingKeyControl(h));
                rigid.velocity = new Vector2(wallJumpSpeed * -h, jumpPower); // ����
                anim.SetBool("isClinging", false);
                isClinging = false;
                break;
            }
            yield return null;
        }
        isClinging = false;
    }
    private IEnumerator ClingingKeyControl(float h) // �� ������ �� �����ð����� ���� �ݴ� ���� Ű ����
    {
        if (h > 0)
        {
            rightControl = true;
        }
        else
        {
            leftControl = true;
        }
        yield return new WaitForSeconds(.1f);
        if (h > 0)
        {
            rightControl = false;
        }
        else
        {
            leftControl = false;
        }
    }
    private IEnumerator JumpAttack() // ���� ���� �ڷ�ƾ
    {
        isJumpAttacking = true;
        float atkDelay = 1.111f;
        while (atkDelay >= 0)
        {
            yield return null;
            if (!anim.GetBool("isJumpping")) // �����ϸ� �ش� �ڷ�ƾ�� ����
            {
                break;
            }
            atkDelay -= Time.deltaTime;

        }
        isJumpAttacking = false;
    }
    public void OnDamaged(Vector2 targetPosition, float damage)
    {
        playerStat.TakeDamage(damage);
        // ���̾ PlayerDamaged�� �ٲ�
        gameObject.layer = 11;
        gameObject.tag = "PlayerDamage";
        // �ǰݽ� �÷��̾� ĳ������ ������ �ø� Color (R,G,B,A)
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);

        // �ǰݽ� �ڷ� �и�
        // �÷��̾��� x�� - ��ǥ���� x �� ���� �� ����� ������ ������ �������� �и�
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 0) * 5, ForceMode2D.Impulse);


        StartCoroutine(DamagedDelay());
        // 2�� �� ���� ����
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
        // ���̾ Player�� �ٲ�
        gameObject.layer = 3;
        gameObject.tag = "Player";
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    private void EnemyAtk(int damage) // �ִϸ��̼� Ʈ���� Ȱ��
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos[0].position, boxSize[0], 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Enemy")
            {
                Debug.Log("����");
                collider.GetComponent<EnemyHP>().TakeDamage(damage, 1);
            }
        }
    }
    private IEnumerator TeleportOn()
    {
        int targerNum = 0;

        teleportCircle.SetActive(true);
        isTeleportReady = true;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius, crystalLayer);
        Time.timeScale = 0f;
        if (collider2Ds.Length > 0)
        {
            teleportTarget.SetActive(true);
            while (isTeleportReady)
            {

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (targerNum == 0)
                    {
                        targerNum = collider2Ds.Length - 1;
                    }
                    else
                    {
                        targerNum -= 1;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (targerNum == collider2Ds.Length - 1)
                    {
                        targerNum = 0;
                    }
                    else
                    {
                        targerNum += 1;
                    }
                }
                teleportTarget.transform.position = collider2Ds[targerNum].transform.position;
                yield return null;
            }

            Time.timeScale = 1f;
            teleportSlider.value -= 0.2f;
            rigid.velocity = new Vector2(0, 0);
            rigid.gravityScale = .25f;
            teleportCircle.SetActive(false);
            teleportTarget.SetActive(false);
            teleportAnim = true;
            anim.SetTrigger("Teleport");
            yield return new WaitForSeconds(.55f);

            transform.position = collider2Ds[targerNum].transform.position;
            collider2Ds[targerNum].GetComponent<Crystal>().CrystalUse();
            rigid.velocity = new Vector2(0, 0);
            yield return new WaitForSeconds(.3f);
            rigid.gravityScale = 1f;
            teleportAnim = false;
            jumpNum = 1;
        }
        else
        {
            while (isTeleportReady)
            {
                yield return null;
            }
            Time.timeScale = 1f;
            teleportCircle.SetActive(false);
        }
    }
    private IEnumerator PlayingEmptyGuide()
    {
        playingEmptyGuide = true;
        yield return new WaitForSeconds(1.5f);
        playingEmptyGuide = false;
    }
    private void OnDrawGizmos() // ���� ������ ���� ������ �ʱ� ������ ����� Ȱ���Ͽ� �׸�-
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos[0].position, boxSize[0]);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    // ������ �Է��� FixedUpdate
    void FixedUpdate()
    {

        if (!teleportAnim && !isTeleportReady && isAttacking == false && isThrowing == false && teleportAttack == false && isDamaged == false && isCutScenePlaying == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            if ((leftControl && h == -1) || (rightControl && h == 1)) // Ű ���� �� �Է��� ������ h �� �ʱ�ȭ
            {
                h = 0;
            }
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (anim.GetBool("isJumpping") && h != 0) // ���� ���϶� ��Ÿ�� �ν�
            {
                Clinging(h);
            }
            else // ����Ÿ�� �����ö�
            {
                anim.SetBool("isClinging", false);
            }

            if (anim.GetBool("isClinging"))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * clingingSpeed);
            }
            if (rigid.velocity.x > maxSpeed) // Right MaxSpeed
            {
                //  background_Scroller.BG_Scroll(0.1f); // ���������� ��� ��ũ��
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < maxSpeed * (-1)) // Left MaxSpeed
            {
                // background_Scroller.BG_Scroll(-0.1f); // �������� ��� ��ũ��
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
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform", "JumpPlatform"));
                // ���� x
                if (rayHit.collider != null)
                {
                    // distance : Ray�� ����� ���� �Ÿ�
                    if (rayHit.distance < .9f) // ���� ���� ���� ��
                    {
                        jumpNum = 0; // ���� Ƚ�� �ʱ�ȭ 
                        // ���� �ִϸ��̼� �ʱ�ȭ
                        anim.SetBool("isJumpping", false);
                        anim.SetBool("IsJumpDown", false);
                        anim.SetBool("isClinging", false);
                        if (rayHit.collider.CompareTag("JumpPlatform"))
                        {
                           // rigid.velocity = new Vector2(rigid.velocity.x, 0);
                        }
                    }
                }
            }

        }
    }
}
