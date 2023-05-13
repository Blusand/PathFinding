using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Compare<T> where T : IComparable<T>
{
    /// <summary>
    ///  按照递增顺序比较元素（可用于小根堆）
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static int Greater(T value1, T value2)
    {
        return value1.CompareTo(value2);
    }

    /// <summary>
    /// 按照递减顺序比较元素（可用于大根堆）
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static int Less(T value1, T value2)
    {
        return value2.CompareTo(value1);
    }
}

interface IPriorityQueue<T> : IEnumerable<T>
    where T : IComparable<T>
{
    int Count { get; } // 堆的长度
    void Push(T value); // 新元素放入堆中
    T Pop(); // 弹出堆顶元素
    T Top(); // 获取堆顶元素
    bool IsEmpty(); // 判断堆是否为空
    List<T> ToList(); // 转换为列表
    T[] ToArray(); // 转换为数组
}

public class PriorityQueue<T> : IPriorityQueue<T>
    where T : IComparable<T>
{
    private const int DefaultCapacity = 4;

    // 比较元素的方法，默认为大根堆
    Func<T, T, int> Compare = Compare<T>.Less;

    // 底层数据结构
    private List<T> _list;

    public int Count
    {
        get => _list.Count;
    }

    public int Capacity
    {
        get => _list.Capacity;
    }

    public PriorityQueue() : this(DefaultCapacity, null)
    {
    }

    public PriorityQueue(Func<T, T, int> cmp) : this(DefaultCapacity, cmp)
    {
    }

    public PriorityQueue(int capacity) : this(capacity, null)
    {
    }

    public PriorityQueue(int capacity, Func<T, T, int> cmp)
    {
        if (capacity < 0)
        {
            Console.WriteLine("容量不能小于0");
            capacity = DefaultCapacity;
        }

        _list = new List<T>(capacity);
        // 默认为大根堆
        Compare = cmp ?? Compare;
    }

    public PriorityQueue(T[] newArray, Func<T, T, int> cmp = null) : this(newArray.ToList(), cmp)
    {
    }

    public PriorityQueue(List<T> newList, Func<T, T, int> cmp = null)
    {
        if (newList == null)
        {
            Console.WriteLine("传入的列表为null");
            _list = new List<T>();
        }
        else
        {
            _list = newList;
        }

        // 默认为大根堆
        Compare = cmp ?? Compare;

        // 建立初始堆
        BuildHeap();
    }

    /// <summary>
    /// 将元素放入堆中
    /// </summary>
    /// <param name="value"></param>
    public void Push(T value)
    {
        // 新元素插入到堆底
        _list.Add(value);

        // 向上调整堆
        int i = Count - 1, root;
        while (i > 0)
        {
            // 计算父节点的下标
            root = i % 2 == 0 ? (i - 1) / 2 : i / 2;
            if (i / 2 >= 0 && Compare(_list[root], _list[i]) > 0)
            {
                Swap(_list, i, root);
                i = root;
            }
            // 调整完毕
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 弹出堆顶元素
    /// </summary>
    public T Pop()
    {
        if (IsEmpty())
        {
            Console.WriteLine("无法出队，堆为空");
            return default(T);
        }

        T top = _list[0];
        Swap(_list, 0, Count - 1);
        _list.RemoveAt(Count - 1);
        HeapAdjest(_list, 0, Count);
        return top;
    }

    /// <summary>
    /// 返回堆顶元素
    /// </summary>
    /// <returns></returns>
    public T Top()
    {
        if (IsEmpty())
        {
            Console.WriteLine("无法出队，堆为空");
            return default(T);
        }

        return _list[0];
    }

    /// <summary>
    /// 队列是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return Count == 0;
    }

    /// <summary>
    /// 将堆转换为列表
    /// 如果是大根堆，则输出一个降序序列
    /// 如果是小根堆，则输出一个升序序列
    /// </summary>
    /// <returns></returns>
    public List<T> ToList()
    {
        int length = Count;
        List<T> sortList = new List<T>(_list);
        for (int i = length - 1; i >= 0; --i)
        {
            Swap(sortList, 0, i);
            --length;
            HeapAdjest(sortList, 0, length);
        }

        return sortList;
    }

    /// <summary>
    /// 将堆转换为数组
    /// 如果是大根堆，则输出一个降序序列
    /// 如果是小根堆，则输出一个升序序列
    /// </summary>
    /// <returns></returns>
    public T[] ToArray()
    {
        List<T> sortList = ToList();
        return sortList.ToArray();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// 建堆
    /// </summary>
    private void BuildHeap()
    {
        for (int i = Count / 2; i >= 0; --i)
        {
            HeapAdjest(_list, i, Count);
        }
    }

    /// <summary>
    /// 调整以root为根节点的子树
    /// </summary>
    /// <param name="root"></param>
    private void HeapAdjest(List<T> heap, int root, int length)
    {
        int left = root * 2 + 1, right = root * 2 + 2, top = root;
        // 左孩子节点更大
        if (left < length && Compare(heap[top], heap[left]) > 0)
        {
            top = left;
        }

        // 右孩子节点更大
        if (right < length && Compare(heap[top], heap[right]) > 0)
        {
            top = right;
        }

        // 当前根节点比孩子节点小
        if (top != root)
        {
            Swap(heap, root, top);
            HeapAdjest(heap, top, length);
        }
    }

    /// <summary>
    /// 交换list中的两个元素
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private void Swap(List<T> list, int left, int right)
    {
        T temp = list[left];
        list[left] = list[right];
        list[right] = temp;
    }
}