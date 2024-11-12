using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Boardmanager : MonoBehaviour
{
    public class CellData
    {
        public bool passable;
    }
    private CellData[,] m_BoardData;
    public int ancho;
    public int alto;
    public Tile[] muro;
    private Tilemap m_Tilemap;
    public Tile[] suelos; // esto es un array [] y sirve para guardar muchas cosas de un tipo a la vez, por ejemplo, de un tilemap
    // Start is called before the first frame update
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        FloorGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FloorGenerate ()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                if ( i == 0 || i == ancho -1 || j == 0 || j == alto -1)
                {
                    m_Tilemap.SetTile(position, muro[Random.Range(0, muro.Length)]);
                }

               else
                {
                    m_Tilemap.SetTile(position, suelos[Random.Range(0, suelos.Length)]);
                }

               
              
           
            }
        }

     
    }
}
