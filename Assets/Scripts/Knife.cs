using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField]
    private GameObject knifePrefab; //  발사체 프리팹
    [SerializeField]
    private Transform spawnPoint; // 발사체 생성 위치
    private Vector3 moveDirection = Vector3.zero; // Vector3 값 (0,0,0)으로 초기화
    private Transform enemyPos; // 나이프에 맞은 Enemy의 위치 정보 저장
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
