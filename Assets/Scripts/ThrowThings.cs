using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowThings : MonoBehaviour
{
    [SerializeField]
    private float throwDirection; // 던지는 방향
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int throwingType; 
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(throwingType == 1 && collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().OnDamaged(1,2);
            StopCoroutine(Throwing(throwDirection));
            Destroy(gameObject);
        }
        else if(throwingType == 2 && collision.tag == "Player")
        {
            collision.GetComponent<PlayerStat>().TakeDamage(5f);
            StopCoroutine(Throwing(throwDirection));
            Destroy(gameObject);
        }
        Debug.Log(collision.tag);
    }

    public IEnumerator Throwing(float throwVelocity)
    {
        float throwing = .5f;
        while(throwing >= 0)
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
        if(throwDirection == -1)
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
