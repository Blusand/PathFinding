using System.Collections.Generic;

public class BFS_Algorithm : ISelectAlgorithm
{
    void BFS(Node startNode, Node endNode)
    {
        Queue<Node> openList = new Queue<Node>();
        // 记录已经走过的结点，防止走回头路
        HashSet<Node> closeList = new HashSet<Node>();

        // 初始时开始结点进入队列
        openList.Enqueue(startNode);
        closeList.Add(startNode);

        // 走的步数
        int step = 0;
        while (openList.Count != 0)
        {   
            int length = openList.Count;
            for (int i = 0; i < length; ++i)
            {
                Node current = openList.Dequeue();
                current.G = step;
                /* 可视化操作 */
                Map.Instance.AddVisualStep(current, NodeType.HasVisited);

                // 遇到目标结点，算法结束
                if (current == endNode)
                    return;

                // 寻找邻居结点（4个方向）
                foreach (var neighbor in Map.Instance.Neighbor(current, Map.Instance.directions4))
                {
                    // 该结点已经访问过
                    if (closeList.Contains(neighbor))
                        continue;

                    // 用于寻找路径
                    neighbor.father = current;
                    
                    openList.Enqueue(neighbor);
                    closeList.Add(neighbor);

                    /* 可视化操作 */
                    Map.Instance.AddVisualStep(neighbor, NodeType.OutSide);
                }
            }

            ++step;
        }
    }

    public void StartAlgorithm(Node startNode, Node endNode)
    {
        BFS(startNode, endNode);
    }
}