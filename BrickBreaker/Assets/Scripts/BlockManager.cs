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
        string s = "";
        do
        {
            s = reader.ReadLine();
            if (s == null)
            {
                break;
            }
            string[] arguments = s.Split(new char[] {';' }, StringSplitOptions.RemoveEmptyEntries);
            float w = (float)Convert.ToDouble(arguments[0]);
            float h = (float)Convert.ToDouble(arguments[1]);
            float r = (float)Convert.ToDouble(arguments[2]);
            Vector2 pos = new Vector2((float)Convert.ToDouble(arguments[3]),(float)Convert.ToDouble(arguments[4]));
            Debug.LogError(w + " " + h + " " + r + " " + pos.x + " " + pos.y);
            blocks.Add(CreateBlock(w, h, r, pos));
        } while (s != "");
        GetComponent<CollisionManager>().SetBlocks(blocks);
    }

    Block CreateBlock(float w, float h, float r, Vector2 pos)
    {
        if (r > 90f)
        {
            r -= 90f;
            float tmp = w;
            w = h; h = w;
        }
        if(r == 90f || r == 0f)
        {
            Debug.Log("Normal ++");
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
