protoc.exe -I=./ --csharp_out=./ ./UnityProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./GameProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./LobbyMsgProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./GameMsgProtocol.proto 
protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../Mighty_Server/PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet/Protocol"
XCOPY /Y Protocol.cs "../../../Mighty_Server/Server/Packet"
XCOPY /Y LobbyMsgProtocol.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet/Protocol"
XCOPY /Y LobbyMsgProtocol.cs "../../../Mighty_Server/Server/Packet"
XCOPY /Y GameMsgProtocol.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet/Protocol"
XCOPY /Y GameMsgProtocol.cs "../../../Mighty_Server/Server/Packet"
XCOPY /Y UnityProtocol.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet/Protocol/Unity"
XCOPY /Y UnityProtocol.cs "../../../Mighty_Server/Server/Packet/Unity"
XCOPY /Y GameProtocol.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet/Protocol/Unity"
XCOPY /Y GameProtocol.cs "../../../Mighty_Server/Server/Packet/Unity"
XCOPY /Y ClientPacketManager.cs "../../../Mighty_Client/Assets/Server/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Mighty_Server/Server/Packet"