﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegaSharpProject.Domain.Info;

namespace TelegaSharpProject.Domain.Interfaces
{
    public interface IDBWorker : IDisposable
    {
        public Task<UserInfo> GetUserInfoAsync(long userId);

        public Task<UserInfo[]> GetLeaderBoardAsync();

        public Task<TaskInfo> GetTaskAsync(int page);

        public void CloseTask(long taskID);

        public void CommentTask(long taskID, long byUser, string text);

        public Task<CommentInfo[]> GetCommentsToTask(long taskID);

        public  Task<CommentInfo[]> GetCommentsFromUser(long userID);

        public void SendTaskAsync(long byUserID, string task);

        public Task RegisterUser(long userId, string userName);
    }
}