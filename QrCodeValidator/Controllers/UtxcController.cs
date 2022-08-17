using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MimeKit;
using MimeKit.Utils;
using QRCoder;
using QrCodeValidator.Data;
using QrCodeValidator.Models;
using static System.Net.Mime.MediaTypeNames;

namespace QrCodeValidator.Controllers
{
    public class UtxcController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly IOptions<SmtpOptions> _options;
        private readonly AppDbContext _context;
        public UtxcController(IWebHostEnvironment environment, IEmailSender emailSender, IOptions<SmtpOptions> options, AppDbContext context)
        {
            _environment = environment;
            _emailSender = emailSender;
            _options = options;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendQrCode()
        {
            var guId = Guid.NewGuid().ToString();
            var utxc = new Utxc
            {
                TxCode = guId,
                IsConfirmed = false
            };
           await _context.GetUtxcs.AddAsync(utxc);
           await _context.SaveChangesAsync();

            
            //qr code generator
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://localhost:5001/Utxc/VeryfyQr?txCode={guId}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            //qr code generator
            System.IO.MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);

            var qrImageTag = $"<img src='{SigBase64}' style='width: 400px;'>";

            //string pathImage = Path.Combine(_environment.WebRootPath, "QrCodeImage", "qr.webp");


            _emailSender.SendEmail(_options.Value.UserName, "ishahbaz.shaikh@gmail.com", "scan Qr code below", qrImageTag);
            return RedirectToAction("Index");

        }

        public IActionResult VerifyQr(string txCode)
        {
            _context.GetUtxcs.Where(x => x.TxCode == txCode).FirstOrDefault().IsConfirmed = true;
            _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}