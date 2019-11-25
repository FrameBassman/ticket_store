using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.IO;
using System.Net.Http;
using DinkToPdf;
using DinkToPdf.Contracts;
using TicketStore.Data.Model;

namespace TicketStore.Api.Model.Pdf
{
    public class Pdf
    {
        private IConverter _converter;
        private readonly Preview _preview;

        public Pdf(Event concert, List<Ticket> tickets, IConverter converter, HttpClient client)
        {
            _converter = converter;
            _preview = new Preview(client, concert);
        }

        public byte[] ToBytes()
        {
            return _converter.Convert(Template());
        }

        private HtmlToPdfDocument Template()
        {
            return new HtmlToPdfDocument
            {
                GlobalSettings = {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings {
                        PagesCount = true,
                        HtmlContent = _preview.Layout(),
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };
        }
    }
}
