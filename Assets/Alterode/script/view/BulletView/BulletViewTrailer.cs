using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletViewTrailer : BulletView
{
    public TrailRenderer trail;

    float originWidth = 0;

    void Start()
    {
        if(originWidth==0)originWidth = trail.widthMultiplier;
    }

    public override void Update()
    {
        base.Update();

        if (bullet != null)
        {
            trail.widthMultiplier = ((bullet.largePercent + 100f) / 100) * originWidth;
        }
    }
}
