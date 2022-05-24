using System.Collections.Generic;
using UnityEngine;

public class Astar_Algorithm : ISelectAlgorithm
{
    // 开启列表
    List<Node> openList = new List<Node>();
    // 关闭列表
    HashSet<Node> closeList = new HashSet<Node>();

    // H代价
    float heuristic(Vector2 index1, Vector2 index2)
    {
        return Mathf.Abs(index1.x - index2.x)
            + Mathf.Abs(index1.y - index2.y);
    }

    // 两个结点之间的距离
    float GetDistance(Vector2 index1, Vector2 index2)
    {
        return (int)((index1 - index2).magnitude * 10);
    }

    // A*算法
    void Astar(Node startNode, Node endNode)
    {
        openList.Add(startNode);

        while (openList.Count != 0)
        {
            var current = openList[0];

            // 遍历openList中的所有节点
            foreach (var t in openList)
            {
                // 新节点的F值 < 当前节点的F值 || 新节点的F值 == 当前节点的F值
                // 则判断它们哪个H值更小，取出代价最小的那个结点
                if (t.F < current.F 
                    || (t.F == current.F && t.H < current.H))
                    {
                        current = t;
                    }
            }

            /* 可视化操作 */
            Map.Instance.AddVisualStep(current, NodeType.HasVisited);

            // 当前结点已经处理完毕
            closeList.Add(current);
            openList.Remove(current);

            // 检查当前结点是否是目标结点
            if (current == endNode)
                return;

            // 找到当前结点所能到达的邻接结点
            foreach (var neighbor in Map.Instance.Neighbor(current, Map.Instance.directions8))
            {
                // 该节点已经处理完毕，则跳过
                if (closeList.Contains(neighbor))
                    continue;
                
                bool inSearch = openList.Contains(neighbor);

                // 当前点的步数 + 到达邻接结点所需的步数
                float costToNeighbor = current.G + GetDistance(current.index, neighbor.index);

                // 该结点未处理过 || 新结点的代价比已设置的代价更有利
                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    // 更新邻接结点的G代价
                    neighbor.G = costToNeighbor;
                    // 将这个邻居连接到当前节点，在算法结束时可以重新追踪路径
                    neighbor.father = current;

                    /* 可视化操作 */
                    Map.Instance.AddVisualStep(neighbor, NodeType.OutSide);

                    // 检查该节点是否在搜索列表中
                    if (!inSearch)
                    {
                        // 如果不在，则设置它的h值（h值只设置一次就可以了，因为它不会改变）
                        neighbor.H = heuristic(neighbor.index, endNode.index);
                        openList.Add(neighbor);
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