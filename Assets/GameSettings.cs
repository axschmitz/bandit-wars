using System;
using UnityEngine;

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2,
    Impossible = 3,
}

public class GameSettings : MonoBehaviour
{
    public Difficulty difficulty;

    public static GameSettings instance;

    void Awake()
    {
        if (GameSettings.instance == null)
        {
            GameSettings.instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateDifficulty(float difficulty)
    {
        this.difficulty = (Difficulty) difficulty;
    }

    public void Test()
    {

    }
}
