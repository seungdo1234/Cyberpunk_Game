using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private Transform shotPoint;
    [Header("DangerLine")]
    [SerializeField]
    private LineRenderer lineRenderer; // 레이저로 사용되는 선 (LineRenderer)
    // Start is called before the first frame update
    private Animator anim;
    public float turretAtkDelay; // 터렛 공격 속도
    private bool turretExp = false; // 터렛이 파괴되는 중인지
    private bool turretIsAttacking = false; // 터렛이 공격 중인지
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turretExp == false)
        {
            // 양방향으로 레이어 탐지
            Debug.DrawRay(transform.position, Vector3.right * 8.0f, new Color(0, 1, 0));
            Debug.DrawRay(transform.position, Vector3.left * 8.0f, new Color(0, 1, 0));
            RaycastHit2D rayHitLeft = Physics2D.Raycast(transform.position, Vector3.left, 8f, LayerMask.GetMask("Wall", "Player")); // 왼쪽 레이어 
            RaycastHit2D rayHitRight = Physics2D.Raycast(transform.position, Vector3.right, 8f, LayerMask.GetMask("Wall", "Player")); // 오른쪽 레이어


            if (!turretIsAttacking && rayHitLeft.collider != null && rayHitLeft.collider.tag == "Player") // 왼쪽 레이어 Player 탐지
            {
                anim.SetBool("TurretLeftAtk", true);
                StartCoroutine(Shot(-1));
            }
            else if (!turretIsAttacking && rayHitRight.collider != null && rayHitRight.collider.tag == "Player") // 오른쪽 레이어 Player 탐지
            {
                anim.SetBool("TurretRightAtk", true);
               StartCoroutine(Shot(1));
            }
        }

    }
    private IEnumerator Shot(int dirc) 
    {
        turretIsAttacking = true;
        EnableDangerLine();
        SetDangerLine(dirc);
        yield return new WaitForSeconds(turretAtkDelay);
        DisableDangerLine();
        if (dirc == 1)
        {
            GameObject clone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity);
            clone.GetComponent<ThrowThings>().Throw(10f, dirc);
        }
        else
        {
            GameObject clone = Instantiate(shotPrefab, shotPoint.position + new Vector3(-1.8f,0,0), Quaternion.identity);
            clone.GetComponent<ThrowThings>().Throw(10f, dirc);
        }
        yield return new WaitForSeconds(1f);
        if (dirc == 1)
        {
            anim.SetBool("TurretRightAtk", false);
        }
        else
        {
            anim.SetBool("TurretLeftAtk", false);
        }
        turretIsAttacking = false;
    }
    private void SetDangerLine(int dirc)
    {
        if(dirc == 1)
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x + 0.4f,shotPoint.position.y,0));
            lineRenderer.SetPosition(1, shotPoint.position + new Vector3(8,0,0));
        }
        else
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x - 0.4f, shotPoint.position.y, 0));
            lineRenderer.SetPosition(1, shotPoint.position + new Vector3(-8, 0, 0));
        }
    }
    private void EnableDangerLine()     // 레이저 오브젝트를 활성함
    {
        lineRenderer.gameObject.SetActive(true);
    }
    private void DisableDangerLine()    // 레이저 오브젝트를 비활성함
    {
        lineRenderer.gameObject.SetActive(false);
    }
}
