using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.API.Models.Forum
{
    public partial class Forum
    {
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
}
