using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField]
    private int dirc; // 방향
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private Transform shotPoint; // Shot 나오는 위치
    [SerializeField]
    private float attackDelay; // DangerLine이 나오는 시간
    [Header("DangerLine")]
    [SerializeField]
    private GameObject dangerLinePrefab; // dangerLine



    void Awake()
    {
        if (dirc == 1)
        {
            shotPoint.position += new Vector3(1.2f,0,0);
        }
        StartCoroutine(Shot());
    }
    private IEnumerator Shot() 
    {
        while (true)
        {
            // DangerLine 소환
            GameObject dangerLine = Instantiate(dangerLinePrefab, shotPoint.position, Quaternion.identity); 
            yield return new WaitForSeconds(2.5f); // DangerLine이 사라지면 Shot
            GameObject shotClone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity); // Shot 소환
            shotClone.GetComponent<ThrowThings>().Throw(17f, dirc); // Shot 날리기
            yield return new WaitForSeconds(attackDelay); // AttackDelay만큼 기다리기
        }
    }
}
