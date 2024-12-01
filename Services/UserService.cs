using System.Linq;
using bigSales.Data;
using bigSales.Models;
using Microsoft.AspNetCore.Identity;

namespace bigSales.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>(); 
        }


        public bool IsEmailExists(string email)
        {
            var existingUser = _context.Users.SingleOrDefault(u => u.Email == email);
            return existingUser != null;
        }


        public bool RegisterUser(User user, string password)
        {

            if (IsEmailExists(user.Email)) 
            {
                return false; 
            }


            user.Password = _passwordHasher.HashPassword(user, password);


            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }


        public User? Login(string email, string password) 
        {

            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null; 
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);


            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null; 
        }
    }
}
