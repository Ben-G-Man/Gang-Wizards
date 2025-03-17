using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public float fadeDuration;
    public GameObject p1;
    public GameObject p2;
    private int p1KillCOunt = 0;
    private int p2KillCOunt = 0;

    public FadeInAndOut fadeController { get; private set; }
    
    public static GameEventManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("WARNING: Found multiple instances of GameEventManager in the scene!");
        }
        instance = this;

        fadeController = GetComponent<FadeInAndOut>();
    }

    public static void LoadScene(string sceneName)
    {
        instance.fadeController.SetNextScene(sceneName);
        instance.fadeController.FadeIn(instance.fadeDuration);
    }

    public static void OnPlayerDeath(GameObject deadPlayer)
    {
        if (GetPlayerNumber(deadPlayer) == 1)
        {
            PersistenceManager.instance.AddToKillCount(1, 1);
        }
        else
        {
            PersistenceManager.instance.AddToKillCount(2, 1);
        }
        LoadScene("Menu");
    }

    public static int GetPlayerNumber(GameObject checkedObject)
    {
        if (checkedObject == instance.p1) return 1;
        if (checkedObject == instance.p2) return 2;
        else return -1;
    }
}
