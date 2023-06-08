using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 레이저에 맞을경우에 나오는 행동
public class LaserTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(100f);
        }
    }
}
