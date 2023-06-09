using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public List<Cell> cells = new List<Cell>();
    private int size;

    public void Init(List<Cell> cells)
    {
        this.cells = cells;
        size = cells.Count;
    }


}
