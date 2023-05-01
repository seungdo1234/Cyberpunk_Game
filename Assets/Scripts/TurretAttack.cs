using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private Transform shotPoint;
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


            
            if (!turretIsAttacking && rayHitLeft.collider != null && rayHitLeft.collider.tag == "Player")
            {
                anim.SetBool("TurretLeftAtk", true);
                StartCoroutine(Shot(-1));
            }
            else if (!turretIsAttacking && rayHitRight.collider != null && rayHitRight.collider.tag == "Player")
            {
                anim.SetBool("TurretRightAtk", true);
               StartCoroutine(Shot(1));
            }
        }

    }
    private IEnumerator Shot(int dirc)
    {
        Transform pos = shotPoint;
        Debug.Log(pos.position);
        turretIsAttacking = true;
        yield return new WaitForSeconds(turretAtkDelay);

        if(dirc == 1)
        {
            GameObject clone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity);
            clone.GetComponent<ThrowThings>().Throw(10f, dirc);
        }
        else
        {
            GameObject clone = Instantiate(shotPrefab, shotPoint.position + new Vector3(-1.8f,0,0), Quaternion.identity);
            clone.GetComponent<ThrowThings>().Throw(10f, dirc);
        }
        Debug.Log(shotPoint.position);
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
}
