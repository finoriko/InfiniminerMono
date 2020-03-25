using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniminerShared
{
    public enum Buttons
    {
        None = 0,

        Fire,
        AltFire,

        Forward,
        Backward,
        Left,
        Right,
        Sprint,
        Jump,
        Crouch,

        Ping,
        Deposit,
        Withdraw,

        //All buttons past this point will never be sent to the server
        SayAll,
        SayTeam,

        ChangeClass,
        ChangeTeam,

        Tool1,
        Tool2,
        Tool3,
        Tool4,
        Tool5,
        ToolUp,
        ToolDown,

        BlockUp,
        BlockDown
    }

    public enum MouseButton
    {
        LeftButton,
        MiddleButton,
        RightButton,
        WheelUp,
        WheelDown
    }

    public enum ScreenEffect
    {
        None,
        Death,
        Teleport,
        Fall,
        Explosion,
    }

    public enum InfiniminerSound
    {
        DigDirt,
        DigMetal,
        Ping,
        ConstructionGun,
        Death,
        CashDeposit,
        ClickHigh,
        ClickLow,
        GroundHit,
        Teleporter,
        Jumpblock,
        Explosion,
        RadarLow,
        RadarHigh,
        RadarSwitch,
    }

    public enum InfiniminerMessage : byte
    {
        BlockBulkTransfer,      // x-value, y-value, followed by 64 bytes of blocktype ; 
        BlockSet,               // x, y, z, type
        UseTool,                // position, heading, tool, blocktype 
        SelectClass,            // class
        ResourceUpdate,         // ore, cash, weight, max ore, max weight, team ore, red cash, blue cash: ReliableInOrder1
        DepositOre,
        DepositCash,
        WithdrawOre,
        TriggerExplosion,       // position

        PlayerUpdate,           // (uint id for server), position, heading, current tool, animate using (bool): UnreliableInOrder1
        PlayerJoined,           // uint id, player name :ReliableInOrder2
        PlayerLeft,             // uint id              :ReliableInOrder2
        PlayerSetTeam,          // (uint id for server), byte team   :ReliableInOrder2
        PlayerDead,             // (uint id for server) :ReliableInOrder2
        PlayerAlive,            // (uint id for server) :ReliableInOrder2
        PlayerPing,             // uint id

        ChatMessage,            // byte type, string message : ReliableInOrder3
        GameOver,               // byte team
        PlaySound,              // byte sound, bool isPositional, ?Vector3 location : ReliableUnordered
        TriggerConstructionGunAnimation,
        SetBeacon,              // vector3 position, string text ("" means remove)
    }

    public enum ChatMessageType
    {
        None,
        SayAll,
        SayRedTeam,
        SayBlueTeam,
    }

}
