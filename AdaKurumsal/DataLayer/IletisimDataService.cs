using AdaKurumsal.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AdaKurumsal.DataLayer
{
    public interface IIletisimDataService
    {
        Task<Iletisim> GetIletisim(string language);
        Task UpdateIletisim(Iletisim iletisim);
    }
    public class IletisimDataService : IIletisimDataService
    {
        private readonly EFContext _context;

        public IletisimDataService(EFContext context)
        {
            _context = context;
        }

        public async Task<Iletisim> GetIletisim(string language)
        {

            return await _context.Iletisim.FirstOrDefaultAsync(x => x.Language == language);
        }

        public async Task UpdateIletisim(Iletisim iletisim)
        {
            _context.Iletisim.Update(iletisim);
            await _context.SaveChangesAsync();
        }
    }
}
