using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab; // 적 프리팹
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // 적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform; // UI를 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]
    private Transform[] enemySpawnPoints; // 적 스폰포인트 리스트
    private int enemyType;
    // Start is called before the first frame update
    void Start()
    {
     //   SpanwnEnemy();
    }

    public void SpanwnEnemy()
    {
        for(int i =0; i<enemyPrefab.Length; i++)
        {
            if (enemyPrefab[i].name == "Enemy")
            {
                enemyType = 1;
            }
            else if (enemyPrefab[i].name == "turret")
            {
                enemyType = 2;
            }
            GameObject clone = Instantiate(enemyPrefab[i], enemySpawnPoints[i].position, Quaternion.identity);
            SpawnHP_Slider(clone, enemyType);
        }
    }

    private void SpawnHP_Slider(GameObject enemy, int enemyType)
    {
        // 적 체력을 나타내는 Slider UI 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        // Tip. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다.
        sliderClone.transform.SetParent(canvasTransform);
        // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI가 쫒아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<HP_SliderPoisition>().Setup(enemy.transform, enemyType);
        // Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
