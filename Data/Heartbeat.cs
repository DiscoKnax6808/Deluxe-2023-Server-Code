using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeluxeNET.Data
{
    public class Heartbeat
    {
        [Key]
        public long Id { get; set; }

        public long PlayerId { get; set; }

        public int StatusVisibility { get; set; }

        public int Platform { get; set; }

        public int DeviceClass { get; set; }

        public long? RoomInstanceId { get; set; }

        [ForeignKey(nameof(RoomInstanceId))]
        public RoomInstance? RoomInstance { get; set; }

        public int VrMovementMode { get; set; }

        public DateTime LastOnline { get; set; } = DateTime.UtcNow;

        public bool IsOnline { get; set; }

        public string AppVersion { get; set; } = "20230406";
    }

    public class RoomInstance
    {
        [Key]
        public long RoomInstanceId { get; set; }

        public long RoomId { get; set; }

        public long SubRoomId { get; set; }

        public string Location { get; set; } = null!;

        public int RoomInstanceType { get; set; }

        public string PhotonRegionId { get; set; } = null!;

        public string PhotonRegion { get; set; } = null!;

        public string PhotonRoomId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int MaxCapacity { get; set; }

        public bool IsFull { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsInProgress { get; set; }

        public int MatchmakingPolicy { get; set; }

        public ICollection<Heartbeat> Heartbeats { get; set; } = new List<Heartbeat>();
    }
}