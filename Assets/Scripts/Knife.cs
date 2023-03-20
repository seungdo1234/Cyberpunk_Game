using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField]
    private GameObject knifePrefab; //  �߻�ü ������
    [SerializeField]
    private Transform spawnPoint; // �߻�ü ���� ��ġ
    private Vector3 moveDirection = Vector3.zero; // Vector3 �� (0,0,0)���� �ʱ�ȭ
    private Transform enemyPos; // �������� ���� Enemy�� ��ġ ���� ����
    private Movement2D movement2D;
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            StartCoroutine(EnemyAttackMark(collision));
            collision.GetComponent<Enemy>().OnDamaged(1);
            Destroy(gameObject);
        }
    }

    private IEnumerator EnemyAttackMark(Collider2D collision)
    {
        while (true)
        {
            enemyPos = collision.transform;
            Debug.Log(enemyPos.position);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * 15f * Time.deltaTime);
    }
}
