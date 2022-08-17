using System;
namespace QrCodeValidator.Models
{
	public class Utxc
	{

        public int Id { get; set; }

        public string TxCode { get; set; }

		public bool IsConfirmed { get; set; }

	}
}

