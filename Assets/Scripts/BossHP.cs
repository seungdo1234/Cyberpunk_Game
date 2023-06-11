using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    // int�� �ƴ� float�� ���� ���� : �����̴� Value���� ������ ��ȯ ��Ű�� ���� Float�� ���
    [SerializeField]
    private float maxHP = 200; // �ִ� ü��
    [SerializeField]
    private float curHP; // ���� ü��

    public float MaxHP => maxHP;
    public float CurHP => curHP;

    private void Awake()
    {
        curHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        curHP -= damage;
    }
}