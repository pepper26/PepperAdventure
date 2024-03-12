using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private HealthEnemy healthEnemy;
    [SerializeField] private Image frontHealthBarEnemy;
    [SerializeField] private Image backHealthBarEnemy;
    [SerializeField] private Transform enemy;
    [SerializeField] private bool facingRight;
    private float lerpTimer;
    public float chipSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
        Flip();
    }
    void Flip()
    {
        if (enemy.transform.localScale.x == 1 && !facingRight || enemy.transform.localScale.x == -1 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    public void UpdateHealthUI()
    {
        float fillF = frontHealthBarEnemy.fillAmount;
        float fillB = backHealthBarEnemy.fillAmount;
        float hFraction = healthEnemy.currentHealthEnemy / healthEnemy.startingHealthEnemy;
        if (fillB > hFraction)
        {
            frontHealthBarEnemy.fillAmount = hFraction;
            backHealthBarEnemy.color = Color.blue;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBarEnemy.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBarEnemy.fillAmount = hFraction;
            backHealthBarEnemy.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBarEnemy.fillAmount = Mathf.Lerp(fillF, backHealthBarEnemy.fillAmount, percentComplete);
        }
    }
}
;
