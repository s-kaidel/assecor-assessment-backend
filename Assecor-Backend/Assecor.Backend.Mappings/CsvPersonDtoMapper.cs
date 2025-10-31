using System.Text;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Requests;
using Assecor.Backend.Mappings.Interfaces;

namespace Assecor.Backend.Mappings
{
    public class CsvPersonDtoMapper : ICsvPersonDtoMapper
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

        private static string? CreateLocationString(CreateCsvPersonRequest request)
        {
            var hasZipCode = request.ZipCode != null;
            var hasCity = !string.IsNullOrWhiteSpace(request.City);

            if (!hasZipCode && !hasCity)
            {
                return null;
            }

            var sb = new StringBuilder();

            if (hasZipCode)
            {
                sb.Append(request.ZipCode.ToString());
            }

            if (!hasCity)
            {
                return sb.ToString();
            }

            if (hasZipCode)
            {
                sb.Append(' ');
            }

            sb.Append(request.City);

            return sb.ToString();
        }
    }
}
