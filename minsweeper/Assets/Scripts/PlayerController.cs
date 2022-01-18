using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int _wherePlayer;
    public bool _isEnd = false;
    public bool _isLock = false;
    public bool _isMap = false;

    // player movement
    public bool _isMoving = false;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float sensitivity = 2.0f;

    // player interactions
    private Door touchDoor;

    public Camera myCamera;

    Animator playerAnim;
    AudioSource playerAudioSource;
    [SerializeField] AudioClip walkclip;
    Rigidbody rigid;

    CanvasManager canvasManager;
    GameManager gameManager;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();

        canvasManager = FindObjectOfType<CanvasManager>();
        gameManager = FindObjectOfType<GameManager>();

        // Lock Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!_isEnd)
        {
            if (!_isLock)   // unlock
            {
                // 이동, 문 상호작용
                PlayerMove();
                MouseClick();
                PlayerRotate();
                if (Input.GetKeyDown(KeyCode.Z))
                    CursorUnLock();
            }
            else if(!_isMap)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                    CursorLock();
            }
            if (Input.GetKeyDown(KeyCode.M)) 
            {
                if (!_isMap)    // open teleport map ui
                {
                    CursorUnLock();
                    gameManager.TeleportUI(true);
                    _isMap = true;
                    _isLock = true;
                }
                else            // close teleport map ui
                {
                    CursorLock();
                    gameManager.TeleportUI(false);
                    _isMap = false;
                    _isLock = false;
                }
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

    private void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * sensitivity * mouseX);
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

    public void CursorUnLock()
    {
        _isLock = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CursorLock()
    {
        _isLock = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayerDie()
    {
        _isEnd = true;
        CursorUnLock();
    }
    public void PlayerGameClear()
    {
        _isEnd = true;
        CursorUnLock();
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
