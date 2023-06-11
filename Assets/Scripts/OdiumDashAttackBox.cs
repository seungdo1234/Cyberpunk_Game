using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdiumDashAttackBox : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().OnDamaged(transform.position, 10f);
            gameObject.SetActive(false);
        }
    }

}
