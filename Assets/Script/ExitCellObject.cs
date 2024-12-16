using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile ExitTile;
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        GameManager.Instance.boardManager.SetCellTile(coord, ExitTile);
    }
    public override void PlayerEntered()
    {
        Debug.Log("Has entrado");
        GameManager.Instance.boardManager.DestroyWorld();

        GameManager.Instance.NewLevel();
    }
}
