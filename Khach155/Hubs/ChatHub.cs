using Khach155.Data;
using Khach155.Models;
using Khach155.Models.RoomViewModel;
using Khach155.Models.MessageViewModel;
using Khach155.Models.DataUserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Khach155.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            int userId = _context.DataUser.FirstOrDefault(x => x.UserName == user).Id ?? 0;

            Message messages = new();
            messages.Timestamp = DateTime.Now;
            messages.ToRoomId = userId;
            messages.Content = message;
            messages.FromUser = userId;
            await _context.AddAsync(messages);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public async Task HistoryChat(string user)
        {
            var history = await _context.Message.ToListAsync();
            await Clients.All.SendAsync("ReceiveMessageHistory", history);
        }





        public async Task SendMessageAdmin(string user_nguoiDung, string mesage)
        {
            int userId = _context.DataUser.FirstOrDefault(x => x.UserName == user_nguoiDung)!.Id ?? 0;

            Message messages = new();
            messages.Timestamp = DateTime.Now;
            messages.ToRoomId = userId;
            messages.Content = mesage;
            messages.FromUser = 9;
            await _context.AddAsync(messages);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessageAdmin", 9, mesage);
        }

        public async Task HistoryChatAdmin()
        { 
            var allHistory = await _context.Message.ToListAsync();
            var history = allHistory.GroupBy(x => x.ToRoomId);
            await Clients.All.SendAsync("ReceiveMessageHistoryAdmin", history);
        }
    }


    public class SignalRReconnectMiddleware
    {
        private readonly RequestDelegate _next;

        public SignalRReconnectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = httpContext.RequestAborted;
            while (true)
            {
                try
                {
                    await _next(httpContext);
                    return;
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken == token)
                    {
                        throw;
                    }
                    // Reconnect on cancelation exception
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }
    }

}
