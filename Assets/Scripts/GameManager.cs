using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

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
    private float sliderSpeed  = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        if(stageNum == 1)
        {
            PlayCutScene(0, 7f);
        }
    }
    
    private IEnumerator ScreenSlider(int sceneNumber, float delay)
    {
        player.isCutScenePlaying = true;
        bool end = false;
        screenSlider.value = 0;
        while(screenSlider.value != 1) // 컷신 전 슬라이드 작동
        {
            screenSlider.value += Time.deltaTime;
            yield return null;
        }
        cutScene[sceneNumber].PlayCutScene(delay); // 컷신 플레이
        while (screenSlider.value != 0) // 컷신 전 슬라이드 작동
        {
            screenSlider.value -= Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(delay); // 컷신 딜레이 만큼 멈추기
        while (screenSlider.value != 1) // 컷신 전 슬라이드 작동
        {
            screenSlider.value += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (screenSlider.value != 0) // 컷신 전 슬라이드 작동
        {
            screenSlider.value -= Time.deltaTime;
            yield return null;
        }
        player.isCutScenePlaying = false;
    }
    public void PlayCutScene(int sceneNumber, float delay)
    {
        StartCoroutine(ScreenSlider(sceneNumber, delay));
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
