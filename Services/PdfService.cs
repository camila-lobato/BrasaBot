using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BrasaBot.Services
{
    public class PdfService
    {
        public byte[] GerarPdf(string titulo, string subtitulo, List<string[]> linhas, string[] cabecalhos)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .Column(col =>
                        {
                            col.Item().Text(titulo).SemiBold().FontSize(16);
                            col.Item().Text(subtitulo).FontSize(8).FontColor(Colors.Grey.Darken1);
                        });

                    page.Content()
                        .PaddingVertical(15)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in cabecalhos)
                                    columns.RelativeColumn();
                            });

                            // Cabeçalho
                            table.Header(header =>
                            {
                                foreach (var c in cabecalhos)
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(c).SemiBold();
                            });

                            // Linhas
                            foreach (var linha in linhas)
                            {
                                foreach (var celula in linha)
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(celula);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Gerado em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                });
            });

            return doc.GeneratePdf();
        }
    }
}