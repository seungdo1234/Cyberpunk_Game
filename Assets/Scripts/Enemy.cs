using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamaged(int damage)
    {
        enemyHP -= damage;
    }
    public void Q()
    {
        Debug.Log("����");
    }
}
