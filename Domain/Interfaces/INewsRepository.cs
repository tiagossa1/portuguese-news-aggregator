using Domain.Entities;

namespace Domain.Interfaces;

public interface INewsRepository
{
    Task CreateRange(IList<News> newsList);

    Task<IList<News>> GetAll();
}