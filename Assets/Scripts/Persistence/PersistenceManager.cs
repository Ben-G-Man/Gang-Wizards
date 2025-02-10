using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public Dictionary<int, int> killCount= new(); 
    public Dictionary<string, int> levelOwnership = new(); 

    public static PersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("WARNING: Found multiple instances of PersistenceManager in the scene!");
        }
        instance = this;
    }

    public void SaveData()
    {
        foreach (int player in killCount.Keys)
        {
            PlayerPrefs.SetInt(player.ToString(), killCount[player]);
            Debug.Log("Saved P" + player + " kill count as " + killCount[player]);
        }
        foreach (string level in levelOwnership.Keys)
        {
            PlayerPrefs.SetInt(level, levelOwnership[level]);
            Debug.Log("Saved " + level + " owner as " + levelOwnership[level]);
        }
        PlayerPrefs.Save();
    }

    public int LoadKillCount(int playerNumber)
    {
        int killCount = PlayerPrefs.GetInt(playerNumber.ToString(), 0);
        Debug.Log("Loaded P" + playerNumber + " kill count as " + killCount);
        return killCount;
    }

    public int AddToKillCount(int playerNumber, int kills)
    {
        killCount[playerNumber] += kills;
        return killCount[playerNumber];
    }

    public int RemoveFromKillCount(int playerNumber, int kills)
    {
        if (killCount[playerNumber] < kills)
        {
            return -1;
        }
        else
        {
            killCount[playerNumber] -= kills;
            return killCount[playerNumber];
        }
    }
}
