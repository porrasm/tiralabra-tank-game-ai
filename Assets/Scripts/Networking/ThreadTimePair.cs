using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class ThreadTime {

    private static int TIMEOUT = Server.TIMEOUT + 1000;

    public Thread thread;
    public int id;
    public long time;

    public static ThreadTime New(ThreadStart start) {

        ThreadTime t = new ThreadTime();
        t.thread = new Thread(start);
        t.thread.IsBackground = true;

        return t;
    }

    public void Start() {
        time = TimeNow();
        thread.Start();
    }
    public bool Done() {

        if (thread == null) {
            return true;
        }

        if (thread.IsAlive) {
            if (TimeNow() - time >= TIMEOUT) {
                thread.Abort();
                return true;
            }
            return false;
        }

        return true;
    }
    public void Kill() {
        thread.Abort();
        thread = null;
    }

    private long TimeNow() {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}
