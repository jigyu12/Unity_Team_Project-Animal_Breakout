using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDecoration : MonoBehaviour
{
    [SerializeField]
    private DecorationTile[] groundTiles;
    [SerializeField]
    private DecorationTile[] decorationTiles;


    public void UpdateDecoraionTiles(bool left, bool right, bool updateMesh)
    {

        for (int i = 0; i < 10; i++)
        {
            groundTiles[0].SetActiveDecorationTile(i, true);
            decorationTiles[0].SetActiveDecorationTile(i, true);

            groundTiles[1].SetActiveDecorationTile(i, true);
            decorationTiles[1].SetActiveDecorationTile(i, true);
        }

        if (left)
        {

            groundTiles[0].SetActiveDecorationTile(7, false);
            groundTiles[0].SetActiveDecorationTile(8, false);
            groundTiles[0].SetActiveDecorationTile(9, false);

            decorationTiles[0].SetActiveDecorationTile(7, false);
            decorationTiles[0].SetActiveDecorationTile(8, false);
            decorationTiles[0].SetActiveDecorationTile(9, false);
        }

        if (right)
        {
            groundTiles[1].SetActiveDecorationTile(7, false);
            groundTiles[1].SetActiveDecorationTile(8, false);
            groundTiles[1].SetActiveDecorationTile(9, false);

            decorationTiles[1].SetActiveDecorationTile(7, false);
            decorationTiles[1].SetActiveDecorationTile(8, false);
            decorationTiles[1].SetActiveDecorationTile(9, false);
        }


        if (updateMesh)
        {
            foreach (var deco in decorationTiles)
            {
                deco.UpdateTileMeshRandom();
            }

        }
    }
}
