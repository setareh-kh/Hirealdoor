using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;
using Microsoft.EntityFrameworkCore;

namespace Hirealdoor.Repositories.Repository;

public class PersonRepository(SqlContext context) : BaseRepository<Person>(context), IPersonRepository
{
    public async Task<PaginateResponseDto<Person>> Filter(PersonIndexDto filter)
    {
        var query = context.Persons.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x => x.FullName.Contains(filter.Search) || x.LicenseNumber.Contains(filter.Search));
        }

        if (filter.Active != null)
        {
            // query = query.Where(x => x.Active == filter.Active);
        }

        if (filter.OfficeId != null)
        {
            query = query.Where(x => x.OfficeId == filter.OfficeId);
        }

        if (filter.Include)
        {
            query = query
                .Include(x => x.ServedAreas)
                .Include(x => x.SpeaksLanguages);
        }

        return await Paginate(filter, query);
    }

    //2,4,5
    public async Task<Person> SyncLanguages(Person person, string langIds)
    {
        var newIds = string.IsNullOrWhiteSpace(langIds)
            ? []
            : langIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .Distinct()
                .ToArray();

        await DbContext.Entry(person)
            .Collection(x => x.SpeaksLanguages)
            .LoadAsync();

        var currentIds = person.SpeaksLanguages
            .Select(x => x.Id)
            .ToArray();

        var idsToRemove = currentIds
            .Except(newIds)
            .ToArray();

        var idsToAdd = newIds
            .Except(currentIds)
            .ToArray();

        foreach (var language in person.SpeaksLanguages.Where(x => idsToRemove.Contains(x.Id)).ToList())
        {
            person.SpeaksLanguages.Remove(language);
        }

        foreach (var id in idsToAdd)
        {
            var language = new Language { Id = id, Name = string.Empty };

            DbContext.Languages.Attach(language);
            person.SpeaksLanguages.Add(language);
        }

        await SaveChangesAsync();

        return person;
    }
}