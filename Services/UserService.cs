using System.Data;
using core7_oracle_angular14.Entities;
using core7_oracle_angular14.Helpers;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace core7_oracle_angular14.Services
{
    public interface IUserService {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Update(User user);
        void Delete(int id);
        void ActivateMfa(int id, bool opt, string qrcode_url);
        void UpdatePicture(int id, string file);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;  

         IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

        public UserService(
            IConfiguration configuration,
            IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public void Delete(int id)
        {
          try {
            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();                
                cmd.CommandText = @"DELETE FROM users WHERE ID = :ID";
                cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = id;
                cmd.BindByName = true;
                cmd.ExecuteNonQuery();
                _oracleConnection.Close();
                _oracleConnection.Dispose();
            }
          } catch(Exception) {
               throw new AppException("User ID not found");
          }
        }

        public IEnumerable<User> GetAll()
        {
          List<User> listUser = new List<User>();
          try {
            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {
                _oracleConnection.Open();                
                cmd.CommandText = @"SELECT id,firstname,lastname,email,mobile,username,role,isactivated,isblocked,profilepic FROM users ORDER BY id ASC";
                OracleDataReader userReader = cmd.ExecuteReader();
                User user;
                while (userReader.Read()) {
                    user  = new User();
                    user.Id = Convert.ToInt32(userReader["ID"]);
                    user.Firstname = Convert.ToString(userReader["FIRSTNAME"]);
                    user.Lastname = Convert.ToString(userReader["LASTNAME"]);
                    user.Email = Convert.ToString(userReader["EMAIL"]);
                    user.Mobile = Convert.ToString(userReader["MOBILE"]);
                    user.Username = Convert.ToString(userReader["USERNAME"]);
                    user.Roles = Convert.ToString(userReader["ROLE"]);
                    user.Isactivated = Convert.ToInt32(userReader["ISACTIVATED"]);
                    user.Isblocked = Convert.ToInt32(userReader["ISBLOCKED"]);
                    user.Profilepic = Convert.ToString(userReader["PROFILEPIC"]);
                    listUser.Add(user);
                }
                userReader.Close();
                userReader.Dispose();
                _oracleConnection.Close();
                _oracleConnection.Dispose();
                return listUser;     
            }
          } catch(Exception ex) {
            throw new AppException(ex.Message);
          }
        }

        public User GetById(int id)
        {
          try {
            using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
            using(var cmd = _oracleConnection.CreateCommand())
            {           
                _oracleConnection.Open();                
                var usernameResult = cmd.CommandText = @"SELECT id,firstname,lastname,email,mobile,username,role,isactivated,isblocked,profilepic,secretkey,qrcodeurl FROM users WHERE ID = :ID";
                cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = id;
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
                user.Secretkey = UserReader.GetString(10);
                try {
                    user.Qrcodeurl = UserReader.GetString(11);                
                } catch(Exception) {}
                _oracleConnection.Close();
                _oracleConnection.Dispose();
                return user;
            }
          } catch(Exception) {
            throw new AppException("User ID not foud.");
          }
        }

        public void Update(User user)
        {
            try {
                using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
                using(var cmd = _oracleConnection.CreateCommand())
                {                            
                    _oracleConnection.Open();
                    if (user.Pasword is not null) {                    
                        cmd.CommandText = @"UPDATE users SET FIRSTNAME = :FIRSTNAME, LASTNAME = :LASTNAME , MOBILE = :MOBILE ,PASWORD = :PASWORD WHERE ID = :ID";
                        cmd.Parameters.Add(new OracleParameter("FIRSTNAME", OracleDbType.Varchar2)).Value = user.Firstname;
                        cmd.Parameters.Add(new OracleParameter("LASTNAME", OracleDbType.Varchar2)).Value = user.Lastname;
                        cmd.Parameters.Add(new OracleParameter("MOBILE", OracleDbType.Varchar2)).Value = user.Mobile;
                        cmd.Parameters.Add(new OracleParameter("PASWORD", OracleDbType.Varchar2)).Value = BCrypt.Net.BCrypt.HashPassword(user.Pasword);
                        cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = user.Id;
                        cmd.ExecuteNonQuery();
                    } else {
                        cmd.CommandText = @"UPDATE users SET FIRSTNAME = :FIRSTNAME, LASTNAME = :LASTNAME , MOBILE = :MOBILE WHERE ID = :ID";
                        cmd.Parameters.Add(new OracleParameter("FIRSTNAME", OracleDbType.Varchar2)).Value = user.Firstname;
                        cmd.Parameters.Add(new OracleParameter("LASTNAME", OracleDbType.Varchar2)).Value = user.Lastname;
                        cmd.Parameters.Add(new OracleParameter("MOBILE", OracleDbType.Varchar2)).Value = user.Mobile;
                        cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = user.Id;
                        cmd.ExecuteNonQuery();
                    }
                    _oracleConnection.Close();
                    _oracleConnection.Dispose();
                }
            } catch(Exception ex) {
                throw new AppException(ex.Message);
            }
        }
        public void ActivateMfa(int id, bool opt, string qrcode_url)
        {
            try {
                using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
                using(var cmd = _oracleConnection.CreateCommand())
                {                            
                    _oracleConnection.Close();
                    _oracleConnection.Open();
                    if (opt is true) {                    
                        cmd.CommandText = @"UPDATE users SET QRCODEURL = :QRCODEURL WHERE ID = :ID";
                        cmd.Parameters.Add(new OracleParameter("QRCODEURL", OracleDbType.Varchar2)).Value = qrcode_url;
                        cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = id;
                        cmd.ExecuteNonQuery();
                    } else {
                        cmd.CommandText = @"UPDATE users SET QRCODEURL = :QRCODEURL WHERE ID = :ID";
                        cmd.Parameters.Add(new OracleParameter("QRCODEURL", OracleDbType.Varchar2)).Value = null;
                        cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = id;
                        cmd.ExecuteNonQuery();
                    }
                    _oracleConnection.Close();
                    _oracleConnection.Dispose();
                }
            } catch(Exception ex) {
                throw new AppException(ex.Message);
            }
        }
        public void UpdatePicture(int id, string file)
        {
            try {
                using(var _oracleConnection = new OracleConnection(_configuration.GetSection("ConnectionStrings").GetSection("OracleConnection").Value))
                using(var cmd = _oracleConnection.CreateCommand())
                {                            
                    _oracleConnection.Close();
                    _oracleConnection.Open();
                    cmd.CommandText = @"UPDATE users SET PROFILEPIC = :PROFILEPIC WHERE ID = :ID";
                    cmd.Parameters.Add(new OracleParameter("PROFILEPIC", OracleDbType.Varchar2)).Value = file;
                    cmd.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32)).Value = id;
                    cmd.ExecuteNonQuery();
                    _oracleConnection.Close();
                    _oracleConnection.Dispose();
                }
            } catch(Exception ex) {
                throw new AppException(ex.Message);
            }
        }
    }

}