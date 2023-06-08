using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField]
    private int dirc; // ����
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private Transform shotPoint; // Shot ������ ��ġ
    [SerializeField]
    private float attackDelay; // DangerLine�� ������ �ð�
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
            yield return new WaitForSeconds(2.5f); // DangerLine�� ������� Shot
            GameObject shotClone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity); // Shot ��ȯ
            shotClone.GetComponent<TurretShot>().ShotSetUp(dirc, 17f, shotDistance);
            yield return new WaitForSeconds(attackDelay); // AttackDelay��ŭ ��ٸ���
        }
    }
}
