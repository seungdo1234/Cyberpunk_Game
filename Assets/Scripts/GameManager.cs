using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int stageNum;
    [SerializeField]
    private CutScene[] cutScene;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Slider screenSlider; // �ƽ� �� �� ������ ȭ�� ������
    [SerializeField]
    private StageNameAlpha stageNameAlpha; // �������� �̸� 
    [SerializeField]
    private BoxDropTrigger boxDropTrigger;
    [SerializeField]
    private GuideWindow teleportGuideWindow; // ��ǻŸ �ƽ� 1�� ������ ������ ���� â

    [SerializeField]
    private Transform[] playerSavePoints; // �÷��̾� ���̺� ����Ʈ
    private int savePointNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (stageNum == 1)
        {
            PlayCutScene(0, 7f);
        }
    }
    private IEnumerator ScreenSlider(int type, float slideSpeed)
    {
        if(type == 1)
        {
            screenSlider.value = 0;
            while (screenSlider.value != 1) // �ƽ� �� �����̵� �۵�
            {
                screenSlider.value += Time.deltaTime * slideSpeed;
                yield return null;
            }
        }
        if(type == 2)
        {
            screenSlider.value = 1;
            while (screenSlider.value != 0) // �ƽ� �� �����̵� �۵�
            {
                screenSlider.value -= Time.deltaTime * slideSpeed;
                yield return null;
            }
        }
    }
    private IEnumerator CutScenePlay(int sceneNumber, float delay)
    {
        player.isCutScenePlaying = true;
        screenSlider.value = 0;
        StartCoroutine(ScreenSlider(1,1));
        cutScene[sceneNumber].PlayCutScene(delay); // �ƽ� �÷���
        StartCoroutine(ScreenSlider(2,1));
        yield return new WaitForSeconds(delay); // �ƽ� ������ ��ŭ ���߱�
        StartCoroutine(ScreenSlider(1, 1));
        yield return new WaitForSeconds(1f);
        StartCoroutine(ScreenSlider(2, 1));
        player.isCutScenePlaying = false;
        yield return new WaitForSeconds(.5f);
        if(stageNum == 2 ) // �������� 2�� 0�� �ƽ��� ���� �ǰ� �� �� �������� â ���
        {    
            if(sceneNumber == 0)
            {
                stageNameAlpha.StartAlpha();
            }
            else if(sceneNumber == 1)
            {
                teleportGuideWindow.GuideOn();
            }
        }
    }
    public void PlayCutScene(int sceneNumber, float delay)
    {
        if(stageNum == 2 && sceneNumber == 0) // �������� 2�� 0�� �ƽ��� ����Ǳ��� BoxDropTrigger ����
        {
            boxDropTrigger.StopAllCoroutines();
        }
        StartCoroutine(CutScenePlay(sceneNumber, delay)); // �ƽ� ����
    }
    private IEnumerator CutScenePlaying(float delay)
    {
        player.isCutScenePlaying = true; // �ƽ� �߿� �÷��̾� ������ �ȵǰ� ����
        yield return new WaitForSeconds(delay);
        player.isCutScenePlaying = false;

    }
    
    private IEnumerator GameOver()
    {
        StartCoroutine(ScreenSlider(1, 2));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ScreenSlider(2, 2));
        player.transform.position = playerSavePoints[savePointNum].position;
        player.PlayerSpawn();
    }
    public void PlayerDie()
    {
        StartCoroutine(GameOver());
    }
}
