using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBrick : NormalBlock, IBreakable
{
    CollisionManager collManager = GameObject.FindObjectOfType<CollisionManager>();
    public void Break()
    {
        GameObject.Destroy(spriteBlock);
        collManager.blocks.Remove(this);
        collManager.Collision -= BlockCollision;
        OnBreak();
    }

    public event Action OnBreak;
    public BreakableBrick(float w, float h, float rot, Vector2 pos, GameObject block, Color color) : base(w, h, rot, pos, block, color)
    {
        collManager.Collision += BlockCollision;
    }

    private void BlockCollision(Block obj)
    {
        if(obj == this)
        {
            Break();
        }
    }
}
