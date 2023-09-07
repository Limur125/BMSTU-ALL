using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Data
{
    public class Material : ICloneable
    {
        public Material() { }
        public Material(int id, MaterialState state, string name, string serial, uint count, string comment, int requestId, Request request)
        {
            Id = id;
            State = state;
            Name = name;
            Serial = serial;
            Count = count;
            Comment = comment;
            RequestId = requestId;
            Request = request;
        }

        public int Id { get; set; }

        public MaterialState State { get; set; }

        public string TextStatus => State switch
        {
            MaterialState.CREATED => "Создана",
            MaterialState.DELETED => "Удалена",
            _ => ""
        };

        public string Name { get; set; } = "";

        [MaxLength(10)]
        public string Serial { get; set; } = "";

        public uint Count { get; set; }

        public string Comment { get; set; } = "";

        public int RequestId { get; set; }

        public Request Request { get; set; }

        public object Clone() => new Material(Id, State, Name, Serial, Count, Comment, RequestId, Request);
    }
    public enum MaterialState : byte
    {
        CREATED,
        DELETED,
    }
}
