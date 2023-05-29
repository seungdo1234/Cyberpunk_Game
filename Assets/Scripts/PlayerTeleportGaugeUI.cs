using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleportGaugeUI : MonoBehaviour
{
    private Slider slider;
    private bool isRecovery;
    [SerializeField]
    private float recoverySpeed;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private IEnumerator GaugeRecovery()
    {
        while(slider.value < 1)
        {
            slider.value += recoverySpeed * Time.deltaTime;
            yield return null;
        }
        isRecovery = false;
    }
    private void Update()
    {
        if(!isRecovery && slider.value != 1)
        {
            isRecovery = true;
            StartCoroutine(GaugeRecovery());
        }
    }
}
