using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Boardmanager : MonoBehaviour
{
    public class CellData
    {
        public bool passable;
        public GameObject ContainedObject;
    }
    public PlayerController playerController;
    private CellData[,] m_BoardData;
    private Grid m_Grid;
    public GameObject foodPrefab;
    public int ancho;
    public int alto;
    public Tile[] muro;
    private Tilemap m_Tilemap;
    public Tile[] suelos; // esto es un array [] y sirve para guardar muchas cosas de un tipo a la vez, por ejemplo, de un tilemap
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void FloorGenerate ()
    {
        m_BoardData = new CellData[ancho, alto];
        m_Tilemap = GetComponentInChildren<Tilemap>();
       
        m_Grid = GetComponentInChildren<Grid>();
        playerController.Spawn(this, new Vector2Int(1, 1));

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                m_BoardData[i, j] = new CellData();

                if ( i == 0 || i == ancho -1 || j == 0 || j == alto -1)
                {
                    m_Tilemap.SetTile(position, muro[Random.Range(0, muro.Length)]);
                    m_BoardData[i, j].passable = false;
                }

               else
                {
                    m_Tilemap.SetTile(position, suelos[Random.Range(0, suelos.Length)]);
                    m_BoardData[i, j].passable = true;
                }

               
              
           
            }
        }
   
     
    }
    public Vector3 CellToWorld (Vector2Int cellindex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellindex);
    }

    public CellData GetCellData (Vector2Int cellindex)
    {
        if (cellindex.x <0 || cellindex.x >= ancho || cellindex.y <0 || cellindex.y >= alto)
        {
            return null;
        }
        return m_BoardData[cellindex.x,cellindex.y];
    }

    //función/método que diga cuántos objetos de comida voy a spawnear, que por cada uno vaya buscando el escenario
    //(dentro de los muros) y que lo spawnee en una casilla aleatoria, si esa casilla está vacía y es pasable
    public void SpawnFood ()
    {
        for (i == 0 || i == ancho - 1 || j == 0 || j == alto - 1)
        {

        }
    }
    //que esta casilla ya no esté vacía
}
