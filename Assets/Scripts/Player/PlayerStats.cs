using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public event Action playerDamage;
    [HideInInspector]
    public int Points = 0;
    public float Health = 100f;

    [Space]
    public float burningMultiplier = 2f;
    [HideInInspector]
    public bool isBurning = false;

    public float poisonMultiplier = 2f;
    [HideInInspector]
    public bool isPoisoned = false;
    

    [HideInInspector]
    bool bisAirborne = false;
    [HideInInspector]
    public float airbornePoints = 0;
    [HideInInspector]
    public int comboCount = 0;

    [Space]
    public float wallSplatMinSpeed = 5;
    public float airbornePointsRate = 1f;

    [Space]
    public GameObject deadUIPrefab;
    public GameObject playerStatsUIPrefab;

    AudioSource audioSource;
    public AudioClip CompleteComboSound;
    public AudioClip ballSplatSound;

    public AudioSource musicSource;

    void Start()
    {
        playerStatsUIPrefab = Instantiate(playerStatsUIPrefab);
        playerStatsUIPrefab.GetComponent<PlayerStatsUI>().playerStats = this;
        playerStatsUIPrefab.GetComponent<PlayerStatsUI>().Initialize();
        audioSource = gameObject.AddComponent<AudioSource>();
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
                airbornePoints += airbornePointsRate * Time.fixedDeltaTime;
                playerDamage?.Invoke();
            }
        }
        else
        {
            if (!GetComponent<PlayerController>().IsGrounded() && comboCount > 0)
                bisAirborne = true;
        }
    }

    private void CalculatePoints()
    {
        RaycastHit hit;
        if (GetComponent<PlayerController>().IsGrounded(out hit) && hit.collider.GetComponent<DamageDealer>() == null)
        {
            if (airbornePoints > 500f)
            {
                audioSource.PlayOneShot(CompleteComboSound, 0.25f);
            }

            // Apply gained points and reset stats
            Points += (int)airbornePoints;
            comboCount = 0;
            airbornePoints = 0;

            //Disable effects when finishing combo
            isBurning = false;
            isPoisoned = false;
        }
    }

    // Called when the player takes damage
    public void TakeDamage(DamageInfo damageInfo)
    {
        if (!IsAlive()) { return; } // Prevent additional damage/points after death
        Health -= Mathf.Round(damageInfo.damage);
        airbornePoints += (int)((damageInfo.pointsPerDamage + airbornePoints) * GetMultiplier()); // Apply points multiplier and airbone points, then round down
        comboCount++;

        playerDamage?.Invoke();

        // Show Dead UI screen
        if (!IsAlive())
        {
            playerStatsUIPrefab.SetActive(false);
            GameObject endScreen = Instantiate(deadUIPrefab);
            DeadUI deadUI = endScreen.GetComponent<DeadUI>();
            deadUI.DisplayScore(Points);
            musicSource.Stop();
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
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

            CalculatePoints();

            audioSource.PlayOneShot(ballSplatSound);

            playerDamage?.Invoke();
        }
    }
}
