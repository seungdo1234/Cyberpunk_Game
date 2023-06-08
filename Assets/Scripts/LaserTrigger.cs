using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �������� ������쿡 ������ �ൿ
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
