using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace SitoDeiSiti.Interfaces
{
    public interface ISito
    {
        Task<Response<List<Images>>> GetImmagini();
        Task<Response<List<Images>>> GetImmaginiByPagina(int Pagina);
        Task<Response<Images>> AddImmagine(Images immagine);
        Task<Response<Images>> RemoveImmagine(int Id);

        Task<Response<List<Pages>>> GetPagine();
    }
}
