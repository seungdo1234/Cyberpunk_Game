
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("PlayerHP_UI")]
    [SerializeField]
    private TextMeshProUGUI textPlayerHP; // Text - TextMeshPro UI [플레이어 체력]
    [SerializeField]
    private PlayerStat playerStat; // 플레이어 현재 스탯
    private Slider hpSlider;
    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        textPlayerHP.text = playerStat.CurHP.ToString() + " / " + playerStat.MaxHP.ToString(); // 텍스트       
        hpSlider.value = playerStat.CurHP / playerStat.MaxHP; // 슬라이드
    }
}
