using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tilemap tileMap = null;
    [SerializeField] private Tile startTile = null;
    [SerializeField] private Tile goalTile = null;
    [SerializeField] private Tile wallTile = null;
    
    enum TileType {Start, Goal, Normal, Wall}
    private TileType currentTileType = TileType.Normal;

    [Header("Helper")] [SerializeField] private PosHolder positionHolder = null;
    [SerializeField] private float x1, y1, x2, y2;
    private Camera camera = null;

    [SerializeField] private UIChanger uiChanger = null;
    private void Awake()
    {
        camera = Camera.main;
        positionHolder.ResetPositions();
    }
    
    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            var mousepos = camera.ScreenToWorldPoint(Input.mousePosition);
            var pos = tileMap.WorldToCell(mousepos);
            if(IsInBounds(mousepos))
                tileMap.SetTile(pos, GetCurrentTile(pos));
        }
    }

    private bool IsInBounds(Vector3 pos)
    {
        return pos.x > x1 && pos.x < x2 && pos.y > y1 && pos.y < y2;
    }

    private Tile GetCurrentTile(Vector3Int mousePosition)
    {
        CheckIfTileOverlay(mousePosition);
        switch (currentTileType)
        {
            case TileType.Start:
                if(positionHolder.StartIsSet)
                    tileMap.SetTile(positionHolder.StartPosition, null);
                positionHolder.SetStart(mousePosition);
                return startTile;
            case TileType.Goal:
                if(positionHolder.GoalIsSet)
                    tileMap.SetTile(positionHolder.GoalPosition, null);
                positionHolder.SetGoal(mousePosition);
                return goalTile;
            case TileType.Wall:
                return wallTile;
            case TileType.Normal:
                return null;
            default:
                return null;
        }
    }

    private void CheckIfTileOverlay(Vector3Int mousePosition)
    {
        if (positionHolder.GoalIsSet && mousePosition == positionHolder.GoalPosition)
            positionHolder.GoalIsSet = false;
        else if (positionHolder.StartIsSet && mousePosition == positionHolder.StartPosition)
            positionHolder.StartIsSet = false;

    }

    public void SetTile(int i)
    {
        currentTileType = (TileType)i;
    }

    public void CheckForStart()
    {
        bool canStart = positionHolder.StartIsSet && positionHolder.GoalIsSet;
        if (canStart)
        {
            uiChanger.ActivateSim();
        }
    }
}
