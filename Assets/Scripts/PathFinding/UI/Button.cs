using TMPro;
using UnityEngine;

public class Button : MonoBehaviour
{
    public TMP_InputField startNodeX;
    public TMP_InputField startNodeY;
    public TMP_InputField endNodeX;
    public TMP_InputField endNodeY;

    Vector2 startNodeIndex;
    Vector2 endNodeIndex;

    bool pause = false;

    void Start()
    {
        // 起点终点初始值
        startNodeX.text = "0";
        startNodeY.text = "0";
        endNodeX.text = "29";
        endNodeY.text = "29";
    }

    // 设置起点和终点的坐标
    void SetVector()
    {
        if (pause)
        {
            pause = false;
            Time.timeScale = 1;
        }

        int startX = int.Parse(startNodeX.text);
        if (startX < 0)
        {
            startX = 0;
        }

        int startY = int.Parse(startNodeY.text);
        if (startY < 0)
        {
            startY = 0;
        }

        startNodeIndex = new Vector2(startX, startY);

        int endX = int.Parse(endNodeX.text);
        if (endX >= Map.mapWidth)
        {
            endX = Map.mapWidth - 1;
        }

        int endY = int.Parse(endNodeY.text);
        if (endY >= Map.mapHeight)
        {
            endY = Map.mapHeight - 1;
        }

        endNodeIndex = new Vector2(endX, endY);
    }

    // BFS
    public void BFS_Button_Click()
    {
        SetVector();
        Map.Instance.SetNode(startNodeIndex, endNodeIndex);
        Map.Instance.BFS();
    }

    // DFS
    public void DFS_Button_Click()
    {
        SetVector();
        Map.Instance.SetNode(startNodeIndex, endNodeIndex);
        Map.Instance.DFS();
    }

    // BestFirstSearch
    public void BestFirstSearch_Button_Click()
    {
        SetVector();
        Map.Instance.SetNode(startNodeIndex, endNodeIndex);
        Map.Instance.BestFirstSearch();
    }

    // Dijkstra
    public void Dijkstra_Button_Click()
    {
        SetVector();
        Map.Instance.SetNode(startNodeIndex, endNodeIndex);
        Map.Instance.Dijkstra();
    }

    // Astar
    public void Astar_Button_Click()
    {
        SetVector();
        Map.Instance.SetNode(startNodeIndex, endNodeIndex);
        Map.Instance.Astar();
    }

    // 暂停游戏
    public void Pause()
    {
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // 退出游戏
    public void Exit()
    {
        Application.Quit();
    }
}