using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TMP_Text healthText;
    public TMP_Text potentialPointsText;
    public TMP_Text totalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        playerStats.playerDamage += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + playerStats.Health;
        potentialPointsText.text = "PP: " + Mathf.RoundToInt(playerStats.airbornePoints) + " x " + playerStats.GetMultiplier();
        totalScoreText.text = "Score: " + playerStats.Points;
    }

    void OnDestroy()
    {
        playerStats.playerDamage -= UpdateUI;
    }
}
