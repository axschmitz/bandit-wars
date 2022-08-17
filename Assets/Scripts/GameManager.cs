using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public int startGold = 100;
    public float goldRatio = 3f;

    public GameObject arrow;
    public int arrowPrice = 20;

    public GameObject heavyBandit;
    public Vector2 banditSpawn = new Vector2(10, -4);
    public int heavyBanditPrice = 50;

    public GameObject heroKnight;
    public Vector2 heroSpawn = new Vector2(-8, -4);
    float nextHero = 0f;
    public float heroSpawnRatio = 0.1f;

    int currentGold;
    float nextGold = 0f;

    public Button spawnHeavyBandit;
    public Button shootSingleArrow;
    public Button spawnArrowRain;
    public Text goldAmount;

    public bool gameOver = false;
    bool arrowRainUsed = false;
    public Vector2[] arrowRainPositions;

    public Difficulty difficulty;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<GameSettings>())
        {
            this.difficulty = FindObjectOfType<GameSettings>().difficulty;
            this.heroSpawnRatio += (float)this.difficulty * 0.01f;
            this.heavyBanditPrice += (int)this.difficulty * 5;
            this.arrowPrice += (int)this.difficulty * 5;
            this.goldRatio += (float)this.difficulty * 0.5f;
        }

        this.currentGold = this.startGold;
        this.spawnHeavyBandit.GetComponentInChildren<Text>().text = this.heavyBanditPrice + " Gold";
        this.shootSingleArrow.GetComponentInChildren<Text>().text = this.arrowPrice + " Gold";
        this.goldAmount.text = this.startGold + " Gold";

        arrowRainPositions = new Vector2[] {
            new Vector2(5, 7), new Vector2(4, 8), new Vector2(3, 7), new Vector2(2, 8),
            new Vector2(1, 7), new Vector2(0, 8), new Vector2(-1, 7.8f), new Vector2(-2, 8.1f),
            new Vector2(-3, 7.4f), new Vector2(-4, 7.9f), new Vector2(-5, 8.2f), new Vector2(-6, 7.3f),
            new Vector2(6, 6.6f), new Vector2(7, 7.1f), new Vector2(-7, 6.8f)
        };
    }

    void FixedUpdate()
    {
        if (Time.fixedTime >= this.nextGold)
        {
            currentGold++;
            this.nextGold = Time.fixedTime + 1f / this.goldRatio;
            this.goldAmount.text = this.currentGold + " Gold";
        }

        if (Time.fixedTime >= this.nextHero && !this.gameOver)
        {
            Instantiate(heroKnight, heroSpawn, Quaternion.identity);
            this.nextHero = Time.fixedTime + 1f / this.heroSpawnRatio;
        }
    }
    public void OnSpawnArrow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x <= 8 && mousePos.x >= - 8 && mousePos.y <= 1.5f && !this.gameOver)
        {
            if (currentGold >= arrowPrice)
            {
                Instantiate(arrow, new Vector3(mousePos.x, 5, -1),
                    arrow.transform.rotation);
                currentGold -= arrowPrice;
            }
            else
            {
                Debug.Log("Not enough Gold!");
            }
        }
    }

    public void SpawnArrowRain()
    {
        if (arrowRainUsed) return;

        arrowRainUsed = true;
        Color arrowRain = spawnArrowRain.GetComponent<Image>().color;

        foreach (Image child in spawnArrowRain.GetComponentsInChildren<Image>())
        {
            child.color = new Color(1f, 1f, 1f, .3f);
        }
        spawnArrowRain.GetComponent<Image>().color = new Color(arrowRain.r, arrowRain.g, arrowRain.b, .3f);
        spawnArrowRain.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, .3f);

        foreach (Vector2 arrowPos in arrowRainPositions)
        {
            GameObject arrowRainElement = Instantiate(arrow, new Vector3(arrowPos.x, arrowPos.y, -1),
                    arrow.transform.rotation);
            arrowRainElement.GetComponent<Arrow>().enemyDamage = 1000;
            arrowRainElement.GetComponent<Arrow>().friendlyDamage = 1000;
        }
    }

    public void SpawnHeavyBandit()
    {
        if (currentGold >= heavyBanditPrice)
        {
            Instantiate(heavyBandit, banditSpawn, Quaternion.identity);
            currentGold -= heavyBanditPrice;
        }
        else
        {
            Debug.Log("Not enough Gold!");
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }
}
