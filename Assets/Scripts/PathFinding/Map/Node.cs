using UnityEngine;
using TMPro;
using System;

/// <summary>
/// 格子类型
/// </summary>
public enum NodeType
{
    Wall,   // 墙
    OutSide,    // 还未处理完的结点
    HasVisited, // 已经处理完的结点
    HasNotVisited,  // 可以走但是还未访问过的格子
    Start,  // 开始结点
    End,    // 结束结点
    Path,   // 路径颜色
}

/// <summary>
/// 格子类
/// </summary>
public class Node : MonoBehaviour, IComparable<Node>
{
    // 坐标
    public Vector2 index;
    // 代价
    [SerializeField]
    private float _F, _G, _H;
    public float F
    {
        set { _F = value; }
        get { return _F; }
    }
    public float G
    {
        set
        {
            if (_G != value)
            {
                _G = value;
                UpdateData();
            }
        }
        get { return _G; }
    }
    public float H
    {
        set
        {
            if (_H != value)
            {
                _H = value;
                UpdateData();
            }
        }
        get { return _H; }
    }
    // 父结点，用于寻找路径
    public Node father;
    // 格子的类型
    public NodeType type;

    // 面板显示
    [SerializeField]
    GameObject costPanel;

    // 数值面板
    [SerializeField]
    TextMeshPro F_Value, G_Value, H_Value; 

    // 放在Update中鬼导致错误
    void Awake()
    {
        InitialNode();
    }

    // 初始化数据
    public void InitialData()
    {
        F = 0;
        G = 0;
        H = 0;
        father = null;
        ShowPanel(false);
    }

    // 初始化结点
    public void InitialNode()
    {
        type = NodeType.HasNotVisited;
        ShowPanel(false);
        InitialData();
    }

    // 更新数据
    public void UpdateData()
    {
        // 计算F代价
        F = G + H;

        // 设置面板中的数值
        F_Value.text = F.ToString();
        G_Value.text = G.ToString();
        H_Value.text = H.ToString();
    }

    // 面板显示
    public void ShowPanel(bool activeState)
    {
        costPanel.SetActive(activeState);
    }

    // 比较函数，用于堆
    public int CompareTo(Node node)
    {
        if (F < node.F || (F == node.F && H < node.H))
            return -1;
        else
            return 1;
    }
}