using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using core7_oracle_angular14.Models;

namespace core7_oracle_angular14.Services
{    
    public interface IAuthService {
        Boolean SignupUser(UserRegister userdata);
        User SignUser(string usrname);
    }

    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;  

         IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

        public AuthService(
            IConfiguration configuration,
            IOptions<AppSettings> appSettings
            )
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public bool SignupUser(UserRegister userdata)
        {
    
            if (this.findEmail(userdata.Email) is true) {
                throw new AppException("Email Address is already taken...");
            }

            if (this.findUsername(userdata.Username) is true) {
                throw new AppException("Username was already taken...");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var xkey = config["AppSettings:Secret"];
            var key = Encoding.ASCII.GetBytes(xkey);

            // CREATE SECRET KEY FOR USER TOKEN===============
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userdata.Email)
                }),
                // Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var secret = tokenHandler.CreateToken(tokenDescriptor);
            var secretkey = tokenHandler.WriteToken(secret);

            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {                            
                _oracleConnection.Close();
                _oracleConnection.Open();
                cmd.CommandText = @"INSERT INTO users(ID,FIRSTNAME,LASTNAME,EMAIL,MOBILE,USERNAME,PASWORD,SECRETKEY,PROFILEPIC,ROLE) VALUES(id.nextval, :FIRSTNAME, :LASTNAME, :EMAIL, :MOBILE, :USERNAME, :PASWORD, :SECRETKEY, :PROFILEPIC, :ROLE)";
                cmd.Parameters.Add(new OracleParameter("FIRSTNAME", OracleDbType.Varchar2)).Value =userdata.Firstname;
                cmd.Parameters.Add(new OracleParameter("LASTNAME", OracleDbType.Varchar2)).Value =userdata.Lastname;
                cmd.Parameters.Add(new OracleParameter("EMAIL", OracleDbType.Varchar2)).Value =userdata.Email;
                cmd.Parameters.Add(new OracleParameter("MOBILE", OracleDbType.Varchar2)).Value =userdata.Mobile;
                cmd.Parameters.Add(new OracleParameter("USERNAME", OracleDbType.Varchar2)).Value =userdata.Username;
                cmd.Parameters.Add(new OracleParameter("PASWORD", OracleDbType.Varchar2)).Value =BCrypt.Net.BCrypt.HashPassword(userdata.Pasword);
                cmd.Parameters.Add(new OracleParameter("SECRETKEY", OracleDbType.Varchar2)).Value =secretkey.ToUpper();
                cmd.Parameters.Add(new OracleParameter("PROFILEPIC", OracleDbType.Varchar2)).Value ="http://localhost:5099/resources/users/pix.png";
                cmd.Parameters.Add(new OracleParameter("ROLE", OracleDbType.Varchar2)).Value = "USER";                    
                cmd.ExecuteNonQuery();
            }
            return true;
        }
        private bool findEmail(string _email) {
            using(var _oracleCon1 = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd1 = _oracleCon1.CreateCommand())
            {
                _oracleCon1.Open();
                var emailResult = cmd1.CommandText = @"SELECT COUNT(*) FROM users WHERE EMAIL = :EMAIL";
                cmd1.Parameters.Add(new OracleParameter("EMAIL", OracleDbType.Varchar2)).Value = _email;
                OracleDataReader EmailReader = cmd1.ExecuteReader();
                cmd1.BindByName = true;                    
                EmailReader.Read();
                if (EmailReader.GetInt32(0) > 0) {
                    EmailReader.Dispose();
                    _oracleCon1.Close();
                    _oracleCon1.Dispose();
                    return true;
                } 
                emailResult = null;
                EmailReader.Dispose();
                _oracleCon1.Close();
                _oracleCon1.Dispose();
            }    
            return false;
        }

        private bool findUsername(string _uname) {
            using(var _oracleCon2 = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd2 = _oracleCon2.CreateCommand())
            {
                _oracleCon2.Open();
                var usernameResult = cmd2.CommandText = @"SELECT COUNT(*) FROM users WHERE USERNAME = :USERNAME";
                cmd2.Parameters.Add(new OracleParameter("USERNAME", OracleDbType.Varchar2)).Value = _uname;
                OracleDataReader UsrnameReader = cmd2.ExecuteReader();
                UsrnameReader.Read();
                if (UsrnameReader.GetInt32(0) > 0) {
                    UsrnameReader.Dispose();
                    _oracleCon2.Close();
                    _oracleCon2.Dispose();                    
                    return true;
                } 
                usernameResult = null;
                UsrnameReader.Dispose();
                _oracleCon2.Close();
                _oracleCon2.Dispose();
          }
            return false;
        }
        public User SignUser(string usrname)
        {
          try {
            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {           
                _oracleConnection.Close();
                _oracleConnection.Open();                
                var usernameResult = cmd.CommandText = @"SELECT id,firstname,lastname,email,mobile,username,role,isactivated,isblocked,profilepic,pasword,qrcodeurl FROM users WHERE USERNAME = :USERNAME";
                cmd.Parameters.Add(new OracleParameter("USERNAME", OracleDbType.Varchar2)).Value = usrname;
                cmd.BindByName = true;
                OracleDataReader UserReader = cmd.ExecuteReader();
                UserReader.Read();
                User user = new User();
                user.Id = UserReader.GetInt32(0);
                user.Firstname = UserReader.GetString(1);
                user.Lastname = UserReader.GetString(2);
                user.Email = UserReader.GetString(3);
                user.Mobile = UserReader.GetString(4);
                user.Username = UserReader.GetString(5);
                user.Roles = UserReader.GetString(6);
                user.Isactivated = UserReader.GetInt32(7);
                user.Isblocked = UserReader.GetInt32(8);
                user.Profilepic = UserReader.GetString(9);
                user.Pasword = UserReader.GetString(10);
                try {
                    user.Qrcodeurl = UserReader.GetString(11);
                } catch(Exception) {
                    user.Qrcodeurl="";
                }
                return user;
            }
          } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                throw new AppException("Username not foud.");
          }
        }
    }
}