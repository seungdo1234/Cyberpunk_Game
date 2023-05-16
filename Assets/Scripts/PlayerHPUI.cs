
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("PlayerHP_UI")]
    [SerializeField]
    private TextMeshProUGUI textPlayerHP; // Text - TextMeshPro UI [�÷��̾� ü��]
    [SerializeField]
    private PlayerStat playerStat; // �÷��̾� ���� ����
    private Slider hpSlider;
    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        textPlayerHP.text = playerStat.CurHP.ToString() + " / " + playerStat.MaxHP.ToString(); // �ؽ�Ʈ       
        hpSlider.value = playerStat.CurHP / playerStat.MaxHP; // �����̵�
    }
}
