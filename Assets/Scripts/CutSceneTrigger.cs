using System.Collections;
using UnityEngine;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform[] cutSceneTriggerBox; // 컷신 트리거 박스 위치
    [SerializeField]
    private bool isTrigger; // Player가 해당 Platform위에 있을 경우
    [SerializeField]
    private Transform[] layerPoint; // 레이어를 발사하는 포인트
    [SerializeField]
    private DropTrigger dropLayer; // 컷신 이후에는 DropLayerPoint가 필요없으므로 비활성화
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private int stageNum;
    [SerializeField]
    private GameObject transparentWall; // 컷신이 끝나도 전으로 못가게 하기위한 투명벽
    [SerializeField]
    private int triggerNum = 0; // 트리거 레이어 위치
    private int sceneNum = 1; // 불러올 씬 넘버
    private bool cutScenePlaying; // 컷신 플레이 중
    private RaycastHit2D rayHit;

    private void Start()
    {
        transform.position = cutSceneTriggerBox[triggerNum].position;
    }
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
        if(stageNum == 1 || (stageNum ==2 && triggerNum == 1) )
        {
            enemySpawner.SpanwnEnemy();
          //  dropLayer.SpawnPointChange();
        }
        if(stageNum == 2 && triggerNum == 0)
        {
            transparentWall.SetActive(true);
        }
        triggerNum++;
        sceneNum++;
        if(stageNum == 2 && triggerNum == 1)
        {
            transform.position = cutSceneTriggerBox[triggerNum].position;
        }
        isTrigger = false;
        cutScenePlaying = false;
    }
    private void Update()
    {
        if (isTrigger)
        {
            Debug.DrawRay(layerPoint[triggerNum].position, Vector3.up * 10.0f, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(layerPoint[triggerNum].position, Vector3.up, 10f, LayerMask.GetMask("Player")); // 위로 레이어 발사 
            if (rayHit.collider != null)
            {
                // rayHit.transform.position = spawnPoint.position;
                if (!cutScenePlaying)
                {
                    rayHit.collider.GetComponent<Animator>().SetBool("isJumpping", false); // 점프 중인 경우 점프 애니메이션 해제
                    rayHit.collider.GetComponent<Animator>().SetBool("IsJumpDown", false); // 점프 중인 경우 점프 애니메이션 해제
                    rayHit.collider.GetComponent<Animator>().SetBool("isRunning", false); // 달리기 중인 경우 달리기 애니메이션 해제
                    if (stageNum == 1)
                    {
                        StartCoroutine(PlayCutScene(sceneNum, 7.5f));   
                    }
                    else if(stageNum == 2 )
                    {
                        if(triggerNum == 0)
                        {
                            StartCoroutine(PlayCutScene(0, 20.5f));
                        }
                        else if (triggerNum == 1)
                        {
                            StartCoroutine(PlayCutScene(1, 34f));
                        }
                    }
                }

            }
        }
    }
}
