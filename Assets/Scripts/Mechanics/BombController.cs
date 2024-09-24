using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject exeplosionEffect;
    [SerializeField] private GameObject fireBombEffect;
    [SerializeField] private GameObject smokeEffect;

    public AudioClip exeplosionClip;

   
    private bool isGrounded;
    private Rigidbody body;
    Vector3 currentPos = new Vector3(0.2f, 1.2f, -0.1f);
    private GameObject objFireEffect;

    private void Start()
    {
        body= GetComponent<Rigidbody>();
        objFireEffect = Instantiate(fireBombEffect);
        GameManager.Instance.listEffect.Add(objFireEffect);
    }
    private void Update()
    {
        if(body.velocity.y < -2)
        {
            isGrounded = true;
        }
        objFireEffect.transform.position = transform.position + currentPos;
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Dumbbells"))
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Ground") && isGrounded){
            Explosion();
            return;
        }
        // collision in Enemy
        if(other.gameObject.CompareTag("Gangster"))
        {
            Explosion();
            return;
        }
        if(other.gameObject.CompareTag("Zombie"))
        {
            Explosion();
            return;
        }
        
        if (other.gameObject.CompareTag("Player") )
        {
            Explosion();
            return;
        }
        if (other.gameObject.CompareTag("Pin")&& isGrounded)
        {
            Explosion();
        }
     
    }

    private void Explosion()
    {
        Instantiate(exeplosionEffect, transform.position, Quaternion.identity);
        
        
        SoundManager.instance.PlayAudioSound(exeplosionClip);
        Destroy(gameObject);
        Destroy(objFireEffect.gameObject);
    }
   
}
