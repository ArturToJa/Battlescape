using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public static PathCreator Instance { get; set; }

    [HideInInspector] public List<Tile> Path = new List<Tile>();

    [SerializeField] GameObject XPrefab;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ClearPath()
    {
        ClearPathVisuals();
        ClearPathData(); 
    }
#region ClearPath subfunctions

    void ClearPathData()
    {
        Path.Clear();
        foreach (Tile tile in Map.Board)
        {
            tile.isUnderEacs = false;
        }
    }

    void ClearPathVisuals()
    {        
        var PathVisuals = GameObject.FindGameObjectsWithTag("Eacs");
        for (int i = 0; i < PathVisuals.Length; i++)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(PathVisuals[i]);
            }
            else
                Destroy(PathVisuals[i]);
        }
    }
#endregion

    public void AddSteps(Tile start, Tile end)
    {
        ClearPath();        
        Path = GeneratePath(start, end);
        for (int i = 1; i < Path.Count; i++)
        {
            AddSingleStep(Path[i]);
        }
    }

#region AddSteps subfunctions

    List<Tile> GeneratePath(Tile start, Tile end)
    {
        Pathfinder.Instance.BFS(start, true);
        Stack<Tile> tileStack = new Stack<Tile>();
        tileStack.Push(end);
        while (!tileStack.Contains(start))
        {
            tileStack.Push(Pathfinder.Instance.Parents[Pathfinder.Instance.GetTilesX(tileStack.Peek()), Pathfinder.Instance.GetTilesZ(tileStack.Peek())]);
        }
        List<Tile> path = new List<Tile>();
        while (tileStack.Count > 0)
        {
            path.Add(tileStack.Pop());
        }
        return path;
    }
    void AddSingleStep(Tile step)
    {
        Instantiate(XPrefab, step.transform.position, Quaternion.identity);
        step.isUnderEacs = true;
    }
#endregion

    public List<Vector3> GetMovementPath()
    {
        List<Vector3> movementPath = new List<Vector3>();
        foreach (Tile X in Path)
        {
            movementPath.Add(X.transform.position);

        }
        return movementPath;
    }
}
