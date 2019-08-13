
/// <summary>
/// Linked priority list. The type <typeparamref name="T"/> with the lowest priority will always be the first one, and the one with the greates value will be the last one. Functionality is identical to a priority queue.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LinkedPriorityList<T> {

    #region fields
    public int Count { get; private set; }

    private class Node<T> {

        public Node(T value, double priority, Node<T> next) {
            this.value = value;
            this.priority = priority;
            this.next = next;
        }

        public T value;
        public double priority;
        public Node<T> next;
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
        } else if (priority < first.priority) {
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

        first.next = next;
    }
    private void AddAfter(Node<T> node, Node<T> newNode) {
        newNode.next = node.next;
        node.next = newNode;
    }

    private void AddNew(T nodeVal, double priority) {

        Node<T> node = first;
        Node<T> newNode = new Node<T>(nodeVal, priority, null);

        while (node.next != null) {

            if (priority < node.next.priority) {
                AddAfter(node, newNode);
                return;
            }
            node = node.next;
        }

        node.next = newNode;
    }
    #endregion

    #region Getting
    /// <summary>
    /// Returns the first value
    /// </summary>
    public T First {
        get {
            return first.value;
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

        T value = first.value;
        first = first.next;
        Count--;

        return value;
    }

    /// <summary>
    /// Returns an equal object from the list.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public T Find(T value) {

        Node<T> node = first;

        for (int i = 0; i < Count; i++) {
            if (node.value.Equals(value)) {
                return node.value;
            }
            node = node.next;
        }

        return default(T);
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
            if (node.value.Equals(value)) {
                return true;
            }
            node = node.next;
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
            array[i] = node.value;
            node = node.next;
        }

        return array;
    }
}
