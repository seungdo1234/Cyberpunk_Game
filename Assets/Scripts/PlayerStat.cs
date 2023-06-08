using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // int가 아닌 float을 쓰는 이유 : 슬라이더 Value값을 서서히 변환 시키기 위해 Float을 사용
    [SerializeField]
    private float maxHP = 100; // 최대 체력
    [SerializeField]
    private float curHP; // 현재 체력

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
