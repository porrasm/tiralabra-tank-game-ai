using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAStarPath : TankAIPathfinding {

    #region fields
    private Node[,] nodes;
    private double cost, costDiag;

    private IntCoords start;
    private IntCoords end;

    LinkedPriorityList<Node> open;
    // Replace
    HashSet<IntCoords> closed;

    private class Node {

        public Node(IntCoords coords, double cost) {
            this.Coords = coords;
            this.cost = cost;
        }

        public Node prev;
        public IntCoords Coords { get; private set; }
        public double cost { get; set; }


        public double EstimatedCost(IntCoords end) {
            return cost + Coords.Distance(end);
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == GetType()) {
                return Coords == ((Node)obj).Coords;
            }
            return false;
        }
        public override int GetHashCode() {
            return Coords.GetHashCode();
        }
    }
    #endregion

    public TankAStarPath(byte[,] level) :base(level) {
        cost = 1;
        costDiag = Maths.Sqrt(2);
    }

    /// <summary>
    /// Finds a path from start towards the end with an independent route found condition. The FoundCondition(IntCoords current) function is called on every cell and if it returns true the current route is returned.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="foundCondition"></param>
    /// <returns>Path as Vector array</returns>
    public override Vector[] FindPath(IntCoords start, IntCoords end, FoundCondition foundCondition) {

        this.start = start;
        this.end = end;

        open = new LinkedPriorityList<Node>();
        // Replace
        closed = new HashSet<IntCoords>();

        Node n = new Node(start, 0);
        open.Add(n, n.EstimatedCost(end));

        while (open.Count > 0) {

            n = open.Remove();

            if (foundCondition(n.Coords)) {
                return NodeToPath(n);
            }

            closed.Add(n.Coords);

            byte allowed = level[n.Coords.x, n.Coords.y];
            for (int i = 0; i < 8; i++) {

                Node neighbour = GetChild(n, allowed, i);

                if (neighbour == null) {
                    continue;
                }

                double tentative_gScore = n.cost + DirCost(i);

                if (tentative_gScore < neighbour.cost) {
                    neighbour.prev = n;
                    neighbour.cost = tentative_gScore;
                }
            }
        }

        return NodeToPath(n);
    }

    /// <summary>
    /// Returns a linked node with index i. If the child is not found the function returns null.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="allowed"></param>
    /// <param name="i"></param>
    /// <returns>Node or null</returns>
    private Node GetChild(Node n, byte allowed, int i) {

        TankDirection d = (TankDirection)i;

        if (!TankDirectionTools.AllowedDirection(allowed, d)) {
            return null;
        }

        double cost = DirCost(i);

        IntCoords newCoords = n.Coords.MoveToDirection(d);

        if (closed.Contains(newCoords)) {
            return null;
        }

        Node newNode = open.Find(new Node(newCoords, 0));

        if (newNode != null) {
            return newNode;
        }

        newNode = new Node(newCoords, double.PositiveInfinity);
        newNode.prev = n;
        open.Add(newNode, newNode.EstimatedCost(end));

        return newNode;
    }

    /// <summary>
    /// Cost of a direction.
    /// </summary>
    /// <param name="i"></param>
    /// <returns>1 for straight, Sqrt(2) for diagonal</returns>
    private double DirCost(int i) {
        if (i > 3) {
            return costDiag;
        }
        return cost;
    }

    // Improve

    /// <summary>
    /// Returns the path from the start leading to the given node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private Vector[] NodeToPath(Node node) {

        int count = 0;

        Node n = node;

        while (n.prev != null) {
            count++;
            n = n.prev;
        }

        Vector[] route = new Vector[count+1];

        for (int i = count; i > 0; i--) {
            route[i] = Vector.CoordsToPosition(node.Coords);
            node = node.prev;
        }
        route[0] = Vector.CoordsToPosition(start);

        return route;
    }
}
