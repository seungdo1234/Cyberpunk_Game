using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform

    // Start is called before the first frame update
    void Start()
    {
        SpanwnEnemy();
    }

    private void SpanwnEnemy()
    {
        GameObject clone = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        SpawnHP_Slider(clone);
    }

    private void SpawnHP_Slider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�.
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �i�ƴٴ� ����� �������� ����
        sliderClone.GetComponent<HP_SliderPoisition>().Setup(enemy.transform);
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
