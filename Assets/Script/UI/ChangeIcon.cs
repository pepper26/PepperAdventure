using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIcon : MonoBehaviour
{
    [SerializeField] private Button downBtn;
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite active;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            downBtn.GetComponent<Image>().sprite = active;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            downBtn.GetComponent<Image>().sprite = down;
        }
    }
}
