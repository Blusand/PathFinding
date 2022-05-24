using System;
using System.Collections.Generic;

// 接口
interface IPriorityQueue<T> where T : IComparable<T>
{
    int Size { get; }   // 堆的长度
    void Push(T value); // 新元素放入堆中
    void Pop(); // 弹出堆顶元素
    T Top();    // 获取堆顶元素
    bool Empty();   // 判断堆是否为空
    void BuildHeap();   // 建堆
    void HeapAdjest(int root, int length);  // 调整堆
    List<T> Sort();    // 堆排序
}

// 优先级队列
public class PriorityQueue<T> where T : IComparable<T>
{
    // 比较元素的方法，用于实现大根堆/小根堆
    Func<T, T, bool> Compare 
        = (T value1, T value2) => value1.CompareTo(value2) > 0;

    // 底层数据结构
    private List<T> list = new List<T>();

    public int Size { get => list.Count - 1; }

    public PriorityQueue(List<T> newList, Func<T, T, bool> cmp = null)
    {
        // 下标0不存放元素
        list.Add(default);
        foreach (T num in newList)
        {
            list.Add(num);
        }

        // 默认为大根堆
        Compare = cmp ?? Compare;

        // 建立初始堆
        BuildHeap();
    }

    // 建堆
    private void BuildHeap()
    {
        for (int i = Size / 2; i > 0; --i)
        {
            HeapAdjest(list, i, Size);
        }
    }

    public bool Empty()
    {
        return Size == 0;
    }

    // 调整以root为根结点的子树
    private void HeapAdjest(List<T> currentList, int root, int length)
    {
        if (root >= length) return;

        // currentList[0]暂存子树的根结点
        currentList[0] = currentList[root];

        for (int i = 2 * root; i <= length; i *= 2)
        {
            // 找出最大/小的孩子
            if (i < length && Compare(currentList[i + 1], currentList[i]))
                ++i;

            // 根结点大于/小于两个孩子结点
            if (Compare(currentList[0], currentList[i]))
                break;
            // 继续往下寻找
            else
            {
                currentList[root] = currentList[i];
                root = i;
            }
        }

        // 被筛选结点放入最终位置
        currentList[root] = currentList[0];
    }

    public List<T> Sort()
    {
        List<T> newList = new List<T>();
        foreach (T num in list)
        {
            newList.Add(num);
        }

        for (int i = Size; i > 1; --i)
        {
            Swap(newList, 1, i);
            // 把剩余的元素调整为堆
            HeapAdjest(newList, 1, i - 1);
        }

        // 下标0处不存放元素
        newList.RemoveAt(0);

        return newList;
    }

    // 交换list中的两个元素
    private void Swap(List<T> currentList, int index1, int index2)
    {
        T temp = currentList[index1];
        currentList[index1] = currentList[index2];
        currentList[index2] = temp;
    }

    public void Push(T value)
    {
        // 新元素插入到堆底
        list.Add(value);
        // 保存尾端元素
        list[0] = value;

        // 向上调整堆
        int i = Size;
        for (; i > 0; i /= 2)
        {
            if (i / 2 > 0 && Compare(list[i], list[i / 2]))
                list[i] = list[i / 2];
            // 调整完毕
            else
                break;
        }

        list[i] = list[0];
    }

    public void Pop()
    {
        if (Empty())
        {
            System.Console.Write("无法出队，堆为空");
            return;
        }

        list[1] = list[Size];
        list.RemoveAt(Size);
        HeapAdjest(list, 1, Size);
    }

    // 返回堆顶元素
    public T Top()
    {
        return list[1];
    }
}