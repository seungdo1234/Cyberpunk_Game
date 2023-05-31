using System.Collections;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField]
    private int throwDirection; // ������ ����
    [SerializeField]
    private float lerpTime; // ���ӽð�
    private SpriteRenderer spriteRenderer;
    private Transform pos;
    [SerializeField]
    private int throwingType; // 1. Knife, 2. Shot 
    // Start is called before the first frame update
    void Awake()
    {
        pos = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall")) // ���� ���� �� ����ü ����
        {
        }
        else if (throwingType == 1 && collision.CompareTag("Enemy")) // Player Knife
        {
       //     collision.GetComponent<Enemy>().KnifeHit();
            collision.GetComponent<EnemyHP>().TakeDamage(3,2);
        }
        else if (throwingType == 2 && collision.CompareTag ( "Player")) // Enemy Shot
        {
            collision.GetComponent<Player>().OnDamaged(pos.position,5f);
        }
        Destroy(gameObject);
        Debug.Log(collision.gameObject.tag); // �������� ������ �� �� �÷��̾� �±װ� �ɸ��� ��������
    }
    public IEnumerator Throwing(float throwVelocity)
    {

        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            transform.Translate(new Vector3(1 * throwDirection, 0, 0) * throwVelocity * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
    public void Throw(float throwVelocity , int direction)
    {
        throwDirection = direction;
        if (throwDirection == -1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        StartCoroutine(Throwing(throwVelocity));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
