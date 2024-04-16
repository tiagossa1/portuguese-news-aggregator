using Domain.Entities;

namespace Domain.Interfaces;

public interface INewsRepository
{
    void CreateRangeIfNotExists(IList<News> newsList);

    IList<News> GetAll();
}