using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform pos;
    [SerializeField]
    private GameObject shotPrefab;
    [SerializeField]
    private Transform shotPoint;
    // Start is called before the first frame update
    void Awake()
    {
        pos = GetComponent<Transform>();
    }
   
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(pos.position, Vector3.right, new Color(0, 1, 0));
        Debug.DrawRay(pos.position, Vector3.left, new Color(0, 1, 0));
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Shot());
        }
        // 빔에 맞은 오브젝트의 정보
     //   RaycastHit2D rayHit = Physics2D.Raycast(pos.position, Vector3.right, 1, LayerMask.GetMask("Platform"));
    }
    private IEnumerator Shot()
    {
        while (true)
        {
            GameObject clone = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity);
            clone.GetComponent<ThrowThings>().Throw(10f);
            yield return new WaitForSeconds(3f);
        }
    }
}
