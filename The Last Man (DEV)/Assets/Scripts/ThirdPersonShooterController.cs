using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;


public class ThirdPersonShooterController : MonoBehaviour
{
    //[SerializeField] private CinemachineVirtualCamera _aimVirtualCamera;
    [SerializeField] private PlayerBehaviour _playerBehaviour;

    [SerializeField] private bool _aim, _shot;
    [SerializeField] private LayerMask _aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform _debugTransform, _spawnBulletPosition;
    [SerializeField] private GameObject _bulletProjectile;

    private GameObject _bullet;


    private void Start()
    {
        //_shot = _playerBehaviour.Shoot();
    }


    private void Update()
    {
        PlayerInput();

        if(_aim)
        {
           // _aimVirtualCamera.gameObject.SetActive(true);
        }
        else
        {
            //_aimVirtualCamera.gameObject.SetActive(false);
        }

        _shot = _playerBehaviour.Shoot();

        if(_playerBehaviour.Shoot())
        {
            _shot = true;
        }

        CastAimRay();
        AimAtOrientation(_debugTransform);
        ShootProjectile(_shot);
        //ShootProjectile(_shot, _debugTransform.position);
    }

    public bool GetShot()
    {
        return _shot;
    }

    private void PlayerInput()
    {
        _aim = Input.GetButtonDown("Aim");
        //_aim = Input.GetKey("Q");

        if(Input.GetButton("Aim"))
        {
            //print("this is happening");
            _aim = true;
        }
        else
        {
            _aim = false;
        }
    }


    private void CastAimRay()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _aimColliderLayerMask))
        {
            _debugTransform.position = raycastHit.point;
        }
    }

    private void ShootProjectile(bool shoot, Vector3 to)
    {
        if(shoot)
        {
            Vector3 aimDir = Vector3.forward;
            Quaternion aimOrientation = _spawnBulletPosition.rotation;
            //Instantiate(_bulletProjectile, _spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //Instantiate(_bulletProjectile, _spawnBulletPosition.position, aimOrientation);

            GameObject newBullet = Instantiate(_bulletProjectile, _spawnBulletPosition.position, aimOrientation);
            //GameObject newBullet = Instantiate(_bulletProjectile, _spawnBulletPosition.position, Quaternion.identity);

            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            BulletProjectile bulletBehaviour = newBullet.GetComponent<BulletProjectile>();
            float mySpeed = bulletBehaviour.Speed();

            bulletRb.transform.position =  Vector3.Lerp(_spawnBulletPosition.position, to, mySpeed);

            //_bulletProject = Instantiate(_bulletProjectile, _spawnBulletPosition);
            shoot = false;

        }
    }

    private void ShootProjectile(bool shoot)
    {
        if(shoot)
        {
            Quaternion aimOrientation = _spawnBulletPosition.rotation;
            //Instantiate(_bulletProjectile, _spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Instantiate(_bulletProjectile, _spawnBulletPosition.position, aimOrientation);

            shoot = false;
        }
    }

    private void AimAtOrientation(Transform target)
    {

        _spawnBulletPosition.LookAt(target, Vector3.forward);

    }
}
