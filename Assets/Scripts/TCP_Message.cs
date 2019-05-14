
using System;

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

public class UDP_Message {

    public UDP_Message(int client_id, int control, int control_hold, string message) {
        this.client_id = client_id;
        this.control = control;
        this.control_hold = control_hold;
        this.message = message;
    }

    public int client_id;
    public int control;
    public int control_hold;

    public string message;

    public byte[] ToBytes() {

        int totalSize = 0;
        totalSize += (3 * sizeof(int));
        totalSize += System.Text.ASCIIEncoding.Unicode.GetByteCount(message);

        byte[] bytes = new byte[totalSize];
        int index = 0;

        index = AppendInteger(ref bytes, index, client_id);
        index = AppendInteger(ref bytes, index, control);
        index = AppendInteger(ref bytes, index, control_hold);
    }

    private int AppendInteger(ref byte[] bytes, int index, int number) {
        int intValue;
        byte[] intBytes = BitConverter.GetBytes(number);

        for (int i = 0; i < intBytes.Length; i++) {
            bytes[index + i] = intBytes[i];
        }
    }
}