using System;

/// <summary>
/// Simple stack collection with a first in first out system
/// </summary>
/// <typeparam name="T"></typeparam>
public class CStack<T> {

    private T[] array;
    private int i;

    private static int defaultArraySize = 8;

    public CStack() {
        Reset();
    }

    /// <summary>
    /// Resets the stack
    /// </summary>
    public void Reset() {
        array = new T[defaultArraySize];
        i = 0;
    }

    /// <summary>
    /// Add new element to the stack
    /// </summary>
    /// <param name="obj"></param>
    public void Push(T obj) {

        if (i >= array.Length) {
            IncreaseArray();
        }

        array[i] = obj;
        i++;
    }

    /// <summary>
    /// Retrieve and remove the first element from the stack
    /// </summary>
    /// <returns></returns>
    public T Pop() {

        if (i > 0) {
            i--;

            T obj = array[i];
            array[i] = default(T);

            return obj;
        }

        throw new Exception("Stack is empty");
    }

    /// <summary>
    /// Retrieve but do not remove the first element from the stack
    /// </summary>
    /// <returns></returns>
    public T Peek() {

        if (i > 0) {
            return array[i - 1];
        }

        throw new Exception("Stack is empty");
    }

    /// <summary>
    /// Returns the amount of elements in the stack
    /// </summary>
    public int Count {
        get {
            return i;
        }
    }

    /// <summary>
    /// Returns the size of the underlying array of the stack
    /// </summary>
    public int Capacity {
        get {
            return array.Length;
        }
    }

    /// <summary>
    /// Increases the size of the array
    /// </summary>
    private void IncreaseArray() {
        Array.Resize(ref array, array.Length * 2);
    }

    /// <summary>
    /// Decreases the size of the underlying array to fit the amount of elements in the array
    /// </summary>
    public void FitToSize() {
        if (array.Length <= defaultArraySize) {
            return;
        }

        int len = array.Length;
        int pow;

        for (pow = 0; ; pow++) {

            len /= 2;

            if (len < i || len < defaultArraySize) {
                break;
            }
        }

        if (pow == 0) {
            return;
        }

        Array.Resize(ref array, array.Length / (1 << pow));
    }
}