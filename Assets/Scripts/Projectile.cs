using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    bool shot;

    public void Shoot()
    {
        shot = true;
        Destroy(gameObject, 3);
    }

    private void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

}
