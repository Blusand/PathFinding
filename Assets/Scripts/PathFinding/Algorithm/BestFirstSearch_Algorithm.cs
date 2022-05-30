using System.Collections.Generic;
using UnityEngine;

public class BestFirstSearch_Algorithm : ISelectAlgorithm
{
    // 开启列表
    PriorityQueue<Node> openList = new PriorityQueue<Node>(Compare<Node>.Greater);
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // 欧几里德距离
    float heuristic(Vector2 index1, Vector2 index2)
    {
        // 欧几里德距离
        return (int)(new Vector2(index2.x - index1.x, index2.y - index1.y)).magnitude;

        // 曼哈顿距离
        // return (Mathf.Abs(index1.x - index2.x)
        //     + Mathf.Abs(index1.y - index2.y)) * 10;
    }

    void BestFirstSearch(Node startNode, Node endNode)
    {
        openList.Push(startNode);

        while (openList.Count != 0)
        {
            Node current = openList.Top();

            /* 可视化操作 */
            Map.Instance.AddVisualStep(current, NodeType.HasVisited);

            // 当前结点已处理完毕
            openList.Pop();
            closeList.Add(current);

            // 检查当前结点是否是目标结点
            if (current == endNode)
                return;

            // 寻找当前结点的邻居结点（8个方向）
            foreach (var neighbor in Map.Instance.Neighbor(current, Map.Instance.directions8))
            {
                // 该结点已经处理完毕，跳过
                if (closeList.Contains(neighbor))
                    continue;
                
                float costToNeighbor = heuristic(neighbor.index, endNode.index);

                // 新的代价更低或者这个结点还未访问过
                if (costToNeighbor < neighbor.H || neighbor.H == 0)
                {
                    // 更新邻居的H代价
                    neighbor.H = costToNeighbor;
                    neighbor.father = current;

                    openList.Push(neighbor);

                    /* 可视化操作 */
                    Map.Instance.AddVisualStep(neighbor, NodeType.OutSide);
                }
            }
        }
    }

    public void StartAlgorithm(Node startNode, Node endNode)
    {
        BestFirstSearch(startNode, endNode);
    }
}