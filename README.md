# portuguese-news-aggregator

A background worker that collects articles from Portuguese news sites and stores them in a local database, so they can be analysed later.

## How it works

- A Quartz job runs when the worker starts and then every hour.
- It reads each source listed in `Worker/appsettings.json`. Publico and Observador are read as JSON, Noticias ao Minuto (tech and lifestyle) as RSS.
- New articles are saved to a LiteDB file, `newsDatabase.db`, next to the worker. An article that is already stored (same source and source id) is skipped.
- Each article keeps its source, title, image, publish date and categories.

## Structure

The solution has five projects: Domain, Application, Database, Infrastructure and Worker.

- Domain: the entities and the repository interface.
- Application: the readers for each site, the reader services and the Quartz job.
- Database: the LiteDB repository.
- Infrastructure: dependency injection setup.
- Worker: the host. It supports systemd, so it can run as a Linux service.

## Running it

You need the .NET 10 SDK.

```bash
git clone https://github.com/tiagossa1/portuguese-news-aggregator.git
cd portuguese-news-aggregator
dotnet run --project Worker
```

To add or remove a news source, edit the `NewsWebsites` section of `Worker/appsettings.json`. Each source has a URL, a code and a type (`JSON` or `RSS`). RSS sources can also set a `CategoryCode`.

### Docker

```bash
docker compose up -d --build
```

The compose file mounts `appsettings.json`, `appsettings.Development.json` and `appsettings.Production.json` from the `Worker` folder. `appsettings.Production.json` is git-ignored, so create it before starting the container.
