using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyRoom : Room
{
    private void SideTriggerEnter(MoveDirection side, Collider2D other)
    {
        
        base.SideTriggerEnter(side, other);
    }
}
