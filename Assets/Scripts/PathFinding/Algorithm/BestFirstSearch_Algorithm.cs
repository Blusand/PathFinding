using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestFirstSearch_Algorithm : ISelectAlgorithm
{
    // 开启列表
    List<Node> openList = new List<Node>();
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // 曼哈顿距离
    float heuristic(Node node1, Node node2)
    {
        return (int)(new Vector2(node2.index.x - node1.index.x, node2.index.y - node1.index.y)).magnitude;
    }

    void BestFirstSearch(Node startNode, Node endNode)
    {
        openList.Add(startNode);

        while (openList.Count != 0)
        {
            var current = openList[0];

            // 找出代价最小的结点
            foreach (var t in openList)
            {
                if (t.H < current.H)
                    current = t;
            }

            /* 可视化操作 */
            Map.Instance.AddVisualStep(current, NodeType.HasVisited);

            // 当前结点已处理完毕
            openList.Remove(current);
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

                float costToNeighbor = heuristic(neighbor, endNode);

                // 新的代价更低或者这个结点还未访问过
                if (costToNeighbor < neighbor.H || !openList.Contains(neighbor))
                {
                    // 更新邻居的H代价
                    neighbor.H = costToNeighbor;
                    neighbor.father = current;

                    openList.Add(neighbor);

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