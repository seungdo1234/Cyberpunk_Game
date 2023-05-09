using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{

    [SerializeField]
    private bool isTrigger; // Player�� �ش� Platform���� ���� ���
    [SerializeField]
    private Transform layerPoint; // ���̾ �߻��ϴ� ����Ʈ
    [SerializeField]
    private Transform spawnPoint; // �ƽ��� ���� �� ������ ����Ʈ
    [SerializeField]
    private GameObject dropLayer; // �ƽ� ���Ŀ��� DropLayerPoint�� �ʿ�����Ƿ� ��Ȱ��ȭ
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private GameManager gameManager;

    private bool cutScenePlaying; // �ƽ� �÷��� ��
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
            rayHit = Physics2D.Raycast(layerPoint.position, Vector3.up, 10f, LayerMask.GetMask("Player")); // ���� ���̾� �߻�
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
