using System.Collections.Generic;
using UnityEngine;

public class Astar_Algorithm : ISelectAlgorithm
{
    // 开启列表
    PriorityQueue<Node> openList = new PriorityQueue<Node>(Compare<Node>.Greater);
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // H代价
    float heuristic(Vector2 index1, Vector2 index2)
    {
        // 欧几里德距离
        // return (int)(new Vector2(index2.x - index1.x, index2.y - index1.y)).magnitude * 10;

        // 曼哈顿距离
        // return (Mathf.Abs(index1.x - index2.x)
        //     + Mathf.Abs(index1.y - index2.y)) * 10;

        // 对角距离
        float dx = Mathf.Abs(index1.x - index2.x);
        float dy = Mathf.Abs(index1.y - index2.y);
        return (dx + dy) * 10 - 6 * Mathf.Min(dx, dy);
    }

    // 两个结点之间的距离，即G代价
    float GetDistance(Vector2 index1, Vector2 index2)
    {
        return (int)((index2 - index1).magnitude * 10);
    }

    // A*算法
    void Astar(Node startNode, Node endNode)
    {
        openList.Push(startNode);

        while (openList.Count != 0)
        {
            // 取出代价最小的那个结点
            var current = openList.Top();

            /* 可视化操作 */
            Map.Instance.AddVisualStep(current, NodeType.HasVisited);

            // 当前结点已经处理完毕
            closeList.Add(current);
            openList.Pop();

            // 检查当前结点是否是目标结点
            if (current == endNode)
                return;

            // 找到当前结点所能到达的邻接结点
            foreach (var neighbor in Map.Instance.Neighbor(current, Map.Instance.directions8))
            {
                // 该节点已经处理完毕，则跳过
                if (closeList.Contains(neighbor))
                    continue;

                // 当前点的步数 + 到达邻接结点所需的步数
                float costToNeighbor = current.G + GetDistance(current.index, neighbor.index);

                // 该结点未处理过 || 新结点的代价比已设置的代价更有利
                if (neighbor.G == 0 || costToNeighbor < neighbor.G)
                {
                    // 更新邻接结点的G代价
                    neighbor.G = costToNeighbor;
                    // 将这个邻居连接到当前节点，在算法结束时可以重新追踪路径
                    neighbor.father = current;

                    // 检查该节点的H代价是否计算过
                    if (neighbor.H == 0)
                    {
                        /* 可视化操作 */
                        Map.Instance.AddVisualStep(neighbor, NodeType.OutSide);

                        // 如果未处理过，则设置它的H值（H值只设置一次就可以了，因为它不会改变）
                        neighbor.H = heuristic(neighbor.index, endNode.index);
                        openList.Push(neighbor);
                    }
                }
            }
        }
    }

    // 运行算法
    public void StartAlgorithm(Node startNode, Node endNode)
    {
        Astar(startNode, endNode);
    }
}