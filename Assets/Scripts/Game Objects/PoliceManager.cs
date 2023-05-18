using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManager : MonoBehaviour
{
    private Animator policeAnimator;
    public GameObject bullets;
    public Transform bulletPivot;
    public float bulletForceAmount;
    public float shootInterval = 10;
    void Start()
    {
        policeAnimator = GetComponent<Animator>();
        InvokeRepeating("Shoot", 0, shootInterval);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnBeAttacked();
        }
    }

    private void Shoot()
    {
        GameObject obj = new GameObject();
        obj = Instantiate(bullets);
        obj.transform.position = bulletPivot.position;
        obj.transform.rotation = Quaternion.Euler(0, 180 * Random.Range(0, 2), 0);
        obj.GetComponent<Rigidbody>().AddForce(Vector3.left * bulletForceAmount);
        policeAnimator.SetTrigger("isShooting");
    }

    public void OnBeAttacked()
    {
        CancelInvoke();
        InvokeRepeating("Shoot", 0, shootInterval / 5);
        Invoke("ResetInvoke", 8);
    }

    private void ResetInvoke()
    {
        CancelInvoke();
        InvokeRepeating("Shoot", 0, shootInterval);
    }
}
