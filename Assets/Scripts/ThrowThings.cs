using System.Collections;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField]
    private float throwDirection; // 던지는 방향
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
        if (throwingType == 1 && collision.tag == "Enemy") // 나이프
        {
            collision.GetComponent<Enemy>().OnDamaged(1, 2);
            StopCoroutine(Throwing(throwDirection));
            Destroy(gameObject);
        }
        else if (throwingType == 2 && collision.tag == "Player") // Shot
        {
            collision.GetComponent<PlayerStat>().TakeDamage(5f);
            collision.GetComponent<Player>().OnDamaged(pos.position);
            StopCoroutine(Throwing(throwDirection));
            Destroy(gameObject);
        }
        Debug.Log(collision.tag); // 왼쪽으로 나이프 쏠 때 플레이어 태그가 걸리는 버그있음
    }
    public IEnumerator Throwing(float throwVelocity)
    {
        float throwing = .5f;
        while (throwing >= 0)
        {
            transform.Translate(new Vector3(1 * throwDirection, 0, 0) * throwVelocity * Time.deltaTime);
            throwing -= Time.deltaTime;
            //     Debug.Log(throwing);
            yield return null;
        }
        Destroy(gameObject);
    }
    public void ThrowTo(float direction) // 방향 바꾸기
    {
        throwDirection = direction;
    }
    public void Throw(float throwVelocity)
    {
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
