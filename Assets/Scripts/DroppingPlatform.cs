using System.Collections;
using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer powerRenderer; // DroppingPlatform �Ŀ� (�Ŀ��� �����鼭 �������� ���� ����)
    [SerializeField] private float powerLerpTime; // �Ŀ� ���� �ð�
    [SerializeField] private float dropTime; // �������� �ð�
    [SerializeField] private float resetTime; // ����� �ð�

    private Vector3 startTransform; // DroppingPlatform �ʱ� ��ġ
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        startTransform = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // Player�� ���� �ö󰣴ٸ�
        {
            StartCoroutine(StartDropping());
        }
    }

    private void ResetPlatform() // �÷��� �����
    {
        powerRenderer.color = Color.white;
        rigidbody2D.isKinematic = true;
        rigidbody2D.velocity = Vector2.zero;
        transform.position = startTransform;
        boxCollider2D.enabled = false; // ���� �Ϸ� �ϱ������� ���� ��Ȱ��ȭ
        StartCoroutine(StartReset());
    }

    private IEnumerator StartReset()
    {
        float currentTime = 0f;
        Color currentColor = spriteRenderer.color; // �ʱ� color �� ����

        while (currentTime < resetTime) // ����
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / resetTime);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            spriteRenderer.color = newColor;
            powerRenderer.color = newColor;
            yield return null;
            currentTime += Time.deltaTime;
        }

        boxCollider2D.enabled = true; // ���� Ȱ��ȭ
    }

    private IEnumerator StartDropping()
    {
        float currentTime = 0f;

        while (currentTime < powerLerpTime) // PowerOff
        {
            float powerColor = Mathf.Lerp(1f, 0f, currentTime / powerLerpTime);
            powerRenderer.color = new Color(powerColor, powerColor, powerColor);
            yield return null;
            currentTime += Time.deltaTime;
        }
        rigidbody2D.isKinematic = false; // ����߸�
        yield return new WaitForSeconds(dropTime);
        ResetPlatform();
    }
}
