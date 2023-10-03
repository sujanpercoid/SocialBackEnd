using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.SqlClient;
using TClone.Data;
using TClone.Models;

namespace TClone.Services
{
    public class Noti : INotification
    {
        private readonly TcDbcontext _noti;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public Noti(TcDbcontext noti, IMapper mapper, IWebHostEnvironment environment, IConfiguration config)
        {
            _noti = noti;
            _mapper = mapper;
            _environment = environment;
            _config = config;
        }
        //Connection string for sql for whole class
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("conn"));
        }
        //Display All Notification
        public async Task<List<Notification>>Notification(string id)
        {
            using var connection = CreateConnection();
            var sqlQuery = @"select *
                            from Notifications
                            where username = @username 
                            ORDER BY NotiId Desc;";
            var parameter = new { username = id };
            var info = await connection.QueryAsync<Notification>(sqlQuery, parameter);
            return info.ToList();

        }
        //Remove All Notification 
        public async Task<string> DeleteAllNoti (string id)
        {
            var noti = await _noti.Notifications.Where(not => not.Username == id).ToListAsync();
            _noti.Notifications.RemoveRange(noti);
            await _noti.SaveChangesAsync();
            var resultMessage = new { message = " All Notification Deleted !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
       
        //Delete a single notification
        public async Task<string>DeleteNoti(int id)
        {
            var noti = await _noti.Notifications.FindAsync(id);
            _noti.Notifications.Remove(noti);
            await _noti.SaveChangesAsync();
            var resultMessage = new { message = "  Notification Deleted !!" };
            return (JsonConvert.SerializeObject(resultMessage));
        }
    }
}
