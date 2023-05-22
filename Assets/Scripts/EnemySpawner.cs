using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int[] enemyType; // �� Ÿ��
    [SerializeField]
    private GameObject[] enemyPrefab; // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    private Transform[] enemySpawnPoints; // �� ��������Ʈ ����Ʈ
    // Start is called before the first frame update
    void Start()
    {
       // SpanwnEnemy();
    }

    public void SpanwnEnemy()
    {
        for(int i = 0; i < enemyType.Length; i++)
        {
            GameObject clone = Instantiate(enemyPrefab[enemyType[i]], enemySpawnPoints[i].position, Quaternion.identity);
            SpawnHP_Slider(clone, enemyType[i]);
        }
    }

    private void SpawnHP_Slider(GameObject enemy, int enemyType)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�.
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �i�ƴٴ� ����� �������� ����
        sliderClone.GetComponent<HP_SliderPoisition>().Setup(enemy.transform, enemyType);
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
