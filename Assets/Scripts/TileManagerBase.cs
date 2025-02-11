using UnityEngine;

public class TileManagerBase : MonoBehaviour
{
    public virtual void StartTile(Vector2Int pos, int index)
    {
    }

    public virtual void AddTile(Vector2Int pos, int index)
    {
    }

    public virtual void EndTile()
    {
    }
}