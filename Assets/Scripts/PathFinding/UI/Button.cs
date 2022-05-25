using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public InputField startNodeX;
    public InputField startNodeY;
    public InputField endNodeX;
    public InputField endNodeY;

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
        startNodeIndex = new Vector2(int.Parse(startNodeX.text), int.Parse(startNodeY.text));
        endNodeIndex = new Vector2(int.Parse(endNodeX.text), int.Parse(endNodeY.text));
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
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    // 退出游戏
    public void Exit()
    {
        Application.Quit();
    }
}