using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImg;
    public Image hpEffectImg;

    public float maxHp = 100f;
    public float currentHp;
    public float buffTime = 0.5f;

    private Coroutine updateCoroutine;

    private void OnEnable()
    {
        EventManager.OnHurt += getHurt;
    }

    private void OnDisable()
    {
        EventManager.OnHurt -= getHurt;
    }

    private void Start()
    {
        currentHp = maxHp;
        UpdateHealthBar();
    }

    private void Update()
    {
        //gameObject.GetComponent<Transform>().LookAt(Camera.main.transform.position);
    }

    public void SetHealth(float health)
    {
        currentHp = Mathf.Clamp(health, 0f, maxHp);

        UpdateHealthBar();

        if (currentHp <= 0)
        {
            //Destroy(gameObject);//ËÀÍö
        }
    }

    public void IncreaseHealth(float amount)
    {
        SetHealth(currentHp + amount);
    }

    public void DecreaseHealth(float amount)
    {
        SetHealth(currentHp - amount);
    }

    private void UpdateHealthBar()
    {
        hpImg.fillAmount = currentHp / maxHp;
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpadateHpEffect());
    }

    private IEnumerator UpadateHpEffect()
    {
        float effectLength = hpEffectImg.fillAmount - hpImg.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < buffTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            hpEffectImg.fillAmount = Mathf.Lerp(hpImg.fillAmount + effectLength, hpImg.fillAmount, elapsedTime / buffTime);
            yield return null;
        }

        hpEffectImg.fillAmount = hpImg.fillAmount;
    }

    public void getHurt()
    {
        DecreaseHealth(10f);
    }
}
