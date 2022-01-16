using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int _wherePlayer;
    public bool _isEnd = false;

    // player movement
    public bool _isMoving = false;
    private bool _isJumping = false;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpPower = 2.0f;

    // player interactions
    private Door touchDoor;

    public Camera myCamera;

    Animator playerAnim;
    AudioSource playerAudioSource;
    [SerializeField] AudioClip walkclip;
    [SerializeField] AudioClip jumpclip;
    private Rigidbody rigidbody;

    CanvasManager canvasManager;

    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        canvasManager = FindObjectOfType<CanvasManager>();
    }

    void Update()
    {
        if (!_isEnd)
        {
            PlayerMove();
            PlayerJump();
            MouseClick();
        }
    }

    private void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            _isMoving = true;
            if (!_isJumping)
            {
                if (!playerAudioSource.isPlaying)
                    playerAudioSource.PlayOneShot(walkclip);
                playerAnim.Play("Run");
            }
            transform.Translate((new Vector3(h, 0, v) * moveSpeed) * Time.deltaTime);
        }
        else
            _isMoving = false;
    }
    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            playerAudioSource.Stop();
            playerAudioSource.PlayOneShot(jumpclip);

            playerAnim.Play("Jump");
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void MouseClick()
    {
        // left click
        if (Input.GetMouseButtonDown(0) && touchDoor)
        {
            _isEnd = touchDoor.DoorOpen(_wherePlayer);
        }
        // right click
        else if (Input.GetMouseButtonDown(1) && touchDoor)
        {
            touchDoor.DoorFlag(_wherePlayer);
        }

        if (_isEnd)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Debug.Log("게임 종료");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
            _isJumping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Room"))
        {
            _wherePlayer = other.GetComponent<Room>().GetRoomNum();
            canvasManager.SetScannerTo(other.GetComponent<Room>()._aroundBomb);
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
