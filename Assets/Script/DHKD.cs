using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHKD : MonoBehaviour
{

    public float radi = 2f; // радиус атаки
    public float damage0 = 10f; // урон
    public float da = 0f; // урон
    public float a = 0f; // урон

    void Update()
    {
      

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("MiniRobbers");
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < radi)
                {
                    RM rm = enemy.GetComponent<RM>();
                    if (rm != null)
                    {
                        rm.TakeDamage(damage0); 
                    }
                }
            }
        
    }
}
