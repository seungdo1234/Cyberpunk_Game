using UnityEngine;

// Player�� ������ �� ����Ǵ� Ʈ���� �ڵ�
public class DropTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("PlayerDamage"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(100);
        }
        else if (collision.CompareTag("Box"))
        {
            Destroy(collision.gameObject);
        }
    }
    // Update is called once per frame

}
