using System.Collections;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField]
    private int throwDirection; // 던지는 방향
    private SpriteRenderer spriteRenderer;
    private Transform pos;
    [SerializeField]
    private int throwingType;
    // Start is called before the first frame update
    void Awake()
    {
        pos = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall")) // 벽에 맞을 때 투사체 삭제
        {
        }
        else if (throwingType == 1 && collision.CompareTag("Enemy")) // Player Knife
        {
       //     collision.GetComponent<Enemy>().KnifeHit();
            collision.GetComponent<EnemyHP>().TakeDamage(1);
        }
        else if (throwingType == 2 && collision.CompareTag ( "Player")) // Enemy Shot
        {
            collision.GetComponent<PlayerStat>().TakeDamage(5f);
            collision.GetComponent<Player>().OnDamaged(pos.position);
        }
        StopAllCoroutines();
        Destroy(gameObject);
        Debug.Log(collision.gameObject.tag); // 왼쪽으로 나이프 쏠 때 플레이어 태그가 걸리는 버그있음
    }
    public IEnumerator Throwing(float throwVelocity)
    {
        float throwing = .7f;
        while (throwing >= 0)
        {
            transform.Translate(new Vector3(1 * throwDirection, 0, 0) * throwVelocity * Time.deltaTime);
            throwing -= Time.deltaTime;
            //     Debug.Log(throwing);
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
