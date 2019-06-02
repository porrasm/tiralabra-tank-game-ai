using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadTimeout {

    private static int id;
    public static int ID() {
        return id++;
    }

    public List<ThreadTime> Threads;
    public ThreadTimeout() {
        Threads = new List<ThreadTime>();
    }
    public void FlushThreads() {

        if (Threads == null) {
            return;
        }

        for (int i = 0; i < Threads.Count; i++) {
            if (Threads[i].Done()) {
                Threads.RemoveAt(i);
                i--;
            }
        }
    }
    public void Kill() {
        foreach (ThreadTime t in Threads) {
            t.thread.Abort();
            Threads[0] = null;
            Threads.RemoveAt(0);
        }
    }
    public void KillID(int id) {
        for (int i = 0; i < Threads.Count; i++) {
            if (Threads[i].id == id) {
                Threads[i].Kill();
                Threads[i] = null;
                Threads.RemoveAt(i);
                break;
            }
        }
    }
}
