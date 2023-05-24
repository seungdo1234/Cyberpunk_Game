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
        while(true) // 컷신 전 슬라이드 작동
        {
            if (!end)
            {
                screenSlider.value += Time.deltaTime;
            }
            else if(end)
            {
                screenSlider.value -= Time.deltaTime;
            }
            if (screenSlider.value >= 1)
            {
                end = true;
            }
            if(end && screenSlider.value == 0)
            {
                break;
            }
            yield return null;
        }
        cutScene[sceneNumber].PlayCutScene(delay); // 컷신 플레이
        yield return new WaitForSeconds(delay); // 컷신 딜레이 만큼 멈추기
        end = false;
        while (true) // 컷신 후 슬라이드 작동
        {
            if (!end)
            {
                screenSlider.value += Time.deltaTime;
            }
            else if (end)
            {
                screenSlider.value -= Time.deltaTime;
            }
            if (screenSlider.value >= 1)
            {
                end = true;
            }
            if (end && screenSlider.value == 0)
            {
                break;
            }
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
