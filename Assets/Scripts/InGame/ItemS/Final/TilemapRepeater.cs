using UnityEngine;

public class TilemapRepeater : MonoBehaviour
{
    public Transform player;
    public Vector2Int tileSize = new Vector2Int(20, 20);
    public GameObject tilePrefab;

    private GameObject[,] tiles = new GameObject[3, 3];
    private Vector2 currentCenterIndex = Vector2.zero;

    private void Start()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3 pos = new Vector3(x * tileSize.x, y * tileSize.y, 0);
                tiles[x + 1, y + 1] = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    private void Update()
    {
        Vector2 playerPos = player.position;
        Vector2 centerTileWorld = new Vector2(
            currentCenterIndex.x * tileSize.x,
            currentCenterIndex.y * tileSize.y
        );

        Vector2 offset = playerPos - centerTileWorld;

        if (Mathf.Abs(offset.x) > tileSize.x / 2f)
        {
            int shiftX = (int)Mathf.Sign(offset.x);
            currentCenterIndex.x += shiftX;
            ShiftTiles(shiftX, 0);
        }
        if (Mathf.Abs(offset.y) > tileSize.y / 2f)
        {
            int shiftY = (int)Mathf.Sign(offset.y);
            currentCenterIndex.y += shiftY;
            ShiftTiles(0, shiftY);
        }
    }

    void ShiftTiles(int shiftX, int shiftY)
    {
        GameObject[,] newTiles = new GameObject[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                int newX = x - shiftX;
                int newY = y - shiftY;

                if (newX >= 0 && newX < 3 && newY >= 0 && newY < 3)
                {
                    newTiles[x, y] = tiles[newX, newY];
                }
                else
                {
                    Vector3 pos = new Vector3(
                        (currentCenterIndex.x + x - 1) * tileSize.x,
                        (currentCenterIndex.y + y - 1) * tileSize.y,
                        0);
                    newTiles[x, y] = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                    Destroy(tiles[x, y]);
                }
            }
        }

        tiles = newTiles;
    }
}
