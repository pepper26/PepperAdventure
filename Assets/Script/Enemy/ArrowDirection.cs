using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    void Update()
    {
        transform.localScale = enemy.localScale;
    }
}
