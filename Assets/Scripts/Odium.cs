using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Odium : MonoBehaviour
{
    [SerializeField]
    private Transform pos;  // 히팅 박스 위치
    [SerializeField]
    private Vector2 boxSize; // 박스 크기

    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float alphaTime;
    [SerializeField]
    private LineRenderer dashAttackDangerLine;
    [SerializeField]
    private float dashAttackDangerLineLerpTime;
    [SerializeField]
    private GameObject dashAttackBox;
    private float dashDistance = 10f;


    [SerializeField]
    private GameObject lightning; // 라이트닝 
    [SerializeField]
    private ParticleSystem lightningParticle; // 라이트닝 파티클
    [SerializeField]
    private GameObject lightningDangerLine; // 라이트닝 DangerLine
    [SerializeField]
    private Transform[] lightningWayPoints; // 라이트닝 웨이 포인트

    [SerializeField]
    private VirtualCameraShake cameraShake;

    [SerializeField]
    private ParticleSystem odiumEffect;
    [SerializeField]
    private SpriteRenderer red_BG;
    public bool pageTranform;

    private EnemyHP odiumHP;
    private int odiumType = 1; // 오디움의 상태 (1페이즈, 2페이즈)
    private int knockDownCount = 0;
    private bool isKnockDown;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Player player;
    private int dirc = -1;
    private bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        odiumHP = GetComponent<EnemyHP>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void PlayerAtk() // 애니메이션 트리거 활용
    {
        Collider2D collider2D = Physics2D.OverlapBox(pos.position, boxSize, 0);
        if (collider2D != null)
        {
            if (collider2D.tag == "Player")
            {
                if (odiumType == 1)
                {
                    collider2D.GetComponent<Player>().OnDamaged(transform.position, 10);
                }
                else
                {
                    collider2D.GetComponent<Player>().OnDamaged(transform.position, 15);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    private void X_Flip()
    {
        if (dirc > 0)
        {
            spriteRenderer.flipX = false;
            pos.position = new Vector3(transform.position.x + 1.3f, transform.position.y + 0.36f, 0);
        }
        else
        {
            spriteRenderer.flipX = true;
            pos.position = new Vector3(transform.position.x - 1.3f, transform.position.y + 0.36f, 0);
        }
    }
    private IEnumerator DashAttack()
    {
        dirc = player.transform.position.x - transform.position.x > 0 ? -1 : 1;
        X_Flip();
        RaycastHit2D rayHit = Physics2D.Raycast(new Vector3(player.transform.position.x + (-dirc * dashDistance / 2), player.transform.position.y, 0), Vector2.right * dirc, dashDistance, LayerMask.GetMask("NoClingingWall"));
        if (rayHit.collider != null)
        {
            rigid.position = new Vector2(rayHit.point.x + (1.5f * dirc), transform.position.y);
        }
        else
        {
            rigid.position = new Vector3(player.transform.position.x + (-dirc * dashDistance / 2), transform.position.y, 0);
        }
        anim.SetTrigger("AttackReady");
        StartCoroutine(DashAttackDangerLine());
        yield return new WaitForSeconds(dashAttackDangerLineLerpTime);

        float targetX = transform.position.x + (dashDistance * dirc);
        anim.SetTrigger("AttackGo");
        dashAttackBox.SetActive(true);
        while (true)
        {

            if (dirc > 0 && targetX <= transform.position.x)
            {
                break;
            }
            else if (dirc < 0 && targetX >= transform.position.x)
            {
                break;
            }
            transform.Translate(Vector3.right * dashSpeed * Time.deltaTime * dirc);
            yield return null;
        }
        dashAttackBox.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(PositionReset());
        knockDownCount++;
    }
    private IEnumerator DashAttackDangerLine()
    {
        dashAttackDangerLine.positionCount = 2; // LineRenderer의 위치 개수 설정
        dashAttackDangerLine.SetPosition(0, rigid.position + new Vector2(0, -0.1f));
        dashAttackDangerLine.SetPosition(1, rigid.position + new Vector2(dirc * dashDistance, -0.1f));
        float currentTime = 0;
        while (currentTime < dashAttackDangerLineLerpTime)
        {
            currentTime += Time.deltaTime;
            float width = Mathf.Lerp(0.3f, 0, currentTime / dashAttackDangerLineLerpTime);
            dashAttackDangerLine.startWidth = width;
            dashAttackDangerLine.endWidth = width;
            yield return null;
        }
        dashAttackDangerLine.positionCount = 0; // 제거
    }
    private IEnumerator TeleportAttack()
    {
        StartCoroutine(Alpha(1, 0));
        yield return new WaitForSeconds(1f);
        dirc = player.transform.position.x - transform.position.x > 0 ? 1 : -1;
        X_Flip();
        transform.position = new Vector3(player.transform.position.x - 1.5f * dirc, transform.position.y, 0);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        anim.SetTrigger("Attacking");
        yield return new WaitForSeconds(2f);
        StartCoroutine(PositionReset());
        knockDownCount++;

    }
    private IEnumerator PositionReset()
    {
        StartCoroutine(Alpha(1, 0));
        yield return new WaitForSeconds(alphaTime);
        dirc = -1;
        X_Flip();
        transform.position = new Vector3(12, -3.8f, 0);
        StartCoroutine(Alpha(0, 1));
        yield return new WaitForSeconds(alphaTime + 1f);
        isAttacking = false;
    }
    private IEnumerator Alpha(float start, float end)
    {
        float currentTime = 0f;
        while (currentTime < alphaTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, currentTime / alphaTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
    }
    private IEnumerator Lightning(int lightingNum) // 라이트닝 공격
    {
        int num = 0;
        float distance, targetPos_WayPoint;
        StartCoroutine(Alpha(1, 0)); // 위로 순간이동
        rigid.velocity = Vector2.zero;
        rigid.isKinematic = true;
        yield return new WaitForSeconds(alphaTime);
        transform.position = new Vector3(0, transform.position.y + 5f, 0);
        StartCoroutine(Alpha(0, 1));
        yield return new WaitForSeconds(alphaTime + 1f);

        while (num < lightingNum)
        {
            List<int> spawnLighting = new List<int>();
            int wayPoint = 0;
            targetPos_WayPoint = Mathf.Abs(player.transform.position.x - lightningWayPoints[0].position.x);
            for (int i = 1; i < lightningWayPoints.Length; i++) // 플레이어하고 가장 가까운 타일 하나 설정
            {
                distance = Mathf.Abs(player.transform.position.x - lightningWayPoints[i].position.x);
                if (targetPos_WayPoint >= distance)
                {
                    targetPos_WayPoint = distance;
                    wayPoint = i;
                }
            }
            spawnLighting.Add(wayPoint);
            for (int i = 0; i < 4; i++) // 랜덤으로 설정
            {
                bool check = false; // 중복 값 제거
                int randomValue = Random.Range(0, 9);
                for (int j = 0; j < spawnLighting.Count; j++)
                {
                    if (randomValue == spawnLighting[j])
                    {
                        i--;
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    spawnLighting.Add(randomValue);
                }
            }
            for (int i = 0; i < spawnLighting.Count; i++) // 라이트닝 범위 
            {
                GameObject lightingDangerLineClone = Instantiate(lightningDangerLine, lightningWayPoints[spawnLighting[i]].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(.7f);
            for (int i = 0; i < spawnLighting.Count; i++) // 라이트닝 소환
            {
                GameObject lightingClone = Instantiate(lightning, lightningWayPoints[spawnLighting[i]].position, Quaternion.identity);
                ParticleSystem lightningParticleClone = Instantiate(lightningParticle, new Vector3(lightningWayPoints[spawnLighting[i]].position.x, lightningWayPoints[spawnLighting[i]].position.y - 1.5f, 0), Quaternion.identity);
            }
            cameraShake.ShakeCamera(1f,3,3);
            num++;
            yield return new WaitForSeconds(.5f);
        }
        rigid.isKinematic = false;
        yield return new WaitForSeconds(5f);
        isKnockDown = false;
        knockDownCount = 0;
    }
    // Update is called once per frame
    private IEnumerator Page2_Effect()
    {
        StartCoroutine(PositionReset());
        yield return new WaitForSeconds(1f);
        ParticleSystem Page2ParticleClone = Instantiate(odiumEffect, new Vector3(transform.position.x - 2, transform.position.y - 1, 0), Quaternion.identity);
        cameraShake.ShakeCamera(5f,5,5);
        float currentTime = 0f;
        while (currentTime < 6f)
        {
            currentTime += Time.deltaTime;
            float color = Mathf.Lerp(1, 0, currentTime / 6f);
            float backgroudAlpha = Mathf.Lerp(0, 0.45f, currentTime / 6f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, color, color, 1);
            red_BG.color = new Color(red_BG.color.r, red_BG.color.g, red_BG.color.b, backgroudAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        odiumHP.OdiumRecoveryHP();
        yield return new WaitForSeconds(3f);
        Destroy(Page2ParticleClone);
        isAttacking = false;
        pageTranform = false;
        knockDownCount = 0;
    }
    public void OdiumPage2()
    {
        StopAllCoroutines();
        odiumType++;
        pageTranform = true;
        StartCoroutine(Page2_Effect());
    }
    void Update()
    {
        if (!pageTranform && !isAttacking && !isKnockDown)
        {
            isAttacking = true;
            int attackPattern = Random.Range(1, 3);
            if (attackPattern == 1)
            {
                StartCoroutine(DashAttack());
            }
            else if (attackPattern == 2)
            {
                StartCoroutine(TeleportAttack());
            }
        }


        if (!pageTranform && !isKnockDown && knockDownCount == 3)
        {
            isKnockDown = true;
            int lightingNum = Random.Range(2, 5);
            StartCoroutine(Lightning(lightingNum));
        }
    }
}
