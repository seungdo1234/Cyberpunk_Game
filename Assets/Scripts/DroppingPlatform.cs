using System.Collections;
using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer powerRenderer; // DroppingPlatform 파워 (파워가 꺼지면서 떨어지는 것을 구현)
    [SerializeField] private float powerLerpTime; // 파워 지속 시간
    [SerializeField] private float dropTime; // 떨어지는 시간
    [SerializeField] private float resetTime; // 재생성 시간

    private Vector3 startTransform; // DroppingPlatform 초기 위치
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // Player가 위로 올라간다면
        {
            StartCoroutine(StartDropping());
        }
    }

    private void ResetPlatform() // 플랫폼 재생성
    {
        powerRenderer.color = Color.white;
        rigidbody2D.isKinematic = true;
        rigidbody2D.velocity = Vector2.zero;
        transform.position = startTransform;
        boxCollider2D.enabled = false; // 생성 완료 하기전까지 발판 비활성화
        StartCoroutine(StartReset());
    }

    private IEnumerator StartReset()
    {
        float currentTime = 0f;
        Color currentColor = spriteRenderer.color; // 초기 color 값 저장

        while (currentTime < resetTime) // 생성
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / resetTime);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            spriteRenderer.color = newColor;
            powerRenderer.color = newColor;
            yield return null;
            currentTime += Time.deltaTime;
        }

        boxCollider2D.enabled = true; // 발판 활성화
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
        rigidbody2D.isKinematic = false; // 떨어뜨림
        yield return new WaitForSeconds(dropTime);
        ResetPlatform();
    }
}
