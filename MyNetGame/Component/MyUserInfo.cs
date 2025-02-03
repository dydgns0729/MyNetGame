using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace MyNetGame.Component
{
    public class MyUserInfo
    {
        //Empty
    }

    /// <summary>
    /// 유저정보 조회 기능 정의
    /// </summary>
    public interface IUserInfoRepository
    {
        UserInfoResult GetUserInfo(UserInfo userInfo);
    }

    /// <summary>
    /// 유저 정보 조회 기능 구현
    /// </summary>
    public class UserInfoRepository : IUserInfoRepository
    {
        private IConfiguration _config;
        private IDbConnection db;

        public UserInfoRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(_config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        public UserInfoResult GetUserInfo(UserInfo userInfo)
        {
            string sql = "usp_UserInfo @UserId";
            User resultUser = db.Query<User>(sql, userInfo).Single();

            if (resultUser != null)
            {
                return new UserInfoResult
                {
                    Protocol = -userInfo.Protocol,
                    Result = 0,
                    UserId = resultUser.UserId,
                    Level = resultUser.Level,
                    Gold = resultUser.Gold
                };
            }
            else
            {
                return new UserInfoResult
                {
                    Protocol = -userInfo.Protocol,
                    Result = 1,
                    UserId = userInfo.UserId,
                    Level = 0,
                    Gold = 0
                };
            }
        }
    }

    /// <summary>
    /// Web-API 구현
    /// </summary>
    [Route("api/[controller]")]
    public class UserInfoServicesController : ControllerBase
    {
        private IUserInfoRepository _repository;

        public UserInfoServicesController(IUserInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserInfo userinfo)
        {
            if (userinfo == null)
            {
                return BadRequest();
            }

            try
            {
                UserInfoResult infoResult = _repository.GetUserInfo(userinfo);
                return Ok(infoResult);
            }
            catch
            {
                UserInfoResult infoResult = new UserInfoResult
                {
                    Protocol = -userinfo.Protocol,
                    Result = 1,
                    UserId = userinfo.UserId,
                    Level = 0,
                    Gold = 0
                };
                return Ok(infoResult);
            }
        }
    }


    /// <summary>
    /// 유저정보 가져오기 요청 : Protocol : 1103
    /// </summary>
    public class UserInfo
    {
        public int Protocol { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
    }

    /// <summary>
    /// 유저정보 가져오기 응답 : Protocol : -1103
    /// </summary>
    public class UserInfoResult
    {
        public int Protocol { get; set; }
        public int Result { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
        [MaxLength(20)]
        public int Level { get; set; }
        public int Gold { get; set; }
    }
}
