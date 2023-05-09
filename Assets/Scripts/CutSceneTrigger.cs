using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{

    [SerializeField]
    private bool isTrigger; // Player가 해당 Platform위에 있을 경우
    [SerializeField]
    private Transform layerPoint; // 레이어를 발사하는 포인트
    [SerializeField]
    private Transform spawnPoint; // 컷신이 끝난 후 스폰될 포인트
    [SerializeField]
    private GameObject dropLayer; // 컷신 이후에는 DropLayerPoint가 필요없으므로 비활성화
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private GameManager gameManager;

    private bool cutScenePlaying; // 컷신 플레이 중
    private RaycastHit2D rayHit;
      
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isTrigger = true;
        }
    }
    private IEnumerator PlayCutScene(int sceneNumber, float delay)
    {
        cutScenePlaying = true;
        gameManager.PlayCutScene(sceneNumber, delay);
        yield return new WaitForSeconds(delay);
        enemySpawner.SpanwnEnemy();
        dropLayer.SetActive(false);
        isTrigger = false;
    }
    private void Update()
    {
        if (isTrigger)
        {
            Debug.DrawRay(layerPoint.position, Vector3.up * 10.0f, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(layerPoint.position, Vector3.up, 10f, LayerMask.GetMask("Player")); // 위로 레이어 발사
            if(rayHit.collider != null)
            {
                // rayHit.transform.position = spawnPoint.position;
                if (!cutScenePlaying)
                {
                    StartCoroutine(PlayCutScene(1, 5.5f));
                }

            }
        }
    }
}
