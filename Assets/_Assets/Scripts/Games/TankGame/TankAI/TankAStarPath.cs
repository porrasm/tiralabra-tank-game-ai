using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CoverInReport]
public class TankAStarPath : TankAIPathfinding {

    #region fields
    private Node[,] nodes;
    private double cost, costDiag;

    private IntCoords start;
    private IntCoords end;

    private LinkedPriorityList<Node> open;

    // Replace
    private CoordsContainer closed;

    private class Node {

        public Node(IntCoords coords, double cost) {
            this.Coords = coords;
            this.Cost = cost;
        }

        public Node prev;
        public IntCoords Coords { get; private set; }
        public double Cost { get; set; }


        public double EstimatedCost(IntCoords end) {
            return Cost + Vector.Distance(Vector.CoordsToPosition(Coords), Vector.CoordsToPosition(end));
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == GetType()) {
                return Coords == ((Node)obj).Coords;
            }
            return false;
        }
    }
    #endregion

    public TankAStarPath(byte[,] level) : base(level) {
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

        base.FindPath(start, end, foundCondition);

        this.start = start;
        this.end = end;

        open = new LinkedPriorityList<Node>();

        // Replace
        closed = new CoordsContainer();

        Node n = new Node(start, 0);
        open.Add(n, n.EstimatedCost(end));

        while (open.Count > 0) {

            n = open.Remove();

            if (foundCondition(n.Coords)) {
                ProcessedCount = closed.Count + 1;
                return NodeToPath(n);
            }

            closed.Add(n.Coords);

            byte allowed = level[n.Coords.x, n.Coords.y];
            for (int i = 0; i < 8; i++) {

                Node neighbour = GetChild(n, allowed, i);

                if (neighbour == null) {
                    continue;
                }

                double tentative_gScore = n.Cost + DirCost(i);

                if (tentative_gScore < neighbour.Cost) {
                    neighbour.prev = n;
                    neighbour.Cost = tentative_gScore;
                }
            }
        }

        return new Vector[] { start };
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

        Node newNode;
        open.Find(new Node(newCoords, 0), out newNode);

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

        Vector[] route = new Vector[count + 1];

        for (int i = count; i > 0; i--) {
            route[i] = Vector.CoordsToPosition(node.Coords);
            node = node.prev;
        }
        route[0] = Vector.CoordsToPosition(start);

        return route;
    }
}
