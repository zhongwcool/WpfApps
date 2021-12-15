using System.Windows.Documents;

namespace WpfApp7;

public interface IDocumentRenderer
{
    void Render(FlowDocument doc, object data);
}