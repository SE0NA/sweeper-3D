using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    public int _wherePlayer;
    public bool _isStopAll = false;
    public bool _isLock = false;
    public bool _isMap = false;
    public bool _nearEnemy = false;

    // player movement
    public bool _isMoving = false;
    private bool _isJumping = false;
    float moveSpeed;
    float jumpPower = 4f;
    float sensitivity = 3.0f;

    // player interactions
    private Door touchDoor;

    [SerializeField] Camera myCamera;

    Animator playerAnim;
    AudioSource playerAudioSource;
    [SerializeField] AudioClip walkclip;
    [SerializeField] AudioClip jumpclip;
    Rigidbody rigid;

    [SerializeField] ParticleSystem vfx_blood;
    [SerializeField] ParticleSystem vfx_clear;

    CanvasManager canvasManager;
    GameManager gameManager;
    InGameMenuBtn ingameMenuBtn;
    MinimapManager minimapManager;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        playerAnim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();

        canvasManager = FindObjectOfType<CanvasManager>();
        gameManager = FindObjectOfType<GameManager>();
        ingameMenuBtn = FindObjectOfType<InGameMenuBtn>();
        minimapManager = FindObjectOfType<MinimapManager>();

        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rigid);
        }

        // Lock Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _wherePlayer = 12;
        moveSpeed = (float)PhotonNetwork.CurrentRoom.CustomProperties["player_speed"];
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        ESCMenu();
        if (!_isStopAll)
        {
            minimapManager.SetMiniMap(gameObject.transform);
            if (!_isLock)   // unlock
            {
                // 이동, 문 상호작용
                PlayerMove();
                PlayerJump();
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
                if (!_isMap && !_nearEnemy)    // open teleport map ui
                {
                    CursorUnLock();
                    ingameMenuBtn.PlayBtnClip(1);
                    gameManager.TeleportUI(true);
                    _isMap = true;
                    _isLock = true;
                }
                else if (_isMap)            // close teleport map ui
                {
                    TeleportUIClose();
                }
                else if (_nearEnemy)
                {
                    canvasManager.NearEnemyPanelOn();
                }
            }
        }
    }

    public void TeleportUIClose()
    {
        CursorLock();
        ingameMenuBtn.PlayBtnClip(2);
        gameManager.TeleportUI(false);
        _isMap = false;
        _isLock = false;
    }

    private void ESCMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isStopAll)
            {
                ingameMenuBtn.PlayBtnClip(1);
                _isStopAll = true;
                CursorUnLock();
                canvasManager.ESCMenu(true);
            }
            else
            {
                ingameMenuBtn.PlayBtnClip(2);
                gameContinue();
            }
        }
    }
    public void gameContinue()
    {
        _isStopAll = false;
        CursorLock();
        canvasManager.ESCMenu(false);
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
            if (PV.IsMine)  playerAnim.SetBool("isMoving", true);
            transform.Translate((new Vector3(h, 0, v) * moveSpeed) * Time.deltaTime);
        }
        else
        {
            _isMoving = false;
            if (PV.IsMine) playerAnim.SetBool("isMoving", false);
        }
    }
    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!_isJumping) {
            _isJumping = true;
            playerAudioSource.PlayOneShot(jumpclip);
            if (PV.IsMine) playerAnim.SetBool("isJumping", true);
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
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
            PV.RPC("RPC_DoorOpen", RpcTarget.AllBufferedViaServer, _wherePlayer, touchDoor._doornum);
        }
        // right click
        else if (Input.GetMouseButtonDown(1) && touchDoor)
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["enable_flag"])
                PV.RPC("RPC_DoorFlag", RpcTarget.AllBufferedViaServer, _wherePlayer, touchDoor._doornum);
        }
    }
    
    [PunRPC]
    public void RPC_DoorOpen(int rnum, int dnum)
    {
        gameManager.DoorOpenByNum(rnum, dnum);
        canvasManager.DoorInteractPanelOff();
    }
    [PunRPC]
    public void RPC_DoorFlag(int rnum, int dnum)
    {
        gameManager.DoorFlagByNum(rnum, dnum);
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
        _isStopAll = true;

        gameObject.layer = 0;
        playerAnim.SetTrigger("Die");
        if (PV.IsMine)
        {   
            CursorUnLock();
            myCamera.GetComponent<Animation>().Play();
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
        vfx_blood.Play();
    }
    public void PlayerGameClear()
    {
        _isStopAll = true;

        gameObject.layer = 0;
        myCamera.GetComponent<Animation>().Play();
        vfx_clear.Play();
        CursorUnLock();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isJumping = false;
            if (PV.IsMine) playerAnim.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Room"))
        {
            _wherePlayer = other.GetComponent<Room>().GetRoomNum();
            if (canvasManager == null) canvasManager = FindObjectOfType<CanvasManager>();
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
