namespace BlazorApp.Data
{
    public class Request : ICloneable
    {
        public Request() { }
        public Request(int id, int number, DateTime created, RequestState state, string department, string author, List<Material> materials)
        {
            Id = id;
            Number = number;
            Created = created;
            State = state;
            Department = department;
            Author = author;
            Materials = materials;
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime Created { get; set; }

        public string FullNumber => (Created.Year % 100).ToString() + "/" + string.Format("{0:d4}", Number);
        
        public RequestState State { get; set; }

        public string TextStatus => State switch
        {
            RequestState.CREATED => "Создана",
            RequestState.DELETED => "Удалена",
            RequestState.APPROVED => "Утверждена",
            _ => "",
        };

        public string Department { get; set; } = "";

        public string Author { get; set; } = "";

        public List<Material> Materials { get; set; }

        public object Clone() => new Request(Id, Number, Created, State, Department, Author, Materials);
    }

    public enum RequestState : byte
    {
        CREATED,
        DELETED,
        APPROVED,
    }
}
