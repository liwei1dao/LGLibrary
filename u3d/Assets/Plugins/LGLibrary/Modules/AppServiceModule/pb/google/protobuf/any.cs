// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: any.proto

#pragma warning disable CS1591, CS0612, CS3021

namespace Google.Protobuf.WellKnownTypes
{
    [System.Serializable]
    [global::ProtoBuf.ProtoContract()]
    public partial class Any
    {
        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string type_url = "";

        [global::ProtoBuf.ProtoMember(2)]
        public byte[] value;

    }

}

#pragma warning restore CS1591, CS0612, CS3021
