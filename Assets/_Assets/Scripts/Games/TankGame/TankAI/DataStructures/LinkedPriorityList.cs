using System;
/// <summary>
/// Linked priority list. The type <typeparamref name="T"/> with the lowest priority will always be the first one, and the one with the greatest value will be the last one. Functionality is identical to a priority queue.
/// </summary>
/// <typeparam name="T"></typeparam>
[CoverInReport]
public class LinkedPriorityList<T> {

    #region fields
    public int Count { get; private set; }

    private class Node<Tn> {

        public Node(Tn value, double priority, Node<Tn> next) {
            this.Value = value;
            this.Priority = priority;
            this.Next = next;
        }

        private Tn value;
        private double priority;
        private Node<Tn> next;

        public Tn Value { get => value; set => this.value = value; }
        public double Priority { get => priority; set => priority = value; }
        public Node<Tn> Next { get => next; set => next = value; }
    }

    private Node<T> first;
    #endregion

    public LinkedPriorityList() {
        Count = 0;
    }

    #region Adding
    /// <summary>
    /// Adds a new value of type <typeparamref name="T"/> to the queue based on it's priority
    /// </summary>
    /// <param name="nodeVal">Value of type <typeparamref name="T"/> to add</param>
    /// <param name="priority">Priority of the value</param>
    public void Add(T nodeVal, double priority) {

        if (Count == 0) {
            AddFirst(nodeVal, priority);
        } else if (priority < first.Priority) {
            ReplaceFirst(nodeVal, priority);
        } else {
            AddNew(nodeVal, priority);
        }

        Count++;
    }

    private void AddFirst(T nodeVal, double priority) {
        first = new Node<T>(nodeVal, priority, null);
    }
    private void ReplaceFirst(T nodeVal, double priority) {

        Node<T> next = first;
        AddFirst(nodeVal, priority);

        first.Next = next;
    }
    private void AddAfter(Node<T> node, Node<T> newNode) {
        newNode.Next = node.Next;
        node.Next = newNode;
    }

    private void AddNew(T nodeVal, double priority) {

        Node<T> node = first;
        Node<T> newNode = new Node<T>(nodeVal, priority, null);

        while (node.Next != null) {

            if (priority < node.Next.Priority) {
                AddAfter(node, newNode);
                return;
            }
            node = node.Next;
        }

        node.Next = newNode;
    }

    /// <summary>
    /// Updates the priority of an element
    /// </summary>
    /// <param name="nodeVal"></param>
    /// <param name="newPriority"></param>
    public void Update(T nodeVal, double newPriority) {
        Remove(nodeVal);
        Add(nodeVal, newPriority);
    }
    #endregion

    #region Getting
    /// <summary>
    /// Returns the first value
    /// </summary>
    public T First {
        get {
            return first.Value;
        }
    }

    /// <summary>
    /// Returns and removes the first value
    /// </summary>
    /// <returns></returns>
    public T Remove() {

        if (Count == 0) {
            throw new System.Exception("Queue is empty.");
        }

        T value = first.Value;
        first = first.Next;
        Count--;

        return value;
    }

    /// <summary>
    /// Returns the i:th element from the list
    /// </summary>
    /// <param name="index"></param>
    public T Get(int i) {

        if (i >= Count) {
            throw new System.IndexOutOfRangeException();
        }

        Node<T> node = first;

        for (int index = 0; index < i; index++) {
            node = node.Next;
        }

        return node.Value;
    }

    /// <summary>
    /// Removes the element from the list
    /// </summary>
    /// <param name="nodeVal"></param>
    public bool Remove(T nodeVal) {

        Node<T> node = first;

        if (node.Value.Equals(nodeVal)) {

            first = node.Next;
            node = null;

            Count--;
            return true;
        }

        for (int i = 0; i < Count - 1; i++) {

            if (nodeVal.Equals(node.Next.Value)) {
                node.Next = node.Next.Next;
                Count--;
                return true;
            }
            
            node = node.Next;
        }

        return false;
    }

    /// <summary>
    /// Returns an equal object from the list.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Find(T value, out T param) {

        Node<T> node = first;

        for (int i = 0; i < Count; i++) {
            if (node.Value.Equals(value)) {
                param = node.Value;
                return true;
            }
            node = node.Next;
        }

        param = default(T);
        return false;
    }
    #endregion

    /// <summary>
    /// Clears the list
    /// </summary>
    public void Clear() {
        first = null;
        Count = 0;
    }

    public bool Contains(T value) {

        Node<T> node = first;

        for (int i = 0; i < Count; i++) {
            if (node.Value.Equals(value)) {
                return true;
            }
            node = node.Next;
        }

        return false;
    }

    /// <summary>
    /// Returns an array with all the type <typeparamref name="T"/> values in the correct order.
    /// </summary>
    /// <returns>Ordered array of type <typeparamref name="T"/></returns>
    public T[] ToArray() {

        T[] array = new T[Count];

        Node<T> node = first;

        for (int i = 0; i < Count; i++) {
            array[i] = node.Value;
            node = node.Next;
        }

        return array;
    }
}
