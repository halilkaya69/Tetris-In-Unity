using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CisimHareket : MonoBehaviour
{
    public Vector3 rotationPoint;//d�n�� noktas�
    private float previousTime;//ne zaman d��ece�ini hesaplamak i�in
    public float fallTime = 0.8f;//keyfi d��me s�resi
    public static int height = 20;//arkaplan boyu
    public static int width = 10;//arkaplan yatay uzunluk
    private static Transform[,] grid =new Transform[width, height];//�zgara

    public float sabit=0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fallTime-= sabit*Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftArrow))//sol hareket
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!HareketEdebilirMi())//her hareketten sonra d��ar� ��kt�m� diye kontrol et
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))//sa� hareket
        {
            transform.position += new Vector3(1, 0, 0);
            if (!HareketEdebilirMi())//her hareketten sonra d��ar� ��kt�m� diye kontrol et
                transform.position -= new Vector3(1, 0, 0);
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);//nesneyi belirli bir nokta etraf�nda belirli bir a��yla d�nd�r�r
            if (!HareketEdebilirMi())//her hareketten sonra d��ar� ��kt�m� diye kontrol et
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            
        }


        //  d��me s�resince a�a�� hareket ettir 
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime ))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!HareketEdebilirMi())//her hareketten sonra d��ar� ��kt�m� diye kontrol et
            {
                transform.position -= new Vector3(0, -1, 0);
                GrideEkle();
                Sat�r�KontrolEt();
                this.enabled = false;//a�a�� d���nce onun hareketini kapa
                FindObjectOfType<CisimSpawner>().CisimSpawn();//yenisini �a��r


            }
            previousTime = Time.time;//zaman� s�f�rla gibi
        }
    }

    void Sat�r�KontrolEt()
    {
        for(int i=height-1; i>=0; i--)//t�m sat�rlar� kontrol et
        {
            if (SatirVarMi(i))
            {
                Sat�r�Sil(i);
                AsagiKaydir(i);
            }
        }
    }

    bool SatirVarMi(int i)
    {
        for(int j = 0; j < width; j++)//t�m s�tunlar� kontrol et
        {
            if (grid[j, i] == null)//gridler dolumu diye kontrol et
            {
                return false;
            }
            
        }
        return true;
    }
    void Sat�r�Sil(int i)
    {
        for(int j = 0; j < width; j++)//noktalar� kontrol et
        {
            Destroy(grid[j, i].gameObject);//o noktay� temizle
            grid[j, i] = null;//noktay� bo�a �evir
        }
    }

    void AsagiKaydir(int i)
    {
        for(int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    void GrideEkle()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);//konumlar�n� inte �evir
            int roundedY = Mathf.RoundToInt(children.transform.position.y);//konumu inte �evir
            
            grid[roundedX, roundedY] = children;//gride x ve ynin int de�erlerini ekle

        }
    }

    bool HareketEdebilirMi()
    {
        foreach(Transform children in transform)//t�m par�alar� kontrol et
        {
            int roundedX=Mathf.RoundToInt(children.transform.position.x);//konumlar�n� inte �evir
            int roundedY=Mathf.RoundToInt(children.transform.position.y);//konumu inte �evir

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)//herhangi biri �er�eve d���ndaysa 
            {
                return false;//izin verme
            }
            if (grid[roundedX, roundedY] != null)
                return false;//de�erler gridde yoksa izin verme
            
        }
        return true;//s�k�nt� yoksa izin ver
    }


}
