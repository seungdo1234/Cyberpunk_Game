using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odium : MonoBehaviour
{
    [SerializeField]
    private Transform pos;  // 히팅 박스 위치
    [SerializeField]
    private Vector2 boxSize; // 박스 크기
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void PlayerAtk(float damage) // 애니메이션 트리거 활용
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
