using System.Runtime.Serialization;

namespace ResourceReload.AltV.Features.AutoReconnect;

internal enum ClientStatus
{
    [EnumMember(Value = "ERROR")] 
    Error,
    [EnumMember(Value = "LOADING")] 
    Loading,
    [EnumMember(Value = "MAIN_MENU")] 
    MainMenu,
    [EnumMember(Value = "DOWNLOADING_FILES")] 
    DownloadingFiles,
    [EnumMember(Value = "CONNECTING")] 
    Connecting,
    [EnumMember(Value = "IN_GAME")] 
    InGame,
    [EnumMember(Value = "DISCONNECTING")] 
    Disconnecting
}