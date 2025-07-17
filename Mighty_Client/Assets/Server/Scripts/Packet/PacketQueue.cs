using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketMessage
{
	public ushort Id { get; set; }
	public IMessage Message { get; set; }
}

public class PacketQueue
{
	public static PacketQueue Instance { get; } = new PacketQueue();

    private Queue<PacketMessage> _packetQueue = new Queue<PacketMessage>();
    private object _lock = new object();

    private List<PacketMessage> list = new List<PacketMessage>();

    public void Push(ushort id, IMessage packet)
	{
		lock (_lock)
		{
			_packetQueue.Enqueue(new PacketMessage() { Id = id, Message = packet });
		}
	}

	public PacketMessage Pop()
	{
		lock (_lock)
		{
			if (_packetQueue.Count == 0)
				return null;

			return _packetQueue.Dequeue();
		}
	}

	public List<PacketMessage> PopAll()
	{
        list.Clear();

        lock (_lock)
		{
			while (_packetQueue.Count > 0)
				list.Add(_packetQueue.Dequeue());
		}

		return list;
	}
}