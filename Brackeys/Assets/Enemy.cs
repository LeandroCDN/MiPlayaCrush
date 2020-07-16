using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealt = 100;
    int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealt;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Play hurt animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {

        Debug.Log("Enemy died!");
        animator.SetBool("IsDead", true);
        //Die Animation
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
    
