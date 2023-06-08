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
    private Slider screenSlider; // 컷신 전 후 나오는 화면 가리개
    [SerializeField]
    private StageNameAlpha stageNameAlpha; // 스테이지 이름 
    [SerializeField]
    private BoxDropTrigger boxDropTrigger;
    [SerializeField]
    private GuideWindow teleportGuideWindow; // 라퓨타 컷신 1이 끝나고 나오는 도움말 창

    [SerializeField]
    private Transform[] playerSavePoints; // 플레이어 세이브 포인트
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
            while (screenSlider.value != 1) // 컷신 전 슬라이드 작동
            {
                screenSlider.value += Time.deltaTime * slideSpeed;
                yield return null;
            }
        }
        if(type == 2)
        {
            screenSlider.value = 1;
            while (screenSlider.value != 0) // 컷신 전 슬라이드 작동
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
        cutScene[sceneNumber].PlayCutScene(delay); // 컷신 플레이
        StartCoroutine(ScreenSlider(2,1));
        yield return new WaitForSeconds(delay); // 컷신 딜레이 만큼 멈추기
        StartCoroutine(ScreenSlider(1, 1));
        yield return new WaitForSeconds(1f);
        StartCoroutine(ScreenSlider(2, 1));
        player.isCutScenePlaying = false;
        yield return new WaitForSeconds(.5f);
        if(stageNum == 2 ) // 스테이지 2의 0번 컷신이 진행 되고 난 후 스테이지 창 출력
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
        if(stageNum == 2 && sceneNumber == 0) // 스테이지 2의 0번 컷신이 진행되기전 BoxDropTrigger 멈춤
        {
            boxDropTrigger.StopAllCoroutines();
        }
        StartCoroutine(CutScenePlay(sceneNumber, delay)); // 컷신 시작
    }
    private IEnumerator CutScenePlaying(float delay)
    {
        player.isCutScenePlaying = true; // 컷신 중에 플레이어 조작이 안되게 설정
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
