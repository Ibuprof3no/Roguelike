using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool m_isMoving;
    public float MoveSpeed = 5f;
    private Vector3 m_MoveTarget;
    private Boardmanager m_Board;
    public Vector2Int CellPosition;
    private bool m_IsGameOver;
    private Animator m_Animator;

    private void Awake()
    {
       m_Animator = GetComponent<Animator>();
    }
    public void Init()
    {
        m_IsGameOver = false;  
        m_isMoving = false;
    }
    public void Spawn(Boardmanager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell, true);
        //el grid tiene un metodo para saber la posicion de una casilla, GetCellCenterWorld, pero esa información la tiene el BoardManager...
        //el boardmanager deberia ser la clase que lidia con toda la informacion relacionada al tablero, como convertir una casilla a una posición del mundo.
        //Para ello, necesitamos una referencia al grid, para poder usar GetCellCenterWorld, coger el componente con un getcomponent, y añadir un metodo que convierta 
        //el indice 2d de una celda y devuelva una posicion del mundo del centro de esa celda

        //mueve el personaje a el metodo que hemos hecho que es un vector 3 (la informacion que le damos es cell, que es la casilla)

    }
   
    public void MoveTo(Vector2Int cell, bool inmediate)//refactoriación del método que sirve para moverse
    {
        CellPosition = cell;
        if(inmediate)
        {
            m_isMoving = false;
            transform.position = m_Board.CellToWorld(CellPosition);
        }
        else
        {
            m_isMoving = true;
            m_MoveTarget = m_Board.CellToWorld(CellPosition);
        }
        m_Animator.SetBool("Moving", m_isMoving);
    }
    public void GameOver()
    {
        m_IsGameOver = true;
    }

    
    private void Update()
    {
        Vector2Int newCellTarget = CellPosition; //cual es la nueva casilla a la que quieres moverte.
        bool hasMoved = false; //sirve para saber si te has movido o no
        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }
        if (m_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
            if (transform.position == m_MoveTarget)
            {
                m_isMoving = false;
                m_Animator.SetBool("Moving", false);
                var cellData = m_Board.GetCellData(CellPosition); //var significa variable
                if (cellData.ContainedObject != null) cellData.ContainedObject.PlayerEntered();
            }
            return;
        }
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) //pulsaciones de las teclas
        {
            newCellTarget.y += 1; //cambia a tu nuevo objetivo
            hasMoved = true;  //afirma que te has movido

        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            //comprueba si la nueva posición es pasable, y muevela si lo es.
            Boardmanager.CellData cellData = m_Board.GetCellData(newCellTarget);


                if (cellData != null && cellData.passable)
                {
                    GameManager.Instance.m_turnManager.Tick();

                    if (cellData.ContainedObject == null)
                    {
                        MoveTo(newCellTarget, false);
                    }
                    else if (cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newCellTarget, false);
                        
                    }
                   
                }

            
        }



    }
   
}