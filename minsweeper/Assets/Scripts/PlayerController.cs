using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int _wherePlayer;
    public bool _isEnd = false;
    public bool _isLock = false;

    // player movement
    public bool _isMoving = false;
    [SerializeField] float moveSpeed = 3.0f;

    // player interactions
    private Door touchDoor;

    public Camera myCamera;

    Animator playerAnim;
    AudioSource playerAudioSource;
    [SerializeField] AudioClip walkclip;
    Rigidbody rigid;

    CanvasManager canvasManager;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();

        canvasManager = FindObjectOfType<CanvasManager>();
    }

    void Update()
    {
        if (!_isEnd)
        {
            if (!_isLock)
            {
                PlayerMove();
                MouseClick();
            }
        }
    }

    private void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            _isMoving = true;
            if (!playerAudioSource.isPlaying)
                playerAudioSource.PlayOneShot(walkclip);
            playerAnim.Play("Run");
            transform.Translate((new Vector3(h, 0, v) * moveSpeed) * Time.deltaTime);
        }
        else
            _isMoving = false;
    }

    private void MouseClick()
    {
        // left click
        if (Input.GetMouseButtonDown(0) && touchDoor)
        {
            touchDoor.DoorOpen(_wherePlayer);
            canvasManager.DoorInteractPanelOff();
        }
        // right click
        else if (Input.GetMouseButtonDown(1) && touchDoor)
        {
            touchDoor.DoorFlag(_wherePlayer);
        }
    }

    public void PlayerDie()
    {
        Debug.Log("게임 종료");
        _isEnd = true;
        // 마우스 잠금 해제/ 애니메이션
    }
    public void PlayerGameClear()
    {
        _isEnd = true;
        // 마우스 잠금 해제
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Room"))
        {
            _wherePlayer = other.GetComponent<Room>().GetRoomNum();
            canvasManager.SetScannerTo(other.GetComponent<Room>()._aroundBomb);
            Debug.Log("방에 들어감: " + other.GetComponent<Room>().GetRoomNum());
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            touchDoor = other.gameObject.GetComponent<Door>();
            canvasManager.DoorInteractPanelOn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            touchDoor = null;
            canvasManager.DoorInteractPanelOff();
        }
    }
}
