using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create PositionHolder")]
public class PosHolder : ScriptableObject
{
    private Vector3Int startPosition;
    private bool startIsSet = false;
    private Vector3Int goalPosition;
    private bool goalIsSet = false;

    public Vector3Int StartPosition => startPosition;

    public Vector3Int GoalPosition => goalPosition;

    public bool StartIsSet
    {
        get => startIsSet;
        set => startIsSet = value;
    }

    public bool GoalIsSet
    {
        get => goalIsSet;
        set => goalIsSet = value;
    }

    public void SetStart(Vector3Int startPos)
    {
        startPosition = startPos;
        startIsSet = true;
    }

    public void SetGoal(Vector3Int goalPos)
    {
        goalPosition = goalPos;
        goalIsSet = true;
    }
    
    public void ResetPositions()
    {
        startPosition = new Vector3Int();
        goalPosition = new Vector3Int();
        startIsSet = false;
        goalIsSet = false;
    }
}
