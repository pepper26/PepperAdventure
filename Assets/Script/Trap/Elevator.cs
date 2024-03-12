using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform player;
    public Transform elevatorswitch;
    public Transform elevatorswitchUp;
    public Transform elevatorswitchDown;
    public Transform downpos;
    public Transform uppos;

    public float speed;
    bool iselevatordown;
    private PlayerControl controls;
    [SerializeField] private AudioSource active;
    [SerializeField] private PauseMenu pauseMenu;

    private void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseMenu>();
        controls = new PlayerControl();
        controls.Enable();
        controls.Action.Down.performed += ctx => StartElevator();
        transform.position = downpos.position;
    }

    void Start()
    {
        active.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (iselevatordown)
        {        
            transform.position = Vector2.MoveTowards(transform.position, uppos.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, downpos.position, speed * Time.deltaTime);
        }
        Active();
    }

    public void StartElevator()
    {
        if(Vector2.Distance(player.position, elevatorswitch.position) < 2.5f ||
           Vector2.Distance(player.position, elevatorswitchUp.position) < 1f ||
           Vector2.Distance(player.position, elevatorswitchDown.position) < 1f)
        {
            if(transform.position.y <= downpos.position.y)
            {
                iselevatordown = true;
            }
            else if(transform.position.y >= uppos.position.y)
            {
                iselevatordown = false;
            }
        }
    }

    private void Active()
    {
        if(transform.position.y >= uppos.position.y || transform.position.y <= downpos.position.y || pauseMenu.GameIsPause == true)
        {
            active.enabled = false;
        }
        else
        {
            active.enabled = true;
        }
    }
}
