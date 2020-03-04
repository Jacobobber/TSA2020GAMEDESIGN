using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject enemyBullet;
    public GameObject player;
    private GameObject projectile;
    

    public float projectileSpeed = 30;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void EnemyFire()
    {
        Instantiate(enemyBullet, transform.position, transform.rotation);
        enemyBullet.GetComponent<Rigidbody>().AddForce(0, 0, 5);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);

            SendMessage("ChangeHealth", -10);
        }
    }
}
