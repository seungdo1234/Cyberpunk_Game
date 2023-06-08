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
    private TurretDangerLine turretDangerLine; // dangerLine
    [SerializeField]
    private float shotDistance;


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
            turretDangerLine.Setup();
            yield return new WaitForSeconds(2.5f); // DangerLine이 사라지면 Shot
            GameObject shotClone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity); // Shot 소환
            shotClone.GetComponent<TurretShot>().ShotSetUp(dirc, 17f, shotDistance);
            yield return new WaitForSeconds(attackDelay); // AttackDelay만큼 기다리기
        }
    }
}
