using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for objects that deal damage to the player
public class DamageDealer : MonoBehaviour
{
    public DamageInfo damageInfo;
    protected AudioSource audioSource;
    public List<AudioClip> damageSound;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public DamageInfo GetDamageInfo()
    {
        return damageInfo;
    }

    /// <summary>
    /// Determines whether damage can be dealt on the supplied GameObject
    /// </summary>
    /// <param name="player">player to test</param>
    /// <returns>whether damage can be dealt</returns>
    public virtual bool CanDealDamage(GameObject player)
    {
        return true;
    }

    /// <summary>
    /// Function that is called when a player collides with this object
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual void OnPlayerCollide(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.TakeDamage(GetDamageInfo());
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject playerRef = collision.gameObject;
        PlayerStats playerStats = playerRef.GetComponent<PlayerStats>();
        if (playerStats != null && CanDealDamage(playerRef))
        {
            OnPlayerCollide(playerRef);
            if (damageSound != null)
                audioSource.PlayOneShot(GetRandomSound(damageSound));
        }
    }

    AudioClip GetRandomSound(List<AudioClip> soundList)
    {
        return soundList[Random.Range(0, soundList.Count - 1)];
    }
};

// Struct for holding damage and points 
public struct DamageInfo
{
    public float damage;
    public int pointsPerDamage;

    public DamageInfo(float damage, int pointsPerDamage)
    {
        this.damage = damage;
        this.pointsPerDamage = pointsPerDamage;
    }
};
