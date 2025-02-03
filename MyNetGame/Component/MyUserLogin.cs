using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace MyNetGame.Component
{
    public class MyUserLogin
    {
        //Empty
    }

    /// <summary>
    /// 로그인 기능 정의
    /// </summary>
    public interface IUserLoginRepository
    {
        UserLoginResult LoginUser(UserLogin userLogin);
    }

    /// <summary>
    /// 로그인 기능 구현
    /// </summary>
    public class UserLoginRepository : IUserLoginRepository
    {
        private IConfiguration _config;
        private IDbConnection db;

        public UserLoginRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(_config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }
        public UserLoginResult LoginUser(UserLogin userLogin)
        {
            string sql = "usp_Login @UserId, @Password";
            var result = db.Query<int>(sql, userLogin).Single();
            UserLoginResult loginResult = new UserLoginResult
            {
                Protocol = -userLogin.Protocol,
                Result = result,
                UserId = userLogin.UserId
            };


            return loginResult;

        }
    }

    /// <summary>
    /// 로그인 Web-API
    /// </summary>
    [Route("api/[controller]")]
    public class UserLoginServicesController : ControllerBase
    {
        private IUserLoginRepository _repository;

        public UserLoginServicesController(IUserLoginRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest();
            }

            try
            {
                UserLoginResult userLoginResult = _repository.LoginUser(userLogin);
                return Ok(userLoginResult);
            }
            catch
            {
                UserLoginResult result = new UserLoginResult
                {
                    Protocol = -userLogin.Protocol,
                    Result = 2,
                    UserId = userLogin.UserId

                };
                return Ok(result);
            }
        }
    }




    /// <summary>
    /// 유저 로그인 요청 : Protocol : 1101
    /// </summary>
    public class UserLogin
    {
        public int Protocol { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
    }

    /// <summary>
    /// 유저 로그인 응답 : Protocol : -1101
    /// </summary>
    public class UserLoginResult
    {
        public int Protocol { get; set; }
        public int Result { get; set; }
        public string UserId { get; set; }
    }

}
