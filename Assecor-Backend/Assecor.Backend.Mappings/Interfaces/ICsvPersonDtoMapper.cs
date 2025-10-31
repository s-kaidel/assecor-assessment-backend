using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Requests;

namespace Assecor.Backend.Mappings.Interfaces
{
    public interface ICsvPersonDtoMapper
    {
        CsvPersonDto MapFromCreateCsvPersonRequest(CreateCsvPersonRequest request);
    }
}
