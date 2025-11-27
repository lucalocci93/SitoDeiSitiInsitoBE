namespace SitoDeiSiti.Backend.DTOs
{

    public enum TemplateName
    {
        ReportIscrizioneUtente,
        ReportIscrizioniDaMaestro
    };

    public record TemplateDTO
    {
        public string? TemplateName { get; set; }
        public string? TemplateBodyHtml { get; set; }
        public string? TemplateHeaderHtml { get; set; }
        public string? TemplateFooterHtml { get; set; }
    }
}
