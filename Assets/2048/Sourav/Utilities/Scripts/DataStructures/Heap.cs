using System;

namespace Sourav.Utilities.Scripts.DataStructures
{
    public class Heap<T> where T : IHeapItem<T>
    {
        T[] items;
        int currentItemCount;

        #region Public Methods
        public int Count
        {
            get { return currentItemCount; }
        }

        public Heap(int MaxHeapSize)
        {
            items = new T[MaxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public T RemoveFirstItem()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        
        }
        #endregion

        #region Private Methods
        private void SortDown(T item)
        {
            while(true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;
                if(childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;
                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }
                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                        return;
                }
                else
                {
                    return;
                }
            
            }
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while(true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                    break;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            itemA.HeapIndex += itemB.HeapIndex;
            itemB.HeapIndex = itemA.HeapIndex - itemB.HeapIndex;
            itemA.HeapIndex = itemA.HeapIndex - itemB.HeapIndex;
        }
        #endregion
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}

