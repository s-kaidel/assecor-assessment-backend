using System.Text;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Requests;

namespace Assecor.Backend.Mappings
{
    public class CsvPersonDtoMapper
    {
        public CsvPersonDto MapFromCreateCsvPersonRequest(CreateCsvPersonRequest request)
        {
            var location = CreateLocationString(request);
            var dto = new CsvPersonDto()
            {
                Name = request.Name,
                LastName = request.LastName,
                Color = request.Color,
                Location = location
            };
            return dto;
        }

        private string CreateLocationString(CreateCsvPersonRequest request)
        {
            var sb = new StringBuilder();
            if (request.ZipCode != null)
            {
                sb.Append(request.ZipCode.ToString());
            }

            if (request.City != null)
            {
                sb.Append($" {request.City}");
            }

            return sb.ToString();
        }
    }
}
