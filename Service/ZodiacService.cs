using AutoMapper;
using HoshiVibe.DB;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace HoshiVibe.Service
{
    public class ZodiacService
    {
        private readonly ZodiacRepository _zodiacRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ZodiacService(ZodiacRepository zodiacRepository, IMapper mapper, DataContext context)
        {
            _zodiacRepository = zodiacRepository;
            _context = context;
            _mapper = mapper;
        }

        public Zodiac? GetZodiacByDate(DateTime dob)
        {
            var m = dob.Month;
            var d = dob.Day;


            string name = m switch
            {
                1 => d <= 19 ? "Ma Kết (Capricorn)" : "Bảo Bình (Aquarius)",
                2 => d <= 18 ? "Bảo Bình (Aquarius)" : "Song Ngư (Pisces)",
                3 => d <= 20 ? "Song Ngư (Pisces)" : "Bạch Dương (Aries)",
                4 => d <= 19 ? "Bạch Dương (Aries)" : "Kim Ngưu (Taurus)",
                5 => d <= 20 ? "Kim Ngưu (Taurus)" : "Song Tử (Gemini)",
                6 => d <= 20 ? "Song Tử (Gemini)" : "Cự Giải (Cancer)",
                7 => d <= 22 ? "Cự Giải (Cancer)" : "Sư Tử (Leo)",
                8 => d <= 22 ? "Sư Tử (Leo)" : "Xử Nữ (Virgo)",
                9 => d <= 22 ? "Xử Nữ (Virgo)" : "Thiên Bình (Libra)",
                10 => d <= 22 ? "Thiên Bình (Libra)" : "Bọ Cạp (Scorpio)",
                11 => d <= 21 ? "Bọ Cạp (Scorpio)" : "Nhân Mã (Sagittarius)",
                12 => d <= 21 ? "Nhân Mã (Sagittarius)" : "Ma Kết (Capricorn)",
                _ => "Không xác định"
            };
            string Normalize(string input)
            {
                if (string.IsNullOrWhiteSpace(input)) return input;
                var formD = input.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var ch in formD)
                {
                    var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                    if (cat != UnicodeCategory.NonSpacingMark)
                        sb.Append(ch);
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            string target = Normalize(name);

            return _context.Zodiacs
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault(z => string.Equals(Normalize(z.Name), target, StringComparison.OrdinalIgnoreCase));
        }


        public ICollection<Zodiac> GetAllZodiacs()
        {
            return _zodiacRepository.GetAllZodiacSigns();
        }
        public Zodiac? GetZodiacById(int id)
        {
            return _zodiacRepository.GetZodiacById(id);
        }
        public bool UpdateZodiac(int id,ZodiacUpdateDTO zodiac)
        {
            var existingZodiac = _zodiacRepository.GetZodiacById(id);
            if (existingZodiac == null) return false;
            _mapper.Map(zodiac, existingZodiac);

            return _zodiacRepository.UpdateZodiac(existingZodiac);
        }
    }
}
