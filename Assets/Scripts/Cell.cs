using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    
    public SpriteRenderer leftTriangle;
    public SpriteRenderer upTriangle;
    public SpriteRenderer downTriangle;
    public SpriteRenderer rightTriangle;

    public bool IsEmpty = true;

    public Color baseColor;
    
    
    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public void SetColor(Color color)
    {
        baseColor = color;
        leftTriangle.color = color;
        upTriangle.color = color;
        downTriangle.color = color;
        rightTriangle.color = color;
    }

    public int CountDifferentColors()
    {
        var uniqueColors = new HashSet<Color>
        {
            leftTriangle.color,
            upTriangle.color,
            downTriangle.color,
            rightTriangle.color
        };

        return uniqueColors.Count;
    } 

    public void ExpandToOtherCell(KeyValuePair<string, Cell> otherCell)
    {
        switch (otherCell.Key)
        {
            case "left":
                otherCell.Value.rightTriangle.color = this.baseColor;
                break;
            case "up":
                otherCell.Value.downTriangle.color = this.baseColor;
                break;
            case "down":
                otherCell.Value.upTriangle.color = this.baseColor;
                break;
            case "right":
                otherCell.Value.leftTriangle.color = this.baseColor;
                break;
        }
    }
    
    
}
