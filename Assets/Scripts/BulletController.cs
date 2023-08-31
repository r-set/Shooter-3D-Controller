using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private GameObject _bulletDecals;

    private float _bulletSpeed = 25f;
    private float _bulletDestroyTime = 3f;

    public Vector3 Target
    {
        get; set;
    }

    public bool Hit
    { 
        get; set;
    }

    private void OnEnable()
    {
        //Destroy(gameObject, _bulletDestroyTime);
        StartCoroutine(DectivateBulletAfterDelay (gameObject));
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, _bulletSpeed * Time.deltaTime);
       
        if (!Hit && Vector3.Distance(transform.position, Target) < 0.1f)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Instantiate(_bulletDecals, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private IEnumerator DectivateBulletAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(_bulletDestroyTime);
        obj.SetActive(false);
    }
}