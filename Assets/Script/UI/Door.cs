using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider2D door;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private GameObject canvasBoss;
    [SerializeField] private HeroKnightTest heroKnightTest;

    void Awake()
    {
        heroKnightTest = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnightTest>();
        door = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvasPlayer.SetActive(true);
            canvasBoss.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            door.isTrigger = false;
            canvasPlayer.SetActive(false);
            canvasBoss.SetActive(true);
            heroKnightTest.inputX = 0;
        }
    }
}
