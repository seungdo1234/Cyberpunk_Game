using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField]
    private float throwDirection = 1f; // 던지는 방향
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().OnDamaged(1,2);
            StopCoroutine(Throwing());
            Destroy(gameObject);
        }
    }

    public IEnumerator Throwing()
    {
        float throwing = .5f;
        while(throwing >= 0)
        {
            transform.Translate(new Vector3(1 * throwDirection, 0, 0) * 20f * Time.deltaTime);
            throwing -= Time.deltaTime;
          //  Debug.Log(throwing);
            yield return null;
        }
        Destroy(gameObject);
    }
    public void ThrowTo(float direction) // 방향 바꾸기
    {
        throwDirection = direction;
    }
    public void Throw()
    {
        if(throwDirection == -1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        StartCoroutine(Throwing());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
