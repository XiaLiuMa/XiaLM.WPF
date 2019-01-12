using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Quartz.Hubs
{
    public class MyChatHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// 开始跟随
        /// </summary>
        /// <param name="isWhite">是否跟随白名单</param>
        /// <returns></returns>
        public async Task StartFollow(bool isWhite)
        {
            
        }

        /// <summary>
        /// 反馈跟随状态
        /// </summary>
        /// <param name="state"></param>
        public async Task FeedBackFollowState(string state)
        {
            await this.Clients.All.SendAsync("FeedBackFollowState", state);
        }
    }
}
