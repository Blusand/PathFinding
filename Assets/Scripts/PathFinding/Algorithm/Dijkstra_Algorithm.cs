using System.Collections.Generic;
using UnityEngine;

public class Dijkstra_Algorithm : ISelectAlgorithm
{
    // 开启列表
    PriorityQueue<Node> openList = new PriorityQueue<Node>(Compare<Node>.Greater);
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // 结点之间的权值，这里为两个格子之间的距离*10
    int Weight(Vector2 index1, Vector2 index2)
    {
        return (int)((index1 - index2).magnitude * 10);
    }

    public void Dijkstra(Node startNode, Node endNode)
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
                if (neighbor.G == 0 || costToNeighbor < neighbor.G)
                {
                    // 更新邻居的G代价
                    neighbor.G = costToNeighbor;
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
        Dijkstra(startNode, endNode);
    }
}