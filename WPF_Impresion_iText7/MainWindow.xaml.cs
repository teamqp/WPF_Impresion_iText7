using iText.Barcodes;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iTGeom = iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace WPF_Impresion_iText7
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathPDF = string.Empty;
        string path = string.Empty;
        string HTML = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            Inicializador();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GeneradorQR();
        }

        private void Inicializador()
        {

            //Rutas
            // C:\\impresion\\holi.pdf
            pathPDF = string.Concat(ConfigurationManager.AppSettings["PathPDF"], "holi.pdf");


            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\bin\\Debug", "");

            //HTML
            HTML = "<H1>Holi </H1> <img src=\"Recurso/logo.png\" />";
        }

        private void GeneradorQR()
        {
            ConverterProperties prop = new ConverterProperties();
            prop.SetBaseUri(path);
            PdfDocument pdf = new PdfDocument(new PdfWriter(pathPDF));
            Document document = new Document(pdf);
            document.SetMargins(0, 0, 0, 0);
            BarcodeQRCode qrCode = new BarcodeQRCode("Example QR Code Creation in iText7");
            PdfFormXObject barcodeObject = qrCode.CreateFormXObject(ColorConstants.BLACK, pdf);
            Image barcodeImage = new Image(barcodeObject).SetWidth(70f).SetHeight(70f);
            barcodeImage.SetMarginTop(650f).SetMarginLeft(120f);
            document.Add(new Paragraph().Add(barcodeImage));
            HtmlConverter.ConvertToPdf(HTML, pdf, prop);
            document.Close();
            Process.Start(pathPDF);
        }

        private void GeneradorQR_v1()
        {
            ConverterProperties prop = new ConverterProperties();
            prop.SetBaseUri(path);
            PdfDocument pdf = new PdfDocument(new PdfWriter(pathPDF));

            String url = "https://itextpdf.com/";

            // Adding QRCode to first page
            //PdfPage pdfPage = pdf.GetFirstPage();
            PdfPage pdfPage = pdf.GetPage(0);

            PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
            iTGeom.Rectangle rect = new iTGeom.Rectangle(100, 600, 100, 100);

            BarcodeQRCode barcodeQRCode = new BarcodeQRCode(url);
            PdfFormXObject pdfFormXObject =
                barcodeQRCode.CreateFormXObject(ColorConstants.BLACK, pdf);
            Image qrCodeImage =
                new Image(pdfFormXObject).SetWidth(rect.GetWidth()).SetHeight(rect.GetHeight());

            Canvas qrCanvas = new Canvas(pdfCanvas, rect);
            qrCanvas.Add(qrCodeImage);
            qrCanvas.Close();

            HtmlConverter.ConvertToPdf(HTML, pdf, prop);
            pdf.Close();
            Process.Start(pathPDF);
        }

    }
}
