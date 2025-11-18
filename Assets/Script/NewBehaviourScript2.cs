using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript2 : MonoBehaviour
{
    public float radi = 2f; // радиус атаки
    public GameObject X;
    public GameObject MN;
    public int N = 0;
    public DHKD rm;
    public NewBehaviourScript newbehaviourscript;

    void Start()
    {
        X.SetActive(false);

    }

    

    void Update()
    {
        
        
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < radi)
                {
                    X.SetActive(true);
                }

                else if (distance > radi)
                {
                    X.SetActive(false);
                    
                }
            }
        
        
    }
}
