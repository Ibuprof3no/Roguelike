using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : CellObject
{
    public int Health = 3;
    private int m_CurrentHEalth;



    private void Awake()
    {
        GameManager.Instance.m_turnManager.OnTick += EnemyTurnHappen;
    }

    private void OnDestroy()
    {
        GameManager.Instance.m_turnManager.OnTick -= EnemyTurnHappen;
    }
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHEalth = Health;
    }

    public override bool PlayerWantsToEnter()
    {
        m_CurrentHEalth -= 1;
        if(m_CurrentHEalth <= 0)
        {
            Destroy(gameObject);
        }
        return false;
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.boardManager;
        var targetCell = board.GetCellData(coord);

        if(targetCell == null || !targetCell.passable || targetCell.ContainedObject != null)
        {
            return false;
        }
        //desocupa la casilla actual
        var currentCell = board.GetCellData(m_Cell);
        currentCell.ContainedObject = null;

        //añadirlo a otra casilla
        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    void EnemyTurnHappen()
    {
        // Buscar la posición actual del jugador
        var playerCell = GameManager.Instance.playerController.CellPosition;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell .y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist ==0 && yDist == 1) || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
        }

        //lógica para ver si me muevo a la derecha, izquierda, arriba o abajo

        else
        {
            if(absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    //si no me puedo mover en x (ni atacar)
                    //entonces me muevo en la y
                    TryMoveInY(yDist);
                }
            }
            else if (!TryMoveInX(yDist))
            {
                TryMoveInX(xDist);
            }
        }
    }

    //método para moverme en x

    bool TryMoveInX (int xDist)
    {
        if(xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }

        //si el jugador está a la izquierda
        return MoveTo(m_Cell + Vector2Int.left);
    }
    //método para moverme en y+
    bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
        {
            //si el jugador está encima
            return MoveTo(m_Cell + Vector2Int.up);
        }

        //si el jugador está debajo
        return MoveTo(m_Cell + Vector2Int.down);
    }
  
}
