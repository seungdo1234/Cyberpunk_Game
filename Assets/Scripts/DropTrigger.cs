using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform playerSpawnPoint;

    private RaycastHit2D rayHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.right * 105.0f, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position, Vector3.right, 105f, LayerMask.GetMask("Player")); // 위로 레이어 발사
        if (rayHit.collider != null)
        {
            rayHit.collider.GetComponent<PlayerStat>().TakeDamage(10f);
            rayHit.transform.position = playerSpawnPoint.position;
        }
    }
}
