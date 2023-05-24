using System.Collections;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{

    [SerializeField]
    private bool isTrigger; // Player�� �ش� Platform���� ���� ���
    [SerializeField]
    private Transform[] layerPoint; // ���̾ �߻��ϴ� ����Ʈ
    [SerializeField]
    private DropTrigger dropLayer; // �ƽ� ���Ŀ��� DropLayerPoint�� �ʿ�����Ƿ� ��Ȱ��ȭ
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private int stageNum;
    [SerializeField]
    private GameObject transparentWall; // �ƽ��� ������ ������ ������ �ϱ����� ����

    private int triggerNum = 0; // Ʈ���� ���̾� ��ġ
    private int sceneNum = 1; // �ҷ��� �� �ѹ�
    private bool cutScenePlaying; // �ƽ� �÷��� ��
    private RaycastHit2D rayHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTrigger = true;
        }
    }
    private IEnumerator PlayCutScene(int sceneNumber, float delay)
    {
        cutScenePlaying = true;
        gameManager.PlayCutScene(sceneNumber, delay);
        yield return new WaitForSeconds(delay);
        if(stageNum == 1)
        {
            enemySpawner.SpanwnEnemy();
            dropLayer.SpawnPointChange();
        }
        if(stageNum == 2)
        {
            transparentWall.SetActive(true);
        }
        triggerNum++;
        sceneNum++;
        isTrigger = false;
        cutScenePlaying = false;
    }
    private void Update()
    {
        if (isTrigger)
        {
            Debug.DrawRay(layerPoint[triggerNum].position, Vector3.up * 10.0f, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(layerPoint[triggerNum].position, Vector3.up, 10f, LayerMask.GetMask("Player")); // ���� ���̾� �߻� 
            if (rayHit.collider != null)
            {
                // rayHit.transform.position = spawnPoint.position;
                if (!cutScenePlaying)
                {
                    rayHit.collider.GetComponent<Animator>().SetBool("isJumpping", false); // ���� ���� ��� ���� �ִϸ��̼� ����
                    rayHit.collider.GetComponent<Animator>().SetBool("IsJumpDown", false); // ���� ���� ��� ���� �ִϸ��̼� ����
                    rayHit.collider.GetComponent<Animator>().SetBool("isRunning", false); // �޸��� ���� ��� �޸��� �ִϸ��̼� ����
                    if (stageNum == 1)
                    {
                        StartCoroutine(PlayCutScene(sceneNum, 7.5f));   
                    }
                    else if(stageNum == 2)
                    {
                        StartCoroutine(PlayCutScene(0, 21f));
                    }
                }

            }
        }
    }
}
