protoc.exe -I=./ --csharp_out=./ ./UnityProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./GameProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./LobbyMsgProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./GameMsgProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../Server/PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet"
XCOPY /Y Protocol.cs "../../../Server/Server/Packet"
XCOPY /Y LobbyMsgProtocol.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet"
XCOPY /Y LobbyMsgProtocol.cs "../../../Server/Server/Packet"
XCOPY /Y GameMsgProtocol.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet"
XCOPY /Y GameMsgProtocol.cs "../../../Server/Server/Packet"
XCOPY /Y UnityProtocol.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet/Unity"
XCOPY /Y UnityProtocol.cs "../../../Server/Server/Packet/Unity"
XCOPY /Y GameProtocol.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet/Unity"
XCOPY /Y GameProtocol.cs "../../../Server/Server/Packet/Unity"
XCOPY /Y ClientPacketManager.cs "../../../3DRPGGame/Assets/3DRPGGame/Scripts/Server/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Server/Packet"