namespace Financial.Events
{
    public class SelectionChangedEvent
    {
        public SelectionChangedEvent(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
