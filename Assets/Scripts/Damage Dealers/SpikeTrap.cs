using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : DamageDealer
{
    float minVelocity = 5;

    public override bool CanDealDamage(GameObject player)
    {
        // Only allow damage over a set y velocity
        return player.GetComponent<Rigidbody>().velocity.y > minVelocity;
    }

    public override void OnPlayerCollide(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        int velocityPoints = (int)player.GetComponent<Rigidbody>().velocity.y;
        playerStats.TakeDamage(new DamageInfo(damageInfo.damage, damageInfo.pointsPerDamage + velocityPoints));
    }
}
