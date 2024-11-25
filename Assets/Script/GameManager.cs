using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public Boardmanager boardManager;
    public PlayerController playerController;
    public TurnManager turnManager { get; private set; }
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private int m_FoodAmount =100;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;//Registra la llmada cuando le haces inoke
        boardManager.FloorGenerate();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = m_FoodAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public  void OnTurnHappen()
    {
        m_FoodAmount--;
        m_FoodLabel.text = m_FoodAmount.ToString(); 
    }
}
