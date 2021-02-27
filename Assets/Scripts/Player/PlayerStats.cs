using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public event Action playerDamage;
    public int Points = 0;
    public float Health = 100f;

    public bool isBurning = false;
    public float burningMultiplier = 2f;

    public bool isPoisoned = false;
    public float poisonMultiplier = 2f;

    bool bisAirborne = false;
    public float airbornePoints = 0;
    public int comboCount = 0;

    public float wallSplatMinSpeed = 5;

    void Start()
    {

    }

    void FixedUpdate()
    {
        // Probably not very performant
        if (bisAirborne)
        {
            if (GetComponent<PlayerController>().IsGrounded())
            {
                bisAirborne = false;
            }
            else
            {
                airbornePoints += 0.5f;
                playerDamage?.Invoke();
            }
        }
        else
        {
            if (!GetComponent<PlayerController>().IsGrounded())
                bisAirborne = true;
        }
    }

    // Called when the player takes damage
    public void TakeDamage(DamageInfo damageInfo)
    {
        Health -= Mathf.Round(damageInfo.damage);
        Points += (int)((damageInfo.pointsPerDamage + airbornePoints) * GetMultiplier()); // Apply points multiplier and airbone points, then round down
        airbornePoints = 0;
        comboCount++;

        playerDamage?.Invoke();

        //@TODO Use event playerDamage to implement UI

        if (Health <= 0)
        {
            // End game
        }
    }
    
    /// <summary>
    /// Get the current points multiplier
    /// </summary>
    /// <returns>float Multiplier</returns>
    public float GetMultiplier()
    {
        float currentMultiplier = 1;

        // Check for multipliers
        if (isBurning)
            currentMultiplier += burningMultiplier;
        if (isPoisoned)
            currentMultiplier += poisonMultiplier;

        currentMultiplier += comboCount;

        return currentMultiplier;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Reset airborne points and combo when touching the ground
        if (!collision.gameObject.GetComponent<DamageDealer>())
        {
            // Calculate wall splat damage
            Vector3 velocity = GetComponent<PlayerController>().GetVelocity();
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 splatSpeed = Vector3.Scale(velocity, normal);
            float speed = splatSpeed.magnitude;
            if (speed > wallSplatMinSpeed)
            {
                TakeDamage(new DamageInfo(speed, (int)(0.5f * speed)));
            }

            comboCount = 0;
            airbornePoints = 0;
        }
    }
}
