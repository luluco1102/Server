﻿using System.IO;

namespace Core.Net
{
    public enum PacketId : ushort
    {
        S_ConnectionSuccess = 0x0010,
        S_ConnectionFailure = 0x0011,
        S_ChatSend = 0x1010,
        C_ChatSend = 0x1020,
    }

    public interface IPacket
    {
        PacketId PacketId { get; }
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }

    /// <summary>
    /// Server -> Client message
    /// </summary>
    public sealed class S_ConnectionSuccess : IPacket
    {
        public PacketId PacketId => PacketId.S_ConnectionSuccess;

        public int AssignedClientId { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AssignedClientId);
        }

        public void Deserialize(BinaryReader reader)
        {
            AssignedClientId = reader.ReadInt32();
        }
    }

    /// <summary>
    /// Server -> Client message
    /// </summary>
    public sealed class S_ConnectionFailure : IPacket
    {
        public PacketId PacketId => PacketId.S_ConnectionFailure;

        public string Reason { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Reason);
        }

        public void Deserialize(BinaryReader reader)
        {
            Reason = reader.ReadString();
        }
    }

    /// <summary>
    /// Server -> Client message
    /// </summary>
    public sealed class S_ChatSend : IPacket
    {
        public PacketId PacketId => PacketId.S_ChatSend;

        public int SenderId { get; set; }
        public string Text { get; set; } = string.Empty;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SenderId);
            writer.Write(Text);
        }

        public void Deserialize(BinaryReader reader)
        {
            SenderId = reader.ReadInt32();
            Text = reader.ReadString();
        }
    }

    /// <summary>
    /// Client -> Server message
    /// </summary>
    public sealed class C_ChatSend : IPacket
    {
        public PacketId PacketId => PacketId.C_ChatSend;

        public string Text { get; set; } = string.Empty;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Text);
        }

        public void Deserialize(BinaryReader reader)
        {
            Text = reader.ReadString();
        }
    }
}