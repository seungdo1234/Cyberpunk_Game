using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bossName; 
    [SerializeField]
    private EnemyHP bossHP;
    private Slider hpSlider;
    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        hpSlider.value = bossHP.CuurentHP / bossHP.MaxHp; // 슬라이드
    }
}
