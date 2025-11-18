using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public float H;
    public int patroint;
    public int magazin;
    public int a;
    public int p;
}

public class NewBehaviourScript : MonoBehaviour
{

    public float speed = 5f;
    public Joystick joystick;
    private Rigidbody2D rb;
    public GameObject ButtonA;
    public GameObject ButtonP;
    private Vector3 initialScale;
    public GameObject V;
    public GameObject E;
    public GameObject X;

    public GameObject PositionWeapons;

    public GameObject Delete1;
    public GameObject Delete2;

    public GameObject Pistolet;
    public GameObject Aftomat;
    public GameObject Pistolet1;
    public GameObject Aftomat1;

    public GameObject Text;

    public GameObject PATRON;
    public Animator anime;
    public float radi = 2f;
    public int patroint = 0;
    public int magazin = 0;
    public int p = 0;
    public int a = 0;
    public TMP_Text uiText;
    public float H = 100;
    public float maxH = 100;
    public Slider healthBar;

    private string saveFilePath;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        ButtonA.SetActive(false);
        ButtonP.SetActive(false);
        Pistolet.SetActive(false);
        Aftomat.SetActive(false);

        if (healthBar != null)
        {
            healthBar.maxValue = maxH;
            healthBar.value = H;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
        LoadGame(); // Загружаем данные при старте игры
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        rb.velocity = direction * speed;
    }

    private IEnumerator Taymer()
    {
        X.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        X.SetActive(false);
        magazin -= 1;
        SaveGame(); // сохраняем после использования магазина
    }

    public void ML()
    {
        if (magazin > 0)
        {
            StartCoroutine(Taymer());
        }
    }

    public void Magzin()
    {
        magazin += patroint;
        patroint = 0;
        SaveGame(); // сохраняем после пополнения
    }

    public void A()
    {
        V.SetActive(true);
        E.SetActive(false);
        Delete1.SetActive(true);
        Delete2.SetActive(false);
        a = 1;
        p = 0;
        SaveGame(); // сохраняем смену оружия
    }

    public void P()
    {
        V.SetActive(false);
        E.SetActive(true);
        Delete1.SetActive(false);
        Delete2.SetActive(true);
        a = 0;
        p = 1;
        SaveGame(); // сохраняем смену оружия
    }

    public void Delet1()
    {
        Pistolet1.transform.position = PositionWeapons.transform.position;
        Pistolet.SetActive(false);
        ButtonP.SetActive(false);
        Delete1.SetActive(false);
        Delete2.SetActive(false);
        Pistolet1.SetActive(true);
        E.SetActive(false);
        SaveGame();
    }

    public void Delet2()
    {
        Aftomat1.transform.position = PositionWeapons.transform.position;
        Aftomat.SetActive(false);
        ButtonA.SetActive(false);
        Delete1.SetActive(false);
        Delete2.SetActive(false);
        Aftomat1.SetActive(true);
        V.SetActive(false);
        SaveGame();
    }

    public void TakeDamage(float D)
    {
        H -= D;
        if (healthBar != null)
        {
            healthBar.value = H;
        }

        if (H <= 0)
        {
            Destroy(gameObject);
        }

        SaveGame(); // сохраняем после получения урона
    }

    void Update()
    {
        uiText.text = patroint.ToString();

        Text.SetActive(patroint > 0);
        PATRON.SetActive(patroint > 0);

        if (magazin > 0)
        {
            ButtonA.SetActive(a == 1 && p == 0);
            ButtonP.SetActive(a == 0 && p == 1);
        }

        // сбор патронов
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Patroni");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < radi)
            {
                patroint += 34;
                Destroy(enemy);
                SaveGame(); // сохраняем после подбора патронов
            }
        }

        GameObject[] P = GameObject.FindGameObjectsWithTag("Pistolet");
        foreach (GameObject enemy in P)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= radi)
            {
                Pistolet1.SetActive(false);
                Pistolet.SetActive(true);
            }
        }

        GameObject[] A = GameObject.FindGameObjectsWithTag("Aftomat");
        foreach (GameObject enemy in A)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= radi)
            {
                Aftomat1.SetActive(false);
                Aftomat.SetActive(true);
            }
        }
    }

    // ------------------ Сохранение и загрузка ------------------

    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.H = H;
        data.patroint = patroint;
        data.magazin = magazin;
        data.a = a;
        data.p = p;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            H = data.H;
            patroint = data.patroint;
            magazin = data.magazin;
            a = data.a;
            p = data.p;

            if (healthBar != null) healthBar.value = H;

            if (a == 1) A();
            else if (p == 1) P();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame(); // автоматическое сохранение при выходе
    }
}
