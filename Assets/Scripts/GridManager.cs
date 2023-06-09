using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Cell[,] grid;
    
    
    [Header("Game")] 
    public int gridSize;
    public int pieceCount;

    private List<List<Cell>> pieces= new List<List<Cell>>();
    private List<Color> pieceColors = new List<Color>();

    void Start()
    {
        GenerateGrid();
        GeneratePieces();
        UpdatePieces();
    }

    private void UpdatePieces()
    {
        foreach (var piece in pieces.Take(1))
        {
            for (int i = 0; i < piece.Count; i++)
            {
                var currentCellOfPiece = piece[i];
                var differentNeighbours = GetAllDifferentNeighbour(currentCellOfPiece);
                if (differentNeighbours.Count != 0)
                {
                    var randomNeighbour = differentNeighbours.ElementAt(UnityRandom.Range(0, differentNeighbours.Count-1));
                    currentCellOfPiece.ExpandToOtherCell(randomNeighbour);
                    
                }
            }
        }
        
    }

    private void GenerateGrid()
    {
        grid = new Cell[gridSize, gridSize];
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                var cell = Instantiate(cellPrefab, new Vector2(1 * x, 1 * y), quaternion.identity);
                cell.transform.SetParent(transform);
                grid[x, y] = cell.GetComponent<Cell>();
                grid[x, y].Init(x, y);
            }
        }
        
        GenerateRandomColor();     
    }
    
    private void GeneratePieces()
    {
        
        var sizes = GenerateRandomPieceSizes();
        for (int i = 0; i < sizes.Count; i++)
        {
            pieces.Add(GeneratePiece(sizes[i], pieceColors[i]));
        }
        
    }
    
    private List<int> GenerateRandomPieceSizes()
    {
        Random rand = new Random();

        // Generate 5 random numbers between 1 and 15
        List<int> numbers = new List<int>();
        for (int i = 0; i < 5; i++) {
            numbers.Add(rand.Next(1, 16));
        }

        // Sum the numbers
        int sum = numbers.Sum();

        // Check if the sum is 16
        if (sum == 16) {
            // Print the numbers
            foreach (int number in numbers) {
                Debug.Log(number);
            }
        } else {
            // Generate new numbers until the sum is 16
            while (sum != 16) {
                numbers.Clear();
                for (int i = 0; i < 5; i++) {
                    numbers.Add(rand.Next(1, 16));
                }
                sum = numbers.Sum();
            }
            
        }

        return numbers;
    }

    private List<Cell> GeneratePiece(int pieceSize, Color color)
    {
        var piece = new List<Cell>();
        
        var emptyCells = GetEmptyCells();
        
        Cell cell = emptyCells[0]; 
        for (int p = 0; p < pieceSize; p++)
        { 
            cell.SetColor(color);
            cell.IsEmpty = false;
            
            piece.Add(cell);

            var neighbour = GetRandomNeighbour(cell);
            if (neighbour != null)
            {
                cell = neighbour;
            }

        }
        
        return piece;
    }

    private void GenerateRandomColor()
    {
        for (int i = 0; i < pieceCount; i++)
        {
            float r = UnityRandom.Range(0f, 1f);
            float g = UnityRandom.Range(0f, 1f);
            float b = UnityRandom.Range(0f, 1f);
            pieceColors.Add(new Color(r, g, b));
        }
    }

    private List<Cell> GetEmptyCells()
    {
        List<Cell> emptyCells = new List<Cell>();
        foreach (var cell in grid)
        {
            if (cell.IsEmpty)
            {
                emptyCells.Add(cell);
            }
        }

        return emptyCells;
    }

    private Dictionary<string, Cell> GetAllDifferentNeighbour(Cell cell)
    {
        var neighbours = new Dictionary<string, Cell>();
        
        if (IsValidCell(cell.x, cell.y + 1) && grid[cell.x, cell.y + 1].upTriangle.color != cell.upTriangle.color) neighbours.Add("up", grid[cell.x, cell.y + 1]);
        if (IsValidCell(cell.x, cell.y - 1) && grid[cell.x, cell.y - 1].downTriangle.color != cell.downTriangle.color) neighbours.Add("down", grid[cell.x, cell.y - 1]);
        if (IsValidCell(cell.x - 1, cell.y) && grid[cell.x - 1, cell.y].leftTriangle.color != cell.leftTriangle.color) neighbours.Add("left", grid[cell.x - 1, cell.y]);
        if (IsValidCell(cell.x + 1, cell.y) && grid[cell.x + 1, cell.y].rightTriangle.color != cell.rightTriangle.color) neighbours.Add("right", grid[cell.x + 1, cell.y]);
        
        return neighbours;
    }
    
    private Cell GetRandomNeighbour(Cell cell)
    {
        var neighbours = new List<Cell>();
        
        if (IsValidCell(cell.x, cell.y + 1) && grid[cell.x, cell.y + 1].IsEmpty) neighbours.Add(grid[cell.x, cell.y + 1]);
        if (IsValidCell(cell.x, cell.y - 1) && grid[cell.x, cell.y - 1].IsEmpty) neighbours.Add(grid[cell.x, cell.y - 1]);
        if (IsValidCell(cell.x - 1, cell.y) && grid[cell.x - 1, cell.y].IsEmpty) neighbours.Add(grid[cell.x - 1, cell.y]);
        if (IsValidCell(cell.x + 1, cell.y) && grid[cell.x + 1, cell.y].IsEmpty) neighbours.Add(grid[cell.x + 1, cell.y]);
        
        return neighbours.Count == 0 ? null : neighbours[UnityRandom.Range(0, neighbours.Count-1)];
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }
    
    
}
