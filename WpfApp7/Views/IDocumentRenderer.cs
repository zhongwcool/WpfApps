using System.Windows.Documents;

namespace WpfApp7.Views;

public interface IDocumentRenderer
{
    void Render(FlowDocument doc, object data);
}