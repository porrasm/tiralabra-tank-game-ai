using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAStarPath {

    #region fields
    private byte[,] level;
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
            G = cost;
        }

        public Node prev;
        public IntCoords Coords { get; private set; }
        public double G { get; set; }


        public double F(IntCoords end) {
            return G + Coords.Distance(end);
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

    public TankAStarPath(byte[,] level) {

        this.level = level;

        cost = 1;
        costDiag = Maths.Sqrt(2);

    }

    public IntCoords[] FindPath(IntCoords start, IntCoords end) {

        if (end.x >= level.Length || end.y >= level.GetLength(1)) {
            return new IntCoords[]{start};
        }

        this.start = start;
        this.end = end;

        open = new LinkedPriorityList<Node>();
        // Replace
        closed = new HashSet<IntCoords>();

        Node n = new Node(start, 0);
        open.Add(n, n.F(end));

        while (open.Count > 0) {

            n = open.Remove();

            if (n.Coords == end) {
                return NodeToPath(n);
            }

            closed.Add(n.Coords);

            byte allowed = level[n.Coords.x, n.Coords.y];
            for (int i = 0; i < 8; i++) {

                Node neighbour = GetChild(n, allowed, i);

                if (neighbour == null) {
                    continue;
                }

                double tentative_gScore = n.G + DirCost(i);

                if (tentative_gScore < neighbour.G) {
                    neighbour.prev = n;
                    neighbour.G = tentative_gScore;
                }
            }
        }

        return NodeToPath(n);
    }

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
        open.Add(newNode, newNode.F(end));

        return newNode;
    }
    private double DirCost(int i) {
        if (i > 3) {
            return costDiag;
        }
        return cost;
    }

    // Improve
    private IntCoords[] NodeToPath(Node node) {

        int count = 0;

        Node n = node;

        while (n.prev != null) {
            count++;
            n = n.prev;
        }

        IntCoords[] route = new IntCoords[count+1];

        for (int i = count; i > 0; i--) {
            route[i] = node.Coords;
            node = node.prev;
        }
        route[0] = start;

        return route;
    }
}
