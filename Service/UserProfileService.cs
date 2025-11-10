using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HoshiVibe.Service
{
    public class UserProfileService
    {
        private readonly UserRepository _userRepo;
        private readonly UserProfileRepository _userProfileRepo;
        private readonly ZodiacService _zodiacService;
        private readonly DestinyService _destinyService;

        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserProfileService(
            UserRepository userRepo,
            UserProfileRepository userProfileRepo,
            IMapper mapper,
            DestinyService destinyService,
            PasswordHasher<User> passwordHasher,
            ZodiacService zodiacService)
        {
            _userRepo = userRepo;
            _userProfileRepo = userProfileRepo;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _zodiacService = zodiacService;
            _destinyService = destinyService;
        }

        public ICollection<UserDTO> GetAllUserInformations()
        {
            var users = _userProfileRepo.GetAllUserInformations();
            if (users == null || users.Count == 0)
                throw new InvalidOperationException("No users found");

            return _mapper.Map<ICollection<UserDTO>>(users);
        }

        public UserDTO GetUserProfileByUserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("UserId không hợp lệ", nameof(id));

            var existingUser = _userProfileRepo.GetProfileByUserId(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User profile not found");

            return _mapper.Map<UserDTO>(existingUser);
        }

        public bool UpdateProfile(Guid id, ProfileUpdateDTO dto)
        {
            var existingProfile = _userProfileRepo.GetUserProfileByProfileId(id);
            if (existingProfile == null) return false;

            _mapper.Map(dto, existingProfile);

            if (dto.Yob != DateTime.MinValue && dto.Yob != default)
            {
                var zodiac = _zodiacService.GetZodiacByDate(dto.Yob);
                if (zodiac != null)
                    existingProfile.ZodiacId = zodiac.Id;

                var destiny = GetDestinyByYob(dto.Yob);
                if (destiny != null)
                {
                    existingProfile.DestinyId = destiny.Id;
                    Console.WriteLine($"[LOG] Gán Destiny: {destiny.Name} (ID={destiny.Id})");
                }
                else
                {
                    Console.WriteLine($"[LOG] Không tìm thấy Destiny tương ứng cho năm {dto.Yob.Year}");
                }
            }

            return _userProfileRepo.UpdateProfile(existingProfile);
        }

        public bool UpdateUser(Guid userId, string email, string password)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;

            if (!string.IsNullOrWhiteSpace(password))
                user.Password = _passwordHasher.HashPassword(user, password);

            return _userRepo.UpdateUser(user);
        }

        private Destiny? GetDestinyByYob(DateTime yob)
        {
            int year = yob.Year;
            string element = GetElementByYear(year);

            return _destinyService
                .GetAllDestinies()
                .FirstOrDefault(d => d.Name.Equals(element, StringComparison.OrdinalIgnoreCase));
        }

        private string GetElementByYear(int year)
        {
            return year switch
            {
                1924 or 1925 or 1984 or 1985 or 2044 or 2045 => "Kim", // Hải Trung Kim
                1926 or 1927 or 1986 or 1987 or 2046 or 2047 => "Hỏa", // Lư Trung Hỏa
                1928 or 1929 or 1988 or 1989 or 2048 or 2049 => "Mộc", // Đại Lâm Mộc
                1930 or 1931 or 1990 or 1991 or 2050 or 2051 => "Thổ", // Lộ Bàng Thổ
                1932 or 1933 or 1992 or 1993 or 2052 or 2053 => "Kim", // Kiếm Phong Kim
                1934 or 1935 or 1994 or 1995 or 2054 or 2055 => "Thủy", // Giản Hạ Thủy
                1936 or 1937 or 1996 or 1997 or 2056 or 2057 => "Thủy", // Giản Hạ Thủy
                1938 or 1939 or 1998 or 1999 or 2058 or 2059 => "Thổ", // Thành Đầu Thổ
                1940 or 1941 or 2000 or 2001 or 2060 or 2061 => "Kim", // Bạch Lạp Kim
                1942 or 1943 or 2002 or 2003 or 2062 or 2063 => "Mộc", // Dương Liễu Mộc
                1944 or 1945 or 2004 or 2005 or 2064 or 2065 => "Thủy", // Tuyền Trung Thủy ✅
                1946 or 1947 or 2006 or 2007 or 2066 or 2067 => "Thổ", // Ốc Thượng Thổ
                1948 or 1949 or 2008 or 2009 or 2068 or 2069 => "Hỏa", // Tích Lịch Hỏa
                1950 or 1951 or 2010 or 2011 or 2070 or 2071 => "Mộc", // Tùng Bách Mộc
                1952 or 1953 or 2012 or 2013 or 2072 or 2073 => "Kim", // Kim Bạch Kim
                1954 or 1955 or 2014 or 2015 or 2074 or 2075 => "Thủy", // Tuyền Trung Thủy
                1956 or 1957 or 2016 or 2017 or 2076 or 2077 => "Hỏa",
                1958 or 1959 or 2018 or 2019 or 2078 or 2079 => "Mộc",
                1960 or 1961 or 2020 or 2021 or 2080 or 2081 => "Thổ",
                1962 or 1963 or 2022 or 2023 or 2082 or 2083 => "Kim",
                1964 or 1965 or 2024 or 2025 or 2084 or 2085 => "Hỏa",
                1966 or 1967 or 2026 or 2027 or 2086 or 2087 => "Thủy",
                1968 or 1969 or 2028 or 2029 or 2088 or 2089 => "Thổ",
                1970 or 1971 or 2030 or 2031 or 2090 or 2091 => "Kim",
                1972 or 1973 or 2032 or 2033 or 2092 or 2093 => "Mộc",
                1974 or 1975 or 2034 or 2035 or 2094 or 2095 => "Thủy",
                1976 or 1977 or 2036 or 2037 or 2096 or 2097 => "Hỏa",
                1978 or 1979 or 2038 or 2039 or 2098 or 2099 => "Thổ",
                1980 or 1981 or 2040 or 2041 => "Mộc",
                1982 or 1983 or 2042 or 2043 => "Thủy",
                _ => "Không xác định"
            };
        }
    }
}