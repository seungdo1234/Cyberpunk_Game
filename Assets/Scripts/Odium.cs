using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odium : MonoBehaviour
{
    [SerializeField]
    private Transform pos;  // ���� �ڽ� ��ġ
    [SerializeField]
    private Vector2 boxSize; // �ڽ� ũ��
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void PlayerAtk(float damage) // �ִϸ��̼� Ʈ���� Ȱ��
    {
        Collider2D collider2D = Physics2D.OverlapBox(pos.position, boxSize, 0);
        if (collider2D != null)
        {
            if (collider2D.tag == "Player")
            {
                collider2D.GetComponent<Player>().OnDamaged(transform.position, damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
