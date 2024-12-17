using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;


public class Boardmanager : MonoBehaviour
{

    public WallObject WallPrefab;
    public ExitCellObject ExitCellPrefab;
    public class CellData

    {

        public bool passable;
        public CellObject ContainedObject;
    }
    public PlayerController playerController;
    private CellData[,] m_BoardData;
    private Grid m_Grid;
    [Header("Tamaño")]
    [Tooltip("Tamaño del mapa")]
    public int ancho = 8;
    public int alto = 8;
    [Header("Mapa")]
    [Tooltip("Componentes del mapa")]
    public Tile[] muro;
    private Tilemap m_Tilemap;
    public Tile[] suelos; // esto es un array [] y sirve para guardar muchas cosas de un tipo a la vez, por ejemplo, de un tilemap
    public FoodObject[] FoodPrefabs;
    public EnemyObject EnemyPrefab;
    private List<Vector2Int> m_EmptyCells;


    void Start()
    {

        //FloorGenerate();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        m_EmptyCells = new List<Vector2Int>();
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



                if (i == 0 || i == ancho - 1 || j == 0 || j == alto - 1)
                {
                    m_Tilemap.SetTile(position, muro[Random.Range(0, muro.Length)]);
                    m_BoardData[i, j].passable = false;
                }

                else
                {
                    m_Tilemap.SetTile(position, suelos[Random.Range(0, suelos.Length)]);
                    m_BoardData[i, j].passable = true;

                    m_EmptyCells.Add(new Vector2Int(i, j));
                }

                m_EmptyCells.Remove(new Vector2Int(1, 1));

            }
        }
        Vector2Int endCoord = new Vector2Int(alto - 2, ancho - 2);
        AddObject(Instantiate(ExitCellPrefab), endCoord);
        m_EmptyCells.Remove(endCoord);
        GenerateWall();
        GenerateFood();
        GenerateEnemies();
    }
    public Vector3 CellToWorld(Vector2Int cellindex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellindex);
    }

    public CellData GetCellData(Vector2Int cellindex)
    {
        if (cellindex.x < 0 || cellindex.x >= ancho || cellindex.y < 0 || cellindex.y >= alto)
        {
            return null;
        }
        return m_BoardData[cellindex.x, cellindex.y];
    }

    void GenerateFood()
    {
        int foodCount = 5;

        //pilla el numero de comidas y vusca un lugar aleatorio
        for (int i = 0; i < foodCount; i++)
        {
            int ramdomIndex = Random.Range(0, m_EmptyCells.Count);

            Vector2Int coord = m_EmptyCells[ramdomIndex];

            m_EmptyCells.RemoveAt(ramdomIndex);
            CellData data = GetCellData(coord);

            //instacias la comida y la mueves a los los lugares que as creado antes
            FoodObject newFood = Instantiate(FoodPrefabs[0]);
            newFood.transform.position = CellToWorld(coord);
            //dice a la casilla que ya no esta bacia
            data.ContainedObject = newFood;


        }

    }
    void GenerateWall()
    {
        int wallCount = Random.Range(5, 10);

        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(5, 10);
            Vector2Int coord = m_EmptyCells[randomIndex];


            m_EmptyCells.RemoveAt(randomIndex);       
            WallObject newWall = Instantiate(WallPrefab);
            AddObject(newWall, coord);
        }


    }

    void GenerateEnemies()
    {
        int enemiesCount = Random.Range(3, 6);
        for(int i = 0;i < enemiesCount;i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];
            m_EmptyCells.RemoveAt(randomIndex);
            EnemyObject newEnemy = Instantiate(EnemyPrefab);
            AddObject(newEnemy, coord);
        }
    }
    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }
    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }
    public void DestroyWorld()
    {
        for (int i = 0; i < ancho; i++)
        {

            for (int j = 0; j < alto; j++)
            {

                Vector3Int position = new Vector3Int(i, j, 0);
                m_Tilemap.SetTile(position, null);
                if(m_BoardData[i,j].ContainedObject != null)
                {
                    Destroy(m_BoardData[i, j].ContainedObject.gameObject);
                }
               
                //m_BoardData[i,j].ContainedObject = null;


            }
        }
    }
    public void Clean()
    {
        //si por lo que sea no hay informacion, pues para de limpiar
        if (m_BoardData == null) return;
        for (int i = 0; i < alto; ++i) //recorre las columnas del tablero
        {
            for (int x = 0; x < ancho; ++x) // recorre las filas del tablero
            {
                var cellData = m_BoardData[x, i];
                if(cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject);
                }
                SetCellTile(new Vector2Int(x, i), null);
            }
        }
    }
}

