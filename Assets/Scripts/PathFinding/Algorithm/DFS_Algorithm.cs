using System.Collections.Generic;
using System;

public class DFS_Algorithm : ISelectAlgorithm
{
    // 记录已经遍历过的结点
    HashSet<Node> closeList = new HashSet<Node>();
    // 记录步数
    int step = 0;

#region shuffle
    Random ran = new Random();

    // 交换List中的两个元素
    void Swap(List<Node> list, int index1, int index2)
    {
        Node temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    // 洗牌算法
    List<Node> Shuffle(List<Node> list)
    {
        for (int i = 0; i < list.Count; ++i)
            Swap(list, i, ran.Next(i, list.Count));
        return list;
    }
#endregion

    bool DFS(Node startNode, Node endNode)
    {
        // 找到目标，结束递归
        if (startNode == endNode)
            return true;
        
        /* 可视化操作 */
        Map.Instance.AddVisualStep(startNode, NodeType.HasVisited);

        // 记录已经遍历过的结点
        closeList.Add(startNode);

        foreach (var neighbor in Map.Instance.Neighbor(startNode, Map.Instance.directions4))
        {
            // 该结点还未遍历过
            if (!closeList.Contains(neighbor))
            {
                neighbor.G = ++step;
                neighbor.father = startNode;
            
                if (DFS(neighbor, endNode))
                    return true;
            }
        }

        return false;
    }

    public void StartAlgorithm(Node startNode, Node endNode)
    {
        DFS(startNode, endNode);
    }
}