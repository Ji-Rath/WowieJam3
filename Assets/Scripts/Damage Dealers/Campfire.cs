using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : DamageDealer
{
    public float pushForce = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool CanDealDamage(GameObject player)
    {
        return true;
    }

    public override void OnPlayerCollide(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.isBurning = true;
        float camAngle = player.GetComponent<PlayerController>().camTransform.eulerAngles.y;
        Vector3 moveDirection = Quaternion.Euler(0f, camAngle, 0f) * Vector3.forward;
        player.GetComponent<Rigidbody>().AddForce(moveDirection * pushForce);
        playerStats.TakeDamage(GetDamageInfo());
    }
}
