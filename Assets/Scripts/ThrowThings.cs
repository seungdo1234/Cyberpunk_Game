using System.Collections;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField]
    private float throwDirection; // ������ ����
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
        if (throwingType == 1 && collision.tag == "Enemy") // ������
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
        Debug.Log(collision.tag); // �������� ������ �� �� �÷��̾� �±װ� �ɸ��� ��������
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
    public void ThrowTo(float direction) // ���� �ٲٱ�
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
