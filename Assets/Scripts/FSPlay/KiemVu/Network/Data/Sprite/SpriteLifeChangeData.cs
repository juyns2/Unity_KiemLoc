using ProtoBuf;
using System;

namespace Server.Data
{
    /// <summary>
    /// Gói tin gửi từ Server về Client thông báo sinh lực, nội lực, thể lực của đối tượng thay đổi
    /// </summary>
    [ProtoContract]
    public class SpriteLifeChangeData
    {
        /// <summary>
        /// ID đối tượng
        /// </summary>
        [ProtoMember(1)]
        public int RoleID { get; set; }

        /// <summary>
        /// Sinh lực hiện tại
        /// </summary>
        [ProtoMember(2)]
        public Int64 HP { get; set; }		// int default HP

        /// <summary>
        /// Nội lực hiện tại
        /// </summary>
        [ProtoMember(3)]
        public int MP { get; set; }

        /// <summary>
        /// Thể lực hiện tại
        /// </summary>
        [ProtoMember(4)]
        public int Stamina { get; set; }

        /// <summary>
        /// Sinh lực tối đa
        /// </summary>
        [ProtoMember(5)]
        public Int64 MaxHP { get; set; }		// int default HP

        /// <summary>
        /// Nội lực tối đa
        /// </summary>
        [ProtoMember(6)]
        public int MaxMP { get; set; }

        /// <summary>
        /// Thể lực tối đa
        /// </summary>
        [ProtoMember(7)]
        public int MaxStamina { get; set; }
    }
}