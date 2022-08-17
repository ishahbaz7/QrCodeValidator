using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace QrCodeValidator.Models
{
    public class GenerateQRCodeModel
    {

        [Display(Name = "Enter QR Code Text")]
        public string QRCodeText
        {
            get;
            set;
        }
    }

}