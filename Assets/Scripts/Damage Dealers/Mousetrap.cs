using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousetrap : DamageDealer
{
    bool bActivated = false;
    float LaunchForce = 100f;
    float TrapRadius = 25f;

    public override bool CanDealDamage(GameObject player)
    {
        return !bActivated;
    }

    public override void OnPlayerCollide(GameObject player)
    {
        if (!bActivated)
        {
            player.GetComponent<PlayerStats>().TakeDamage(GetDamageInfo());
            player.GetComponent<Rigidbody>().AddExplosionForce(LaunchForce, gameObject.transform.position, 25f);
        }
    }
}
