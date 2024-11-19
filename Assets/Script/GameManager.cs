using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public Boardmanager boardManager;
    public PlayerController playerController;
    public TurnManager m_turnManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_turnManager = new TurnManager();

        boardManager.FloorGenerate();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
