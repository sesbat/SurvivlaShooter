using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    GameObject hudUi;

    [SerializeField]
    GameObject hitUi;
    [SerializeField]
    GameObject gameOverObj;

    Setting setting;
    public bool isSetting {
        get { return setting.gameObject.activeSelf; }
    }
    int score = 0;
    TextMeshProUGUI scoreTxt;
    Slider hpBar;
    bool isGameOver = false;

    public void SetMaxHp(int max) => hpBar.maxValue = max;
    public float SetHP {
        set { hpBar.value = value; }
        get { return hpBar.value; }
    }

    public int AddScore {
        set {
            score += value;
            scoreTxt.text = $"SCORE:{score}";
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            setting = hudUi.transform.GetChild(4).GetComponent<Setting>();
            scoreTxt = hudUi.GetComponentInChildren<TextMeshProUGUI>();
            hpBar = hudUi.GetComponentInChildren<Slider>();
            AddScore = 0;
            instance = this;
        }
        else
            Destroy(instance);
    }

    [ContextMenu("GameOver")]
    public void GameOver()
    {
        var spawners = GameObject.FindObjectsOfType<ZombieSpawner>();
        foreach (var spawn in spawners)
        {
            Destroy(spawn);
        }

        var enemys = GameObject.FindObjectsOfType<Zombie>();
        foreach (var enemy in enemys)
        {
            enemy.GameOver();
        }
        gameOverObj.SetActive(true);
        Invoke("ResetGame", 5);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            setting.gameObject.SetActive(!setting.gameObject.activeSelf);
        }
    }

    public IEnumerator HitPlayerUi()
    {
        if(!hitUi.activeSelf)
        {
            hitUi.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitUi.SetActive(false);
        }
        yield return null;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
