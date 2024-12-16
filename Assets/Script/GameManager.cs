using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int currentlevel;
    public static GameManager Instance { get; private set; }

    [Header("Controladores")]
    [Tooltip("Grid que contiene el script del board manager")]
    public Boardmanager boardManager;
    [Space(10)]
    [Tooltip("Objeto Player con el Script player controler dentro")]
    public PlayerController playerController;
    public TurnManager m_turnManager { get; private set; }

    [Header("Comida")]
    [Tooltip("Comida maxima del jugador")]
    [Range(0, 101)]
    public int m_FoodAmount = 100;

    [Header("UI")]
    [Tooltip("Documento UI Con Datos como La comida")]
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



    void Start()
    {
        m_turnManager = new TurnManager();
        //vincular metodos cadavez que llames al ontick az onturnhappen
        m_turnManager.OnTick += OnTurnHappen;
        Debug.Log("Comida Actual: " + m_FoodAmount);
        //vusca un elemento de tipo label dentro de el elemento raiz/origen llamado FoodLabel y cuando lo vusques eso va a ser apartir de ahora m_FoodLabel
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");

        NewLevel();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");
        m_GameOverPanel.style.visibility = Visibility.Hidden;
    }


    void Update()
    {

    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {


        m_FoodAmount += amount;
        m_FoodLabel.text = "Comida: " + m_FoodAmount;
        if (m_FoodAmount <= 0)
        {
            playerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over! \n\n Has avanzado a través de " + currentlevel + " niveles";
        }

    }
    public void NewLevel()
    {

        boardManager.Init();



        currentlevel++;
    }
    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        currentlevel = 1;
        m_FoodAmount = 60;
        m_FoodLabel.text = "Comida: " + m_FoodAmount;

        boardManager.Clean();
        boardManager.Init();
        playerController.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1)); //...como el personaje
    }

}