using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints; // �÷��� �̵� �÷���
    [SerializeField]
    private float moveSpeed; // ��������Ʈ�� �����ϴ� �ð�
    [SerializeField]
    private int dirc = 1;
    private Rigidbody2D playerRigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MovePlatform());
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerRigidbody2D = collision.collider.GetComponent<Rigidbody2D>();

            // player�� ������ ���� �� Ȥ�� Platform�� ���� �������� �� ��
            if ((playerRigidbody2D.velocity.x == 0) || (playerRigidbody2D.velocity.x < 0 && dirc == -1) || (playerRigidbody2D.velocity.x > 0 && dirc == 1))
            {
                // Player�� Platform�� ���� �����̱� ���� Player�� ��ġ�� ��ȯ��Ŵ
                collision.collider.transform.position += new Vector3(dirc * moveSpeed * Time.deltaTime, 0, 0);
            }

        }
    }
    private IEnumerator MovePlatform()
    {
        int wayPointNum;
        if(dirc == 1)
        {
            wayPointNum = 1;
        }
        else
        {
            wayPointNum = 0;
        }
        while (true)
        {
            transform.position += new Vector3 ( dirc * moveSpeed * Time.deltaTime,0, 0);
            if ( dirc == 1 && transform.position.x >= wayPoints[wayPointNum].position.x )
            {
                dirc = -1;
                wayPointNum--;
            }
            else if(dirc == -1 && transform.position.x <= wayPoints[wayPointNum].position.x)
            {
                dirc = 1;
                wayPointNum++;
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
