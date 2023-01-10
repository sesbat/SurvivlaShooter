using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    GameObject hudUi;

    Setting setting;
    TextMeshProUGUI score;
    Slider hpBar;
    bool isGameOver = false;

    public float SetHP {
        set { hpBar.value = value; }
        get { return hpBar.value; }
    }
    public int SetScore {
        set {
            score.text = $"SCORE:{value}";
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            setting = hudUi.transform.GetChild(4).GetComponent<Setting>();
            score = hudUi.GetComponentInChildren<TextMeshProUGUI>();
            hpBar = hudUi.GetComponentInChildren<Slider>();
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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            setting.gameObject.SetActive(!setting.gameObject.activeSelf);
        }
    }
}
