using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RM : MonoBehaviour
{
    public float H = 100;
    public float maxH = 100;           // ← добавлено
    public Slider healthBar;           // ← добавлено

    public float radi;
    public float speedZombi;
    public Transform Player;
    public float damage0 = 10f;
    public GameObject PAtronmm;

    void Start()
    {
        // ← настройка полосы здоровья
        if (healthBar != null)
        {
            healthBar.maxValue = maxH;
            healthBar.value = H;
        }
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < radi)
            {
                NewBehaviourScript rm = enemy.GetComponent<NewBehaviourScript>();
                if (rm != null)
                {
                    rm.TakeDamage(damage0);
                }
            }
            else if (distance > radi)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    Player.position,
                    speedZombi * Time.deltaTime
                );
            }
        }
    }

    public void TakeDamage(float D)
    {
        H -= D;

        // ← обновляем полосу здоровья
        if (healthBar != null)
        {
            healthBar.value = H;
        }

        if (H <= 0)
        {
            Instantiate(PAtronmm, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
