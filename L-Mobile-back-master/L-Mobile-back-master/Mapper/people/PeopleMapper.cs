
public static class PeopleMapper
{
    public static PeopleDTO ToDTO(this People people)
    {
        return new PeopleDTO
        {
            Id = people.Id,
            Name = people.Name,
            CompanyId = people.CompanyId,
            CompanyName = people.Company?.Name // Optional: Include Company name if needed
        };
    }

    public static People ToEntity(this PeopleDTO peopleDTO)
    {
        return new People
        {
            Id = peopleDTO.Id,
            Name = peopleDTO.Name,
            CompanyId = peopleDTO.CompanyId
            // Assuming Company property is not set here; it might be managed separately
        };
    }
}
