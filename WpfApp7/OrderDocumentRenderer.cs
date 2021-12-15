using System.Globalization;
using System.Windows;
using System.Windows.Documents;

namespace WpfApp7;

internal class OrderDocumentRenderer : IDocumentRenderer
{
    public void Render(FlowDocument doc, object data)
    {
        var group = doc.FindName("rowsDetails") as TableRowGroup;
        var styleCell = doc.Resources["BorderedCell"] as Style;
        foreach (var item in ((OrderMaster)data).OrderDetails)
        {
            var row = new TableRow();

            var cell = new TableCell(new Paragraph(new Run(item.Sku)))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(new Paragraph(new Run(item.Spec)))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(new Paragraph(new Run(item.Number.ToString(CultureInfo.InvariantCulture))))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(new Paragraph(new Run(item.Unit)))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(new Paragraph(new Run(item.UnitPrice.ToString(CultureInfo.InvariantCulture))))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(
                new Paragraph(new Run((item.Number * item.UnitPrice).ToString(CultureInfo.InvariantCulture))))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            cell = new TableCell(new Paragraph(new Run(item.Description)))
            {
                Style = styleCell
            };
            row.Cells.Add(cell);

            group.Rows.Add(row);
        }
    }
}