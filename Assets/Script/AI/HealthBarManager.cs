using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBar;

    public Animator healthAnim;

    public Slider healthSlider;

    public Image fillSlider;
    public Image backgroundSlider;

    private int hpLayer, hp;

    private readonly Color[] hpLayerColor = {Color.white, Color.red, Color.blue, Color.green };
        
    public void ShowHealthBar(int _hpLayer)
    {
        healthBar.SetActive(true);
        hpLayer = _hpLayer;
        hp = 100;

        StartCoroutine(AnimationBar());
    }

    private IEnumerator AnimationBar()
    {
        for (int i = 0; i < hpLayer; i++)
        {
            healthSlider.value = 0;
            fillSlider.color = hpLayerColor[i + 1];
            backgroundSlider.color = hpLayerColor[i];

            for (int j = 0; j <= 100; j++)
            {
                yield return new WaitForSeconds(.000000001f);
                healthSlider.value++;
            }
        }
    }

    public void Damaged(int _curHealth)
    {
        hp = _curHealth;

        if (hp <= 0 && hpLayer > 1)
        {
            hp = 100;
        }

        healthAnim.SetTrigger("hurt");
        healthSlider.value = hp;
    }
}
