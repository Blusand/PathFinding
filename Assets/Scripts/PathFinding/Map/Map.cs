using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理器
/// </summary>
public class Map : MonoBehaviour
{
    // 单例模式
    private static Map instance;

    public static Map Instance
    {
        get { return instance; }
    }

    // 地图宽高
    [Header("地图信息")]
    static public int mapWidth = 30, mapHeight = 30;

    [SerializeField, Header("结点信息")]
    Node nodePerfab;

    [SerializeField, Min(0)]
    float nodeWidth, nodeHeight;

    // 二维地图
    Node[,] nodes;

    // 开始和结束结点
    // 玩家设置坐标
    [Min(0)]
    public Vector2 startNodeIndex, endNodeIndex;

    // 内部获取结点
    Node startNode, endNode;

    // 可视化步骤
    List<Pair<Node, NodeType>> visualStep = new List<Pair<Node, NodeType>>();

    [Range(0.001f, 0.01f), SerializeField, Header("可视化结点信息")]
    float visualSpeed = 0.01f;

    [SerializeField]
    Color startNodeColor = Color.green,
        endNodeColor = Color.red,
        wallColor = Color.black,
        hasVisitedColor = Color.cyan,
        outsideColor = Color.yellow,
        pathColor = Color.blue;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitMap();
    }

    void Update()
    {
        // 调用寻路函数
        // PlayerInput();
    }

    #region CallAlgorithm

    // 玩家按键输入
    void PlayerInput()
    {
        // BFS算法
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BFS();
        }
        // DFS算法
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DFS();
        }
        // 最佳优先搜索算法
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BestFirstSearch();
        }
        // Dijkstra算法
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Dijkstra();
        }
        // A*算法
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Astar();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteMap();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (nodes == null)
                InitMap();
        }
    }

    public void BFS()
    {
        ClearMap();
        StopAllCoroutines();
        ISelectAlgorithm pathFinding = new BFS_Algorithm();
        pathFinding.StartAlgorithm(startNode, endNode);
        StartCoroutine(VisualSearch(startNode, endNode));
    }

    public void DFS()
    {
        ClearMap();
        StopAllCoroutines();
        ISelectAlgorithm pathFinding = new DFS_Algorithm();
        pathFinding.StartAlgorithm(startNode, endNode);
        StartCoroutine(VisualSearch(startNode, endNode));
    }

    public void BestFirstSearch()
    {
        ClearMap();
        StopAllCoroutines();
        ISelectAlgorithm pathFinding = new BestFirstSearch_Algorithm();
        pathFinding.StartAlgorithm(startNode, endNode);
        StartCoroutine(VisualSearch(startNode, endNode));
    }

    public void Dijkstra()
    {
        ClearMap();
        StopAllCoroutines();
        ISelectAlgorithm pathFinding = new Dijkstra_Algorithm();
        pathFinding.StartAlgorithm(startNode, endNode);
        StartCoroutine(VisualSearch(startNode, endNode));
    }

    public void Astar()
    {
        ClearMap();
        StopAllCoroutines();
        ISelectAlgorithm pathFinding = new Astar_Algorithm();
        pathFinding.StartAlgorithm(startNode, endNode);
        StartCoroutine(VisualSearch(startNode, endNode));
    }

    #endregion

    #region MapControl

    // 设置起点和终点
    public void SetNode(Vector2 startNodeIndex, Vector2 endNodeIndex)
    {
        // 清除旧结点的数据
        this.startNode.type = NodeType.HasNotVisited;
        this.endNode.type = NodeType.HasNotVisited;
        this.startNode = nodes[(int)startNodeIndex.x, (int)startNodeIndex.y];
        this.endNode = nodes[(int)endNodeIndex.x, (int)endNodeIndex.y];

        // 设置新的数据
        this.startNode.index = startNodeIndex;
        this.endNode.index = endNodeIndex;
        this.startNode.type = NodeType.Start;
        this.endNode.type = NodeType.End;

        // 刷新地图
        ClearMap();
    }

    // 初始化地图
    void InitMap()
    {
        nodes = new Node[mapWidth, mapHeight];

        for (int row = 0; row < mapWidth; ++row)
        {
            for (int col = 0; col < mapHeight; ++col)
            {
                // 生成物体时记得在new Vector3中根据物体的大小进行偏移
                nodes[row, col] = Instantiate(nodePerfab,
                    new Vector3(row * nodeWidth, col * nodeHeight, 0), Quaternion.identity, transform);
                nodes[row, col].gameObject.transform.localScale = new Vector3(nodeWidth, nodeHeight, 1);
                nodes[row, col].index = new Vector2(row, col);
            }
        }

        // 设置起点和终点
        startNode = nodes[(int)startNodeIndex.x, (int)startNodeIndex.y];
        startNode.type = NodeType.Start;
        SetNodeColor(startNode, startNode.type);

        endNode = nodes[(int)endNodeIndex.x, (int)endNodeIndex.y];
        endNode.type = NodeType.End;
        SetNodeColor(endNode, endNode.type);

        // 设置障碍物
        for (int i = 10; i <= 25; ++i)
        {
            nodes[i, 25].type = NodeType.Wall;
            nodes[i, 25].gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", Color.black);
        }

        for (int j = 10; j <= 25; ++j)
        {
            nodes[25, j].type = NodeType.Wall;
            nodes[25, j].gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", Color.black);
        }
    }

    // 清除数据
    void ClearMap()
    {
        for (int row = 0; row < mapWidth; ++row)
        {
            for (int col = 0; col < mapHeight; ++col)
            {
                nodes[row, col].InitialData();
                SetNodeColor(nodes[row, col], nodes[row, col].type);
            }
        }

        visualStep.Clear();
    }

    // 删除地图
    void DeleteMap()
    {
        StopAllCoroutines();
        visualStep.Clear();

        for (int row = 0; row < mapWidth; ++row)
        {
            for (int col = 0; col < mapHeight; ++col)
            {
                Destroy(nodes[row, col].gameObject);
            }
        }

        nodes = null;
    }

    #endregion

    #region AuxiliaryFunction

    // 方向数组1（左上、上、右上、右、右下、下、左下、左）
    public List<List<int>> directions8 = new List<List<int>>
    {
        new List<int> { -1, -1 }, new List<int> { 0, 1 },
        new List<int> { 1, 1 }, new List<int> { 1, 0 },
        new List<int> { 1, -1 }, new List<int> { 0, 1 },
        new List<int> { -1, -1 }, new List<int> { -1, 0 },
    };

    // 方向数组2（上、下、左、右）
    public List<List<int>> directions4 = new List<List<int>>
    {
        new List<int> { 0, 1 }, new List<int> { 0, -1 },
        new List<int> { -1, 0 }, new List<int> { 1, 0 }
    };

    // 判断位置是否合法
    bool isValid(Vector2 index)
    {
        // 位置越界
        if (index.x < 0 || index.x >= mapWidth
                        || index.y < 0 || index.y >= mapHeight)
            return false;

        // 墙壁不可达
        if (nodes[(int)index.x, (int)index.y].type == NodeType.Wall)
            return false;

        return true;
    }

    // 寻找当前结点的邻居结点
    public List<Node> Neighbor(Node node, List<List<int>> directions)
    {
        List<Node> neighbors = new List<Node>();

        foreach (var dir in directions)
        {
            Vector2 neighborIndex = node.index + new Vector2(dir[0], dir[1]);
            // 将符合条件的结点放入列表中
            if (isValid(neighborIndex))
            {
                Node neighbor = nodes[(int)node.index.x + dir[0], (int)node.index.y + dir[1]];
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    // 找到起点到终点的路径
    IEnumerator FindPath(Node startNode, Node endNode)
    {
        SetNodeColor(endNode, NodeType.End);

        Node node = endNode.father;
        while (node != null && node.father != null)
        {
            SetNodeColor(node, NodeType.Path);
            node = node.father;
            yield return new WaitForFixedUpdate();
        }

        SetNodeColor(startNode, NodeType.Start);
    }

    // 添加可视化结点
    public void AddVisualStep(Node node, NodeType type)
    {
        if (node.index == startNodeIndex)
        {
            visualStep.Add(new Pair<Node, NodeType>(node, NodeType.Start));
        }
        else
            visualStep.Add(new Pair<Node, NodeType>(node, type));
    }

    // 设置颜色
    void SetNodeColor(Node node, NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.Start:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", startNodeColor);
                break;
            case NodeType.End:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", endNodeColor);
                break;
            case NodeType.HasVisited:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", hasVisitedColor);
                break;
            case NodeType.HasNotVisited:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", Color.white);
                break;
            case NodeType.OutSide:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", outsideColor);
                break;
            case NodeType.Wall:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", wallColor);
                break;
            case NodeType.Path:
                node.gameObject.GetComponent<Renderer>().material.SetColor("_CellColor", pathColor);
                break;
        }
    }

    // 可视化搜索过程
    IEnumerator VisualSearch(Node startNode, Node endNode)
    {
        foreach (var step in visualStep)
        {
            SetNodeColor(step.First, step.Second);
            step.First.ShowPanel(true);
            yield return new WaitForSeconds(visualSpeed);
        }

        StartCoroutine(FindPath(startNode, endNode));
    }

    #endregion
}

public class Pair<T1, T2>
{
    public T1 First { get; set; }
    public T2 Second { get; set; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}