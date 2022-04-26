using System.Windows.Documents;

namespace App07.Print.Views;

public interface IDocumentRenderer
{
    void Render(FlowDocument doc, object data);
}