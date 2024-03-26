namespace ShTask.ViewModels
{
    public class MainInvoiceViewModel
    {
        public string CustomerName { get; set; } = null!;

        public DateTime Invoicedate { get; set; }

        public int? CashierId { get; set; }

        public int BranchId { get; set; }
    }
}
