using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLevel : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    private HealthEnemy healthEnemy;

    private void Start()
    {
        healthEnemy = GameObject.FindGameObjectWithTag("Boss").GetComponent<HealthEnemy>();
    }
    // Update is called once per frame
    void Update()
    {
        if(healthEnemy.currentHealthEnemy == 0)
        {
            itemPrefab.SetActive(true);
        }
    }
}
