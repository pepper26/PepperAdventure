using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private bool flip;
    [SerializeField] private float speed;
    [SerializeField] private float space;
    [SerializeField] private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distance > space)
        {
            Target();
        }
        else if(distance <= space) 
        {
            anim.SetBool("Moving", false);
        }
    }


    private void Target()
    {
        Vector3 scale = transform.localScale;
        if (player.transform.position.x > transform.transform.position.x)
        {
            anim.SetBool("Moving", true);
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            anim.SetBool("Moving", true);
            scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
            transform.Translate(speed * Time.deltaTime * -1, 0, 0);
        }
        transform.localScale = scale;
    }
}

