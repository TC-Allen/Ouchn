namespace Ouchn.Util
{
    public class GridData
    {
        public string Title { get; set; }
        public string State { get; set; }

        public GridData(string title)
        {
            Title = title;
            State = "doing";
        }

        public GridData(string title, string state)
        {
            Title = title;
            State = state;
        }
    }
}
