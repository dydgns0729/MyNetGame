using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace MyNetGame.Component
{
    public class MyUserLevelUp
    {
        //Empty
    }

    /// <summary>
    /// 유저 레벨업 기능 정의
    /// </summary>
    public interface IUserLevelUpRepository
    {
        UserLevelUpResult LevelUpUser(UserLevelUp userLevelUp);
    }

    /// <summary>
    /// 유저 레벨업 기능 구현
    /// </summary>
    public class UserLevelUpRepository : IUserLevelUpRepository
    {
        private IConfiguration _config;
        private IDbConnection db;

        public UserLevelUpRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(_config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        public UserLevelUpResult LevelUpUser(UserLevelUp userLevelUp)
        {
            string sql = "usp_LevelUp @UserId";
            var resultUser = db.Query<int>(sql, userLevelUp).Single();

            if (resultUser > 0)
            {
                return new UserLevelUpResult
                {
                    Protocol = -userLevelUp.Protocol,
                    Result = 0,
                    UserId = userLevelUp.UserId,
                    Level = resultUser
                };
            }
            else
            {
                return new UserLevelUpResult
                {
                    Protocol = -userLevelUp.Protocol,
                    Result = 1,
                    UserId = userLevelUp.UserId,
                    Level = 0
                };
            }
        }
    }

    /// <summary>
    /// Web-API 구현
    /// </summary>
    [Route("api/[controller]")]
    public class UserLevelUpServicesController : ControllerBase
    {
        private IUserLevelUpRepository _repository;
        public UserLevelUpServicesController(IUserLevelUpRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Post([FromBody] UserLevelUp userLevelUp)
        {
            if (userLevelUp == null)
            {
                return BadRequest();
            }
            try
            {
                UserLevelUpResult result = _repository.LevelUpUser(userLevelUp);
                return Ok(result);
            }
            catch
            {
                UserLevelUpResult result = new UserLevelUpResult
                {
                    Protocol = -userLevelUp.Protocol,
                    Result = 1,
                    UserId = userLevelUp.UserId,
                    Level = 0
                };
                return Ok(result);
            }
        }
    }


    /// <summary>
    /// 유저 레벨업 요청 : Protocol : 1104
    /// </summary>
    public class UserLevelUp
    {
        public int Protocol { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
    }

    /// <summary>
    /// 유저 레벨업 응답 : Protocol : -1104
    /// </summary>
    public class UserLevelUpResult
    {
        public int Protocol { get; set; }
        public int Result { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
        public int Level { get; set; }
    }
}