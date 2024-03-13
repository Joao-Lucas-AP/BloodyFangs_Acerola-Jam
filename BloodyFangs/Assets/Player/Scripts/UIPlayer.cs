using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    public Image hpImage;
    public Image hpFillEffect;

    public float hp;
    public float maxHp;
    [SerializeField] float hurtSpeed = 0.005f;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        hpImage.fillAmount = hp / maxHp;

        if(hpFillEffect.fillAmount > hpImage.fillAmount)
        {
            hpFillEffect.fillAmount -= hurtSpeed;
        }
        else
        {
            hpFillEffect.fillAmount = hpImage.fillAmount;
        }
    }
}
