using System;
using System.Collections.Generic;
using Capriz_WPF.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Capriz_WPF.Common
{
    public static class StatusReportExporter
    {
        /// <summary>
        /// Экспортирует список записей Data.Data в PDF-отчёт (landscape A4).
        /// </summary>
        public static void Export(List<Data.Data> records, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(15);
                    page.DefaultTextStyle(t => t.FontSize(8).FontFamily("Lato"));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(c => ComposeContent(c, records));
                    page.Footer().Element(ComposeFooter);
                });
            })
            .GeneratePdf(filePath);
        }

        private static void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("Отчёт о статусах датчиков")
                    .FontSize(16).Bold();

                col.Item().Text($"Сформирован: {DateTime.Now:dd.MM.yyyy HH:mm:ss}")
                    .FontSize(10);

                col.Item().PaddingBottom(8).LineHorizontal(1);
            });
        }

        private static void ComposeContent(IContainer container, List<Data.Data> records)
        {
            container.Table(table =>
            {
                // 13 колонок: Дата/время + 12 статусов
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(85);      // Дата/время
                    columns.RelativeColumn();        // Temp1
                    columns.RelativeColumn();        // Temp2
                    columns.RelativeColumn();        // Hum1
                    columns.RelativeColumn();        // Hum2
                    columns.RelativeColumn();        // Wind1
                    columns.RelativeColumn();        // Wind2
                    columns.RelativeColumn();        // WindWMT
                    columns.RelativeColumn();        // Pressure1
                    columns.RelativeColumn();        // Pressure2
                    columns.RelativeColumn();        // SKYDEX
                    columns.RelativeColumn();        // AmountClouds
                    columns.RelativeColumn();        // DMDV
                });

                // Шапка
                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("Дата/время");
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusTemp1"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusTemp2"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusHum1"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusHum2"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusWind1"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusWind2"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusWindWMT"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusPressure1"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusPressure2"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusSKYDEX"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("AmountClouds"));
                    header.Cell().Element(HeaderCell).Text(StatusConverter.ShortHeader("StatusDMDV"));
                });

                // Данные
                foreach (var d in records)
                {
                    BodyCell(table, $"{d.Date} {d.Time}");
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusTemp1));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusTemp2));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusHum1));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusHum2));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusWind1));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusWind2));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusWindWMT));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusPressure1));
                    BodyCell(table, StatusConverter.Status01ToText(d.StatusPressure2));
                    BodyCell(table, StatusConverter.SkydexToText(d.StatusSKYDEX));
                    BodyCell(table, StatusConverter.AmountCloudsToText(d.AmountClouds));
                    BodyCell(table, StatusConverter.DmdvToText(d.StatusDMDV));
                }
            });
        }

        private static void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(t =>
            {
                t.Span("Страница ");
                t.CurrentPageNumber();
                t.Span(" из ");
                t.TotalPages();
            });
        }

        // ---------- Хелперы для ячеек ----------

        private static IContainer HeaderCell(IContainer container) =>
            container
                .Background(Colors.Grey.Lighten2)
                .BorderBottom(1).BorderColor(Colors.Grey.Darken1)
                .Padding(3)
                .AlignCenter()
                .AlignMiddle();

        private static void BodyCell(TableDescriptor table, string text)
        {
            table.Cell()
                .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                .Padding(3)
                .AlignLeft()
                .AlignMiddle()
                .Text(text ?? "Н.Д.");
        }
    }
}