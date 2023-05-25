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
        while(screenSlider.value != 1) // �ƽ� �� �����̵� �۵�
        {
            screenSlider.value += Time.deltaTime * slideSpeed;
            yield return null;
        }
        cutScene[sceneNumber].PlayCutScene(delay); // �ƽ� �÷���
        while (screenSlider.value != 0) // �ƽ� �� �����̵� �۵�
        {
            screenSlider.value -= Time.deltaTime * slideSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(delay); // �ƽ� ������ ��ŭ ���߱�
        while (screenSlider.value != 1) // �ƽ� �� �����̵� �۵�
        {
            screenSlider.value += Time.deltaTime * slideSpeed;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (screenSlider.value != 0) // �ƽ� �� �����̵� �۵�
        {
            screenSlider.value -= Time.deltaTime * slideSpeed;
            yield return null;
        }
        player.isCutScenePlaying = false;
        yield return new WaitForSeconds(.5f);
        if(stageNum == 2  && sceneNumber == 0) // �������� 2�� 0�� �ƽ��� ���� �ǰ� �� �� �������� â ���
        {
           
            stageNameAlpha.StartAlpha();
        }
    }
    public void PlayCutScene(int sceneNumber, float delay)
    {
        if(stageNum == 2 && sceneNumber == 0) // �������� 2�� 0�� �ƽ��� ����Ǳ��� BoxDropTrigger ����
        {
            boxDropTrigger.StopAllCoroutines();
        }
        StartCoroutine(ScreenSlider(sceneNumber, delay)); // �ƽ� ����
    }
    private IEnumerator CutScenePlaying(float delay)
    {
        player.isCutScenePlaying = true; // �ƽ� �߿� �÷��̾� ������ �ȵǰ� ����
        yield return new WaitForSeconds(delay);
        player.isCutScenePlaying = false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
