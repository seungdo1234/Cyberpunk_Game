using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���� ���� �� ����ü ����
        {
            collision.transform.position = playerSpawnPoint.position;
            collision.GetComponent<PlayerStat>().TakeDamage(10f);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
