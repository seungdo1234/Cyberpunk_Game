using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CutScene[] cutScene;
    [SerializeField]
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        PlayCutScene(0, 7f);
    }

    public void PlayCutScene(int sceneNumber, float delay)
    {
        cutScene[sceneNumber].PlayCutScene(delay);
        StartCoroutine(CutScenePlaying(delay));
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
