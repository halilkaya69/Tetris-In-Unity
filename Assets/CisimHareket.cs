using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CisimHareket : MonoBehaviour
{
    public Vector3 rotationPoint;//dönüþ noktasý
    private float previousTime;//ne zaman düþeceðini hesaplamak için
    public float fallTime = 0.8f;//keyfi düþme süresi
    public static int height = 20;//arkaplan boyu
    public static int width = 10;//arkaplan yatay uzunluk
    private static Transform[,] grid =new Transform[width, height];//ýzgara

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
            if (!HareketEdebilirMi())//her hareketten sonra dýþarý çýktýmý diye kontrol et
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))//sað hareket
        {
            transform.position += new Vector3(1, 0, 0);
            if (!HareketEdebilirMi())//her hareketten sonra dýþarý çýktýmý diye kontrol et
                transform.position -= new Vector3(1, 0, 0);
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);//nesneyi belirli bir nokta etrafýnda belirli bir açýyla döndürür
            if (!HareketEdebilirMi())//her hareketten sonra dýþarý çýktýmý diye kontrol et
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            
        }


        //  düþme süresince aþaðý hareket ettir 
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime ))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!HareketEdebilirMi())//her hareketten sonra dýþarý çýktýmý diye kontrol et
            {
                transform.position -= new Vector3(0, -1, 0);
                GrideEkle();
                SatýrýKontrolEt();
                this.enabled = false;//aþaðý düþünce onun hareketini kapa
                FindObjectOfType<CisimSpawner>().CisimSpawn();//yenisini çaðýr


            }
            previousTime = Time.time;//zamaný sýfýrla gibi
        }
    }

    void SatýrýKontrolEt()
    {
        for(int i=height-1; i>=0; i--)//tüm satýrlarý kontrol et
        {
            if (SatirVarMi(i))
            {
                SatýrýSil(i);
                AsagiKaydir(i);
            }
        }
    }

    bool SatirVarMi(int i)
    {
        for(int j = 0; j < width; j++)//tüm sütunlarý kontrol et
        {
            if (grid[j, i] == null)//gridler dolumu diye kontrol et
            {
                return false;
            }
            
        }
        return true;
    }
    void SatýrýSil(int i)
    {
        for(int j = 0; j < width; j++)//noktalarý kontrol et
        {
            Destroy(grid[j, i].gameObject);//o noktayý temizle
            grid[j, i] = null;//noktayý boþa çevir
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
            int roundedX = Mathf.RoundToInt(children.transform.position.x);//konumlarýný inte çevir
            int roundedY = Mathf.RoundToInt(children.transform.position.y);//konumu inte çevir
            
            grid[roundedX, roundedY] = children;//gride x ve ynin int deðerlerini ekle

        }
    }

    bool HareketEdebilirMi()
    {
        foreach(Transform children in transform)//tüm parçalarý kontrol et
        {
            int roundedX=Mathf.RoundToInt(children.transform.position.x);//konumlarýný inte çevir
            int roundedY=Mathf.RoundToInt(children.transform.position.y);//konumu inte çevir

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)//herhangi biri çerçeve dýþýndaysa 
            {
                return false;//izin verme
            }
            if (grid[roundedX, roundedY] != null)
                return false;//deðerler gridde yoksa izin verme
            
        }
        return true;//sýkýntý yoksa izin ver
    }


}
