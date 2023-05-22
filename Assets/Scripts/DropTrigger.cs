using UnityEngine;

// Player가 떨어질 때 실행되는 트리거 코드
public class DropTrigger : MonoBehaviour
{
    private int spawnPointNum = 0;
    [SerializeField]
    private Transform[] playerSpawnPoint;
    [SerializeField]
    private Transform[] layerPoint;
    [SerializeField]
    private float[] distance;

    private RaycastHit2D rayHit;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SpawnPointChange()
    {
        spawnPointNum += 1;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(layerPoint[spawnPointNum].position, Vector3.right * distance[spawnPointNum], new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(layerPoint[spawnPointNum].position, Vector3.right, distance[spawnPointNum], LayerMask.GetMask("Player", "Box")); // 오른쪽으로 레이어 발사
        if (rayHit.collider != null)
        {
            if (rayHit.collider.tag == "Player")
            {
                rayHit.collider.GetComponent<PlayerStat>().TakeDamage(10f);
                rayHit.transform.position = playerSpawnPoint[spawnPointNum].position;
            }
            else if (rayHit.collider.tag == "Box")
            {
                Destroy(rayHit.collider.gameObject);
                
            }
        }
    }
}
