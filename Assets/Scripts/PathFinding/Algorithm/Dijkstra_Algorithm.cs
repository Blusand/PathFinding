using System.Collections.Generic;
using UnityEngine;

public class Dijkstra_Algorithm : ISelectAlgorithm
{
    // 开启列表
    List<Node> openList = new List<Node>();
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // 结点之间的权值，这里为两个格子之间的距离*10
    int Weight(Vector2 index1, Vector2 index2)
    {
        return (int)((index1 - index2).magnitude * 10);
    }

    public void Dijkstra(Node startNode, Node endNode)
    {
        openList.Add(startNode);

        while (openList.Count != 0)
        {
            var current = openList[0];

            foreach (var t in openList)
            {
                if (t.G < current.G)
                    current = t;
            }

            /* 可视化操作 */
            Map.Instance.AddVisualStep(current, NodeType.HasVisited);

            // 当前结点已处理完毕
            openList.Remove(current);
            closeList.Add(current);

            if (current == endNode)
                return;

            // 寻找当前结点的邻居结点（8个方向）
            foreach (var neighbor in Map.Instance.Neighbor(current, Map.Instance.directions8))
            {
                // 该结点已经处理完毕，跳过
                if (closeList.Contains(neighbor))
                    continue;

                float costToNeighbor = current.G + Weight(current.index, neighbor.index);

                // 新的代价更低或者这个结点还未访问过
                if (!openList.Contains(neighbor) || costToNeighbor < neighbor.G)
                {
                    // 更新邻居的G代价
                    neighbor.G = costToNeighbor;
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
        Dijkstra(startNode, endNode);
    }
}