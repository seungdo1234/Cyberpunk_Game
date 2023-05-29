using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Player player;
    private bool onThePlayer; // �÷��̾ ���� �ִ���
    private bool playerDownJump; // �÷��̾ �� ������ �ϸ� �����ð����� update���� �ȵ�����
    private float distance = 0.5f; // Player���� distance
   
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();


    }
    private IEnumerator Delay()
    {
        playerDownJump = true;
        yield return new WaitForSeconds(0.5f);
        playerDownJump = false;
    }
    public void PlayerDownJump()
    {
        Debug.Log("��������");
        boxCollider2D.enabled = false;
        onThePlayer = false;
        distance = 0.5f;
        StartCoroutine(Delay());
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerDownJump)
        {
            if (transform.position.y + distance < player.transform.position.y)
            {
                if (!onThePlayer)
                {
                    distance = 0;
                    onThePlayer = true;
                }
                boxCollider2D.enabled = true;
            }
            else
            {
                if (onThePlayer)
                {
                    onThePlayer = false;
                    distance = 0.5f;
                }
                boxCollider2D.enabled = false;
            }
        }
    }
}
