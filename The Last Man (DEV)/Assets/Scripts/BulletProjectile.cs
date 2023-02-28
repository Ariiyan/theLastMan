using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody _bulletRigidbody;
    [SerializeField, Range(5f, 30f)] private float _speed;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _bulletRigidbody.velocity = transform.forward * _speed;
    }

    public float Speed()
    {
        return _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.tag);
        if(other.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
