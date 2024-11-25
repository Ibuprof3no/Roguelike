using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    private Boardmanager m_Board;
    private Vector2Int m_CellPosition;

    public void Spawn(Boardmanager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell);
        //el grid tiene un metodo para saber la posicion de una casilla, GetCellCenterWorld, pero esa información la tiene el BoardManager...
        //el boardmanager deberia ser la clase que lidia con toda la informacion relacionada al tablero, como convertir una casilla a una posición del mundo.
        //Para ello, necesitamos una referencia al grid, para poder usar GetCellCenterWorld, coger el componente con un getcomponent, y añadir un metodo que convierta 
        //el indice 2d de una celda y devuelva una posicion del mundo del centro de esa celda

        //mueve el personaje a el metodo que hemos hecho que es un vector 3 (la informacion que le damos es cell, que es la casilla)

    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
          
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
                MoveTo(newCellTarget);
                GameManager.Instance.turnManager.Tick();
            }
        }



    }

}
