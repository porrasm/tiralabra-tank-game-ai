using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

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