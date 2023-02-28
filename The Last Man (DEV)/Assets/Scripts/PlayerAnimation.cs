using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private ThirdPersonShooterController _ShootingController;
    
    private Animator _animator;
    private Vector2 moveSpeed;
    private bool jumping, shooting;

    private int moveXHash, moveYHash, jumpHash, shotHash;




    private void Start()
    {
        //_player = GetComponent<PlayerBehaviour>();
        _animator = GetComponent<Animator>();


        SetHash();
    }


    private void Update()
    {
        GetValues();
        AnimatePlayer();

    }


    private void GetValues()
    {
        moveSpeed = _player.GetPlayerMovement();
        jumping = _player.GetJumpLaunced();
        shooting = _ShootingController.GetShot();
    }

    private void AnimatePlayer()
    {
        _animator.SetFloat(moveXHash, moveSpeed.x, 0.1f, Time.deltaTime); 
        _animator.SetFloat(moveYHash, moveSpeed.y, 0.1f, Time.deltaTime); 
        _animator.SetBool(jumpHash, jumping);
        _animator.SetBool(shotHash, shooting);
    }

    private void SetHash()
    {
        moveXHash = Animator.StringToHash("MoveSpeedX");
        moveYHash = Animator.StringToHash("MoveSpeedY");
        jumpHash = Animator.StringToHash("Jump");
        shotHash = Animator.StringToHash("Shoot");
    }
}
