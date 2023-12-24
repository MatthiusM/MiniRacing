using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedData : MonoBehaviour
{
    public static SavedData instance;

    private int coins;
    private int maxSpeed;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        coins = PlayerPrefs.GetInt("Coins", 0);
        maxSpeed = PlayerPrefs.GetInt("MaxSpeed", 10);
    }

    public int Coins
    {
        get { return coins; }
        private set
        {
            coins = value;
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
        }
    }

    public int MaxSpeed
    {
        get { return maxSpeed; }
        private set
        {
            maxSpeed = Mathf.Clamp(value, 10, 25);
            PlayerPrefs.SetInt("MaxSpeed", maxSpeed);
            PlayerPrefs.Save();
        }
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            return;
        }

        Coins += amount;
    }

    public bool UseCoins(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        if (coins >= amount)
        {
            Coins -= amount;
            return true;
        }
        
        return false;
    }

    public void IncreaseMaxSpeed(int increment)
    {
        if (increment > 0)
        {
            MaxSpeed += increment;
        }
    }
}
