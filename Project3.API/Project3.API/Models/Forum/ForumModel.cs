using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.API.Models.Forum
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int UserId{ get; set; }
        public int PlatformId { get; set; }
        public DateTimeOffset LastActivity { get; set; }
    }

    public class Thread
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public DateTimeOffset LastActivity { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Created { get; set; }
        public int UserId { get; set; }
        public int ThreadId { get; set; }
        public int UpCount { get; set; }
        public int DownCount { get; set; }
        public int SourcePostId { get; set; }
        public DateTimeOffset LastActivity { get; set; }
    }
}
