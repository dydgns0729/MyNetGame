﻿using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.ComponentModel.DataAnnotations;

namespace MyNetGame.Component
{
    public class MyUserAdd
    {
        //Empty
    }

    /// <summary>
    /// 유저 생성 기능 정의
    /// </summary>
    public interface IUserAddRepository
    {
        UserAddResult AddUser(UserAdd userAdd);
    }

    /// <summary>
    /// 유저 생성 기능 구현
    /// </summary>
    public class UserAddRepository : IUserAddRepository
    {
        private IConfiguration _config;
        private IDbConnection db;

        public UserAddRepository(IConfiguration config)
        {
            _config = config;
            db = new SqlConnection(_config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        public UserAddResult AddUser(UserAdd userAdd)
        {
            string sql = "usp_AddUser @UserId, @Password";
            var result = db.Query<int>(sql, userAdd).Single();

            //결과값 세팅
            UserAddResult userAddResult = new UserAddResult
            {
                Protocol = -userAdd.Protocol,
                Result = result,
                UserId = userAdd.UserId
            };
            return userAddResult;
        }
    }

    /// <summary>
    /// Web - Api 구현
    /// </summary>
    [Route("api/[controller]")]
    public class UserAddServicesController : ControllerBase
    {
        #region Variables
        private IUserAddRepository _repository;
        #endregion

        public UserAddServicesController(IUserAddRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserAdd userAdd)
        {
            if (userAdd == null)
            {
                return BadRequest();
            }

            try
            {
                UserAddResult addResult = _repository.AddUser(userAdd);
                if (addResult == null)
                {
                    return BadRequest("유저 추가 실패");
                }
                return Ok(addResult);
            }
            catch
            {
                //결과값 세팅
                UserAddResult result = new UserAddResult
                {
                    Protocol = -userAdd.Protocol,
                    Result = 2,
                    UserId = userAdd.UserId
                };
                return Ok(result);
            }
        }
    }

    /// <summary>
    /// 유저 생성 요청 - protocal : 1102
    /// </summary>
    public class UserAdd
    {
        public int Protocol { get; set; }
        [MaxLength(20)]
        public string UserId { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
    }

    /// <summary>
    /// 유저 생성 응답 - protocal : -1102
    /// </summary>
    public class UserAddResult
    {
        public int Protocol { get; set; }
        public int Result { get; set; }
        public string UserId { get; set; }
    }
}
