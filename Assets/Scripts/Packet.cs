using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Packet {

    public Packet(int client_id, int control, int control_hold, string message) {
        this.client_id = client_id;
        this.control = control;
        this.control_hold = control_hold;
        this.message = message;
    }

    [NonSerialized]
    public IPAddress IPAddress;
    [NonSerialized]
    public int port;

    public int client_id;
    public int control;
    public int control_hold;
    public string message;

    public byte[] ToByteArray() {
        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream()) {
            formatter.Serialize(stream, this);
            return stream.ToArray();
        }
    }

    public override string ToString() {
        return "client: " + client_id + ", message: " + message;
    }

    public static Packet BytesToPacket(byte[] bytes) {
        
        if (bytes == null) {
            UnityEngine.MonoBehaviour.print("Bytes was null");
            return null;
        }

        if (bytes.Length == 0) {
            UnityEngine.MonoBehaviour.print("Bytes was empty");
            return null;
        }

        try {
            using (var memStream = new MemoryStream()) {
                var binForm = new BinaryFormatter();
                memStream.Write(bytes, 0, bytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Packet packet = (Packet)binForm.Deserialize(memStream);
                return packet;
            }
        } catch (Exception e) {
            return null;
        }
    }
    
    //Real methods
    public static byte[] ToByteData(List<Packet> packets) {

        byte[] packetBytes = PacketsToByteArray(packets);

        int contentLength = packetBytes.Length + 4;
        byte[] intBytes = BitConverter.GetBytes(contentLength);

        byte[] data = new byte[contentLength];

        for (int i = 0; i < 4; i++) {
            data[i] = intBytes[i];
        }
        for (int i = 0; i < packetBytes.Length; i++) {
            data[i + 4] = packetBytes[i];
        }

        return data;
    }
    private static byte[] PacketsToByteArray(List<Packet> packets) {

        //PacketContainer p = new PacketContainer();
        //p.packets = packets.ToArray();

        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream()) {
            formatter.Serialize(stream, packets);
            return stream.ToArray();
        }
    }

    public static List<Packet> ToPacketData(byte[] bytes) {

        if (bytes == null) {
            MonoBehaviour.print("Bytes was null");
            return null;
        }

        if (bytes.Length == 0) {
            MonoBehaviour.print("Bytes was empty");
            return null;
        }

        int length = bytes.Length;

        if (length == 4) {
            return new List<Packet>();
        }

        byte[] packetBytes = new byte[length - 4];

        try {
            using (var memStream = new MemoryStream()) {
                var binForm = new BinaryFormatter();
                memStream.Write(bytes, 0, bytes.Length);
                memStream.Seek(4, SeekOrigin.Begin);
                List<Packet> packets = (List<Packet>)binForm.Deserialize(memStream);
                return packets;
            }
        } catch (Exception e) {
            MonoBehaviour.print(e);
            return null;
        }
    }
}

public struct TCP_Message {

    public int control;
    public int control_hold;

    public bool message;
}

public struct TCP_Message_Large {
    public int control;
    public int control_hold;

    public string message;
}