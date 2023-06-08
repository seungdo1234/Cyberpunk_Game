using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    
    private void OnTriggerStay2D(Collider2D collision)
    {
         if (collision.CompareTag("Player")) // Enemy Shot
        {
            collision.GetComponent<Player>().OnDamaged(transform.position, 5f);
            Destroy(gameObject);
        }
    }

    public void ShotSetUp(int dirc, float speed, float throwDistance)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (dirc == -1)
        {
            spriteRenderer.flipX = true;
        }
        StartCoroutine(Shot(dirc, speed, throwDistance));
    }
    private IEnumerator Shot(int dirc, float speed, float throwDistance)
    {
        float nowDistance = 0f;
        while(nowDistance<= throwDistance)
        {
            nowDistance += Time.deltaTime * speed;
            transform.position += new Vector3(dirc * Time.deltaTime * speed, 0,0);
            yield return null;
        }
        Destroy(gameObject);
    }

}
