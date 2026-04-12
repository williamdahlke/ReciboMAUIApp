using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ReciboMAUIApp.Model;

public class AcquittanceDocument : IDocument
{
    public AcquittanceDocument(int id, double? price, string customerName, string description)
    {
        Id = id;
        Price = price.GetValueOrDefault();
        CustomerName = customerName;
        Description = description;
        ServiceDate = DateTime.Now;
        FactoryName = Factory.Name;
        FactoryAddress = Factory.Address;
        FactoryCellPhone = Factory.CellPhone;
        FactoryDocument = Factory.Document;
        City = $"{Factory.City}, {Factory.State}, {DateTime.Now.ToLongDateString()}";
    }

    public int Id { get; set; }
    public double Price { get; set; }
    public string? CustomerName { get; set; }
    public string? Description { get; set; }
    public string? City { get; set; }

    public DateTime ServiceDate { get; set; }
    public string? FactoryName { get; set; }
    public string? FactoryAddress { get; set; }
    public string? FactoryDocument { get; set; }
    public string? FactoryCellPhone { get; set; }

    public override string ToString()
    {
        return $"{Id}_{ServiceDate}";
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Content().Column(col =>
            {
                // Título
                col.Item().Text($"RECIBO Nº {Id} valor de R$ {Price}")
                    .Bold().FontSize(16);

                col.Item().PaddingTop(20);

                // Texto principal
                col.Item().Text($"Recebi (emos) de {FactoryName}");

                col.Item().PaddingTop(10);

                col.Item().Text($"Referente à {Description}");

                col.Item().PaddingTop(20);

                col.Item().Text("Por ser verdade, firmo o presente.");

                col.Item().PaddingTop(20);

                col.Item().Text(City);

                col.Item().PaddingTop(20);

                // Emitente
                col.Item().Text($"Nome do emitente: {FactoryName}");

                col.Item().Text($"Endereço: {FactoryAddress}");

                col.Item().PaddingTop(10);

                // Rodapé com CNPJ e celular
                col.Item().Text($"CNPJ: {FactoryDocument}    Celular: {FactoryCellPhone}");
            });
        });
    }
}