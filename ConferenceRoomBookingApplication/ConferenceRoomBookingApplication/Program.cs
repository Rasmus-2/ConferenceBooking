using ConferenceRoomBookingApplication.Models;

namespace ConferenceRoomBookingApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Models.User currentUser = ShowStartPage();

            if (currentUser.IsAdmin)
            {
                ShowAdminOptions(currentUser);
            }
            else
            {
                ShowUserOptions(currentUser);
            }

            Console.WriteLine("\nMain ran through!");
        }

        public static Models.User ShowStartPage()
        {
            Models.User currentUser = new Models.User();
            Console.WriteLine("Welcome to the booking app");
            Console.WriteLine("[1] Login");
            Console.WriteLine("[2] Create account");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    currentUser = Login();
                    break;

                case '2':
                    currentUser = CreateAccount();
                    break;
            };
            Console.Clear();
            string firstName = currentUser.Name.Split(' ').First();
            Console.WriteLine("Welcome " + firstName);
            return currentUser;
        }

        public static Models.User Login()
        {
            Models.User user = new Models.User();

            Console.Clear();
            Console.Write("Enter username: ");
            string userName = Console.ReadLine();
            user = GetUser(userName);

            while (user == null)
            {
                Console.Clear();
                Console.WriteLine("We can't find your account, please try again");
                Console.Write("Enter username: ");
                userName = Console.ReadLine();
                user = GetUser(userName);
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            while (user.Password != password)
            {
                Console.Clear();
                Console.WriteLine("Wrong password, please try again");
                Console.Write("Enter password: ");
                password = Console.ReadLine();
            }
            return user;
        }

        public static Models.User GetUser(string userName)
        {
            using (var myDb = new MyDbContext())
            {
                var user = (from u in myDb.Users
                            where u.UserName == userName
                            select u).FirstOrDefault();
                return user;
            }
        }

        public static Models.User GetUser(int userId)
        {
            using (var myDb = new MyDbContext())
            {
                var user = (from u in myDb.Users
                            where u.Id == userId
                            select u).FirstOrDefault();
                return user;
            }
        }

        public static Models.User CreateAccount()
        {
            Console.Clear();

            //--Check if containing a space (eg. Firstname Lastname)
            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            //--Check if unique
            Console.Write("Choose a username: ");
            string userName = Console.ReadLine();

            //--Minimum 8 characters, both digits and letters
            Console.Write("Choose a password: ");
            string password = Console.ReadLine();

            //--Check if contains @-symbol
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter company name: ");
            string companyName = Console.ReadLine();
            Models.Company company = GetCompany(companyName);
            if (company == null)
            {
                company = CreateCompany(companyName);
            }

            Console.Write("Enter your department at your company: ");
            string department = Console.ReadLine();

            //--Set IsAdmin to false

            Models.User newUser = new Models.User()
            {
                Name = name,
                UserName = userName,
                Password = password,
                Email = email,
                Phone = phoneNumber,
                CompanyId = company.Id,
                Department = department,
                IsAdmin = false
            };

            using (var myDb = new MyDbContext())
            {
                myDb.Users.Add(newUser);
                myDb.SaveChanges();
                Models.User user = GetUser(userName);
                return user;
            }
        }

        public static void ShowUsers()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var user in myDb.Users)
                {
                    Console.WriteLine("[" + user.Id + "] " + user.Name);
                }
            }
        }

        public static void ShowUserInfo(int userId)
        {
            Models.User user = GetUser(userId);
            if (user.IsAdmin)
            {
                Console.WriteLine("Id:           " + user.Id + " (Admin)");
            }
            else
            {
                Console.WriteLine("Id:           " + user.Id);
            }
            Console.WriteLine("Name:         " + user.Name);
            Console.WriteLine("Username:     " + user.UserName);
            Console.WriteLine("Email:        " + user.Email);
            Console.WriteLine("Phone number: " + user.Phone);
            Console.WriteLine("Company:      " + GetCompany(user.CompanyId).Name);
            Console.WriteLine("Department:   " + user.Department);
        }

        public static int GetOrCreateCityId(string cityName)
        {
            using (var myDb = new MyDbContext())
            {
                var citySearch = (from c in myDb.Cities
                                  where c.Name == cityName
                                  select c).SingleOrDefault();

                if (citySearch == null)
                {
                    Models.City city = new City() { Name = cityName };
                    myDb.Cities.Add(city);
                    myDb.SaveChanges();

                    var newCityId = (from c in myDb.Cities
                                     where c.Name == cityName
                                     select c.Id).SingleOrDefault();
                    return newCityId;
                }
                else
                {
                    return citySearch.Id;
                }
            }
        }

        public static Models.City GetCity(int cityId)
        {
            using (var myDb = new MyDbContext())
            {
                var city = (from c in myDb.Cities
                            where c.Id == cityId
                            select c).SingleOrDefault();
                return city;
            }
        }

        public static int GetOrCreateCountryId(string countryName)
        {
            using (var myDb = new MyDbContext())
            {
                var countrySearch = (from c in myDb.Countries
                                     where c.Name == countryName
                                     select c).SingleOrDefault();

                if (countrySearch == null)
                {
                    Models.Country country = new Country() { Name = countryName };
                    myDb.Countries.Add(country);
                    myDb.SaveChanges();

                    var newCountryId = (from c in myDb.Countries
                                        where c.Name == countryName
                                        select c.Id).SingleOrDefault();
                    return newCountryId;
                }
                else
                {
                    return countrySearch.Id;
                }
            }
        }

        public static Models.Country GetCountry(int countryId)
        {
            using (var myDb = new MyDbContext())
            {
                var country = (from c in myDb.Countries
                               where c.Id == countryId
                               select c).SingleOrDefault();
                return country;
            }
        }

        public static Models.Company GetCompany(string companyName)
        {
            using (var myDb = new MyDbContext())
            {
                var company = (from c in myDb.Companies
                               where c.Name.ToLower() == companyName.ToLower()
                               select c).SingleOrDefault();
                return company;
            }
        }

        public static Models.Company GetCompany(int companyId)
        {
            using (var myDb = new MyDbContext())
            {
                var company = (from c in myDb.Companies
                               where c.Id == companyId
                               select c).SingleOrDefault();
                return company;
            }
        }

        public static Models.Company CreateCompany(string companyName)
        {
            Console.Write("Enter your companys registration number: ");
            string registrationNumber = Console.ReadLine();

            Console.WriteLine("Enter your companys billing address");
            Console.Write("Street: ");
            string street = Console.ReadLine();

            Console.Write("Postal code: ");
            string postalCode = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();
            int cityId = GetOrCreateCityId(city);

            Console.Write("Country: ");
            string country = Console.ReadLine();
            int countryId = GetOrCreateCountryId(country);

            Models.Company company = new Company()
            {
                Name = companyName,
                RegistrationNumber = registrationNumber,
                Street = street,
                PostalCode = postalCode,
                CityId = cityId,
                CountryId = countryId
            };

            using (var myDb = new MyDbContext())
            {
                myDb.Companies.Add(company);
                myDb.SaveChanges();
                return GetCompany(companyName);
            }
        }

        public static void ShowCompanies()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var company in myDb.Companies)
                {
                    Console.WriteLine("[" + company.Id + "] " + company.Name);
                }
            }
        }

        public static void ShowCompanyInfo(int companyId)
        {
            Models.Company company = GetCompany(companyId);
            Console.WriteLine("Id:         " + company.Id);
            Console.WriteLine("Name:       " + company.Name);
            Console.WriteLine("Reg number: " + company.RegistrationNumber);
            Console.WriteLine("Address:");
            Console.WriteLine("\t" + company.Street);
            Console.WriteLine("\t" + company.PostalCode);
            Console.WriteLine("\t" + GetCity(company.CityId).Name);
            Console.WriteLine("\t" + GetCountry(company.CountryId).Name);
        }

        public static void ShowUserOptions(Models.User currentUser)
        {
            Console.WriteLine("User options");
            Console.WriteLine("-------------------------");
            Console.WriteLine("[1] Show room info");
            Console.WriteLine("[2] Show booking calendar");
            Console.WriteLine("[3] View my bookings");
            Console.WriteLine("[4] Show costs");
            Console.WriteLine("[5] My account");
            Console.WriteLine("[6] Log out");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    ShowRooms();
                    break;
                case '2':
                    Console.Clear();
                    ShowCalendar();
                    break;
                case '3':
                    Console.Clear();
                    ShowMyBookings(currentUser.Id);
                    break;
                case '4':
                    Console.Clear();
                    ShowMyCosts(currentUser.Id);
                    break;
                case '5':
                    Console.Clear();
                    ShowUserInfo(currentUser.Id);
                    Console.ReadLine();
                    Console.Clear();
                    ShowCompanyInfo(currentUser.CompanyId);
                    Console.ReadLine();
                    Console.Clear();
                    ShowCompanies();
                    Console.ReadLine();
                    Console.Clear();
                    ShowUsers();
                    break;
                case '6':
                    Console.Clear();
                    ShowStartPage();
                    break;
            }
        }

        public static void ShowRooms()
        {
            //Show list of all rooms and their info
            //if admin, add option to change room info

            using (var myDb = new MyDbContext())
            {
                foreach (var room in myDb.Rooms)
                {
                    Console.WriteLine("[" + room.Id + "] " + room.Name + ": " + room.Description);
                    Console.WriteLine("Size: " + "Seats: " + room.Seats + "Facilities: " + "Price: " + room.Price);
                    Console.WriteLine();
                }
            }
        }

        public static void AddRoom()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();

            //Add enum and show what's available
            Console.WriteLine("Size: ");
            int size = int.Parse(Console.ReadLine());

            Console.WriteLine("Seats: ");
            int seats = int.Parse(Console.ReadLine());

            Console.WriteLine("Description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Price: ");
            int price = int.Parse(Console.ReadLine());

            //Add facilities

        }

        public static void ShowCalendar()
        {
            //--Show grid of booked and available rooms
            //--Options menu for: Change week, Book, Rebook, Cancel booking, change booking
        }

        public static void ShowMyBookings(int userId)
        {

        }

        public static void ShowMyCosts(int userId)
        {

        }

        public static void ShowAdminOptions(Models.User currentUser)
        {
            //--Options menu for: Add rooms, Show your own info, Show optional userinfo, show optional company info,
            //Economy overview, Show room overview,
            //Statistics: show popular room, show popular facilities,
            //person that done most bookings, percentages of room available/booked per
            //week/in total, more...
            Console.WriteLine("Admin options");
            Console.WriteLine("-------------------------");
            Console.WriteLine("[1] Show room info");
            Console.WriteLine("[2] Show booking calendar");
            Console.WriteLine("[3] View my bookings");
            Console.WriteLine("[4] Add room");
            Console.WriteLine("[5] Show users");
            Console.WriteLine("[6] Show companies");
            Console.WriteLine("[7] Economic overview");
            Console.WriteLine("[8] Statistics");
            Console.WriteLine("[9] My Account");
            Console.WriteLine("[0] Log out");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    ShowRooms();
                    break;
                case '2':
                    Console.Clear();
                    ShowCalendar();
                    break;
                case '3':
                    Console.Clear();
                    ShowMyBookings(currentUser.Id);
                    break;
                case '4':
                    Console.Clear();
                    AddRoom();
                    break;
                case '5':
                    Console.Clear();
                    ShowUsers();
                    break;
                case '6':
                    Console.Clear();
                    ShowCompanies();
                    break;
                case '7':
                    Console.Clear();
                    ShowEconomy();
                    break;
                case '8':
                    Console.Clear();
                    ShowStatistics();
                    break;
                case '9':
                    Console.Clear();
                    ShowUserInfo(currentUser.Id);
                    break;
                case '0':
                    Console.Clear();
                    ShowStartPage();
                    break;
            }
        }

        public static void ShowEconomy()
        {

        }

        public static void ShowStatistics()
        {

        }
    }
}
