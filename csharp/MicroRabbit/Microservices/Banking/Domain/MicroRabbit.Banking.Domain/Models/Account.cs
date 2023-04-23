namespace MicroRabbit.Banking.Domain.Models
{
	public class Account
	{
		public int Id { get; set; }
		public string? AccountType { get; set; }
		public double AccountBalance { get; set; }
	}
}