public struct ValuePair<TLeft, TRight> {

    public ValuePair(TLeft left, TRight right) {
        Left = left;
        Right = right;
    }

    public TLeft Left;
    public TRight Right;
}