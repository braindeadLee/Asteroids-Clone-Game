using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCalculators
{
    //Functions to return spawn coordinates inside an outer boundary and outside an inner boundary or "safezone", in this case so an asteroid can't spawn immediately on top of the spaceship
    public Vector3 SquareBoundsCoordinates(float _InnerBoundsX, float _InnerBoundsY, float OuterBoundsX, float OuterBoundsY)
    {
        Vector3 parameters = new Vector3(0, 0, 0);

        //Random factor to decide if coordinates spawn Right or Up sides
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            parameters.x = Random.Range(_InnerBoundsX, OuterBoundsX);
            parameters.y = Random.Range(-OuterBoundsY, OuterBoundsY);

        }
        else
        {
            parameters.x = Random.Range(-OuterBoundsX, OuterBoundsX);
            parameters.y = Random.Range(_InnerBoundsY, OuterBoundsY);
        }

        // Random factor to decide if coordinates spawn in Up & Right sides or Bottom & Left sides
        random = Random.Range(0, 2);
        if (random == 0) parameters = -parameters;

        return parameters;
    }
}
