using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BlockManager : MonoBehaviour
{
    public List<Block> blocks { private set; get; }
    [SerializeField]
    Ball ball;
    int level, countOfBreakableBricks;
    [SerializeField]
    GameObject visualBlock;
    void Start()
    {
        level = 0;
        countOfBreakableBricks = 0;
        LoadLevel();
    }
    void LoadLevel()
    {
        blocks = new List<Block>();
        TextAsset levelText = Resources.Load<TextAsset>(Path.Combine("Levels",SceneManager.GetActiveScene().name));
        StreamReader reader = new StreamReader(new MemoryStream(levelText.bytes));
        string s;
        do
        {
            s = reader.ReadLine();
            if (s == null || s == "")
            {
                break;
            }
            string[] arguments = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            float w = (float)Convert.ToDouble(arguments[0]);
            float h = (float)Convert.ToDouble(arguments[1]);
            float r = (float)Convert.ToDouble(arguments[2]);
            Vector2 pos = new Vector2((float)Convert.ToDouble(arguments[3]), (float)Convert.ToDouble(arguments[4]));
            bool isBreakable = arguments[5].Trim() == "+";
            blocks.Add(CreateBlock(w, h, r, pos, isBreakable));
        } while (s != "");
        GetComponent<CollisionManager>().SetBlocks(blocks);
        level++;
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
            countOfBreakableBricks++;
            BreakableBrick b = new BreakableBrick(w, h, r, pos, visualBlock, new Color(1, 0.957f, 0.251f, 1));
            b.OnBreak += brick_OnBreak;
            return b;
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

    private void brick_OnBreak()
    {
        countOfBreakableBricks--;
        if (countOfBreakableBricks == 0)
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
