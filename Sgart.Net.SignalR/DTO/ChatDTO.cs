namespace Sgart.Net.SignalR.DTO
{
    /// <summary>
    /// rappresenta il messaggio da scambiare con SignalR
    /// </summary>
    public class ChatDTO
    {
        public string? ClientId { get; set; }
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Message { get; set; }
    }
}
