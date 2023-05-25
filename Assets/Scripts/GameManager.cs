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
    public float slideSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (stageNum == 1)
        {
            PlayCutScene(0, 7f);
        }
    }
    
    private IEnumerator ScreenSlider(int sceneNumber, float delay)
    {
        player.isCutScenePlaying = true;
        screenSlider.value = 0;
        while(screenSlider.value != 1) // 컷신 전 슬라이드 작동
        {
            screenSlider.value += Time.deltaTime * slideSpeed;
            yield return null;
        }
        cutScene[sceneNumber].PlayCutScene(delay); // 컷신 플레이
        while (screenSlider.value != 0) // 컷신 전 슬라이드 작동
        {
            screenSlider.value -= Time.deltaTime * slideSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(delay); // 컷신 딜레이 만큼 멈추기
        while (screenSlider.value != 1) // 컷신 후 슬라이드 작동
        {
            screenSlider.value += Time.deltaTime * slideSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (screenSlider.value != 0) // 컷신 후 슬라이드 작동
        {
            screenSlider.value -= Time.deltaTime * slideSpeed;
            yield return null;
        }
        player.isCutScenePlaying = false;
        yield return new WaitForSeconds(.5f);
        if(stageNum == 2  && sceneNumber == 0) // 스테이지 2의 0번 컷신이 진행 되고 난 후 스테이지 창 출력
        {
           
            stageNameAlpha.StartAlpha();
        }
    }
    public void PlayCutScene(int sceneNumber, float delay)
    {
        if(stageNum == 2 && sceneNumber == 0) // 스테이지 2의 0번 컷신이 진행되기전 BoxDropTrigger 멈춤
        {
            boxDropTrigger.StopAllCoroutines();
        }
        StartCoroutine(ScreenSlider(sceneNumber, delay)); // 컷신 시작
    }
    private IEnumerator CutScenePlaying(float delay)
    {
        player.isCutScenePlaying = true; // 컷신 중에 플레이어 조작이 안되게 설정
        yield return new WaitForSeconds(delay);
        player.isCutScenePlaying = false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
