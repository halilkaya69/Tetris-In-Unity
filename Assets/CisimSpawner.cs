using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CisimSpawner : MonoBehaviour
{
    public GameObject[] Cisimler;//cisimleri al�caz

    // Start is called before the first frame update
    void Start()
    {
        CisimSpawn();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CisimSpawn()
    {
        Instantiate(Cisimler[Random.Range(0,Cisimler.Length)],transform.position,Quaternion.identity);
        //cisimleri var et   rasgele                spawner objesininoldu�u yerde  �retildi�i do�rultuda
    }
}
