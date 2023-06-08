using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // int�� �ƴ� float�� ���� ���� : �����̴� Value���� ������ ��ȯ ��Ű�� ���� Float�� ���
    [SerializeField]
    private float maxHP = 100; // �ִ� ü��
    [SerializeField]
    private float curHP; // ���� ü��

    private Player player;
    public float MaxHP => maxHP;
    public float CurHP => curHP;

    private void Awake()
    {
        player = GetComponent<Player>();
        curHP = maxHP;
    }
    
    public void TakeDamage(float damage)
    {
        curHP -= damage;
        isGameOver();
    }
    public void RecoveryHP(float recovery)
    {
        if(curHP+ recovery <= maxHP)
        {
            curHP += recovery;
        }
        else
        {
            curHP = maxHP;
        }
    }
    private void isGameOver()
    {
        if(curHP <= 0)
        {
            player.PlayerDie();

        }
    }
}
