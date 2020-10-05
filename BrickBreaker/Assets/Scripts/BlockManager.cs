using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public List<Block> blocks { private set; get; }
    [SerializeField]
    Ball ball;
    [SerializeField]
    GameObject visualBlock;
    void Start()
    {
        blocks = new List<Block>();
        StreamReader reader = new StreamReader(Path.Combine(Application.dataPath, "C:\\Users\\Богдан\\Desktop\\Blocks.txt"));
        string s;
        do
        {
            s = reader.ReadLine();
            if (s == null || s=="")
            {
                break;
            }
            string[] arguments = s.Split(new char[] {';' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.LogWarning(s);
            float w = (float)Convert.ToDouble(arguments[0]);
            float h = (float)Convert.ToDouble(arguments[1]);
            float r = (float)Convert.ToDouble(arguments[2]);
            Vector2 pos = new Vector2((float)Convert.ToDouble(arguments[3]),(float)Convert.ToDouble(arguments[4]));
            bool isBreakable = arguments[5].Trim() == "+";
            Debug.LogError(arguments[5]);
            blocks.Add(CreateBlock(w, h, r, pos, isBreakable));
        } while (s != "");
        GetComponent<CollisionManager>().SetBlocks(blocks);
    }

    Block CreateBlock(float w, float h, float r, Vector2 pos, bool isBreakable)
    {
        r %= 180f; 
        if (r >= 90f)
        {
            r -= 90f;
            float tmp = w;
            w = h;
            h = tmp;
        }
        if (isBreakable)
        {
            return new BreakableBrick(w, h, r, pos, visualBlock, Color.green);
        }
        if(r == 0f)
        {
            return new NormalBlock(w,h,r,pos, visualBlock);
        }
        else
        {
            return new RotatedBlock(w, h, r, pos, visualBlock);
        }
    }

    void Update()
    {
        
    }
}
